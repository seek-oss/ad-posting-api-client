@echo off
setlocal

cd /d "%~dp0"

set PackagesDir=packages
set NuGetPath=.nuget\NuGet.exe
set FakePath=%PackagesDir%\FAKE\tools\Fake.exe
set RubyVersion=ruby-2.3.0-x64-mingw32
set RubyPackage=%RubyVersion%.7z

if not exist "%FakePath%" "%NuGetPath%" Install FAKE -Version 4.26.0 -OutputDirectory "%PackagesDir%" -ExcludeVersion
if not errorlevel 0 goto :eof

if not exist "%PackagesDir%\7-Zip.CommandLine" "%NuGetPath%" Install 7-Zip.CommandLine -Version 9.20.0 -OutputDirectory "%PackagesDir%" -ExcludeVersion
if not errorlevel 0 goto :eof

if not exist "%PackagesDir%\%RubyPackage%" powershell.exe -NoProfile -ExecutionPolicy Bypass -Command Invoke-WebRequest "http://dl.bintray.com/oneclick/rubyinstaller/%RubyPackage%" -OutFile "packages\%RubyPackage%"
if not errorlevel 0 goto :eof

if not exist "%PackagesDir%\%RubyVersion%" "%PackagesDir%\7-Zip.CommandLine\tools\7za" x "%PackagesDir%\%RubyPackage%" -o".\%PackagesDir%" -y
if not errorlevel 0 goto :eof

cmd /c "%PackagesDir%\%RubyVersion%\bin\gem" install pact:1.9.1
if not errorlevel 0 goto :eof

if "%1" equ "UploadPact" set branch=--envvar branch %2

set PATH=%PackagesDir%\%RubyVersion%\bin;%PATH%
"%FakePath%" build.fsx %1 --nocache %branch%
endlocal

if "%1" neq "UploadPact" goto :eof

git add --force ../pact/ad_posting_api_client-ad_posting_api.json ../pact/README.md
if not errorlevel 0 goto :eof

git commit --author="Build Agent <%3>" --message="update pact"
if not errorlevel 0 goto :eof

git push origin HEAD
if not errorlevel 0 goto :eof

:end
exit /b %errorlevel%
