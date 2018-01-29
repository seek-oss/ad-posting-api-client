@echo off
setlocal

cd /d "%~dp0"

set PackagesDir=packages
set NuGetPath=.nuget\NuGet.exe
set FakePath=%PackagesDir%\FAKE\tools\Fake.exe
set RubyVersion=ruby-2.3.3-x64-mingw32
set RubyPackage=%RubyVersion%.7z

if not exist "%FakePath%" "%NuGetPath%" Install FAKE -Version 4.26.0 -OutputDirectory "%PackagesDir%" -ExcludeVersion
if errorlevel 1 goto :end

if "%1" equ "UploadPact" goto :doPactPrep
if "%1" equ "PactMarkdown" goto :doPactPrep
goto :dobuild

:doPactPrep

if not exist "%PackagesDir%\7-Zip.CommandLine" "%NuGetPath%" Install 7-Zip.CommandLine -Version 9.20.0 -OutputDirectory "%PackagesDir%" -ExcludeVersion
if errorlevel 1 goto :end

if not exist "%PackagesDir%\%RubyPackage%" powershell.exe -NoProfile -ExecutionPolicy Bypass -Command Invoke-WebRequest "http://dl.bintray.com/oneclick/rubyinstaller/%RubyPackage%" -OutFile "packages\%RubyPackage%"
if errorlevel 1 goto :end

if not exist "%PackagesDir%\%RubyVersion%" "%PackagesDir%\7-Zip.CommandLine\tools\7za" x "%PackagesDir%\%RubyPackage%" -o".\%PackagesDir%" -y
if errorlevel 1 goto :end

cmd /c "%PackagesDir%\%RubyVersion%\bin\gem" install pact:1.9.1
if errorlevel 1 goto :end

set branch=--envvar branch %2

set PATH=%PackagesDir%\%RubyVersion%\bin;%PATH%

:dobuild

"%FakePath%" build.fsx %1 --nocache %branch%
if errorlevel 1 goto :end
endlocal

if "%1" neq "UploadPact" goto :end

git add --force ../pact/ad_posting_api_client-ad_posting_api.json ../pact/ad_posting_api_client-ad_posting_template_api.json
git add --force "../pact/README.md"
git add --force "../pact/Ad Posting API Client - Ad Posting API.md"
git add --force "../pact/Ad Posting API Client - Ad Posting Template API.md"
if errorlevel 1 goto :end

git commit --author="Build Agent <%3>" --message="update pact"

git push origin HEAD
if errorlevel 1 goto :end

:end
exit /b %errorlevel%
