@echo off
setlocal

cd /d "%~dp0"

set PackagesDir=packages
set NuGetPath=.nuget\NuGet.exe
set FakePath=%PackagesDir%\FAKE\tools\Fake.exe

if not exist "%FakePath%" "%NuGetPath%" Install FAKE -Version 4.26.0 -OutputDirectory "%PackagesDir%" -ExcludeVersion
if errorlevel 1 goto :end

if "%1" equ "UploadPact" goto :doPactPrep
if "%1" equ "PactMarkdown" goto :doPactPrep
goto :dobuild

:doPactPrep

if not exist "%PackagesDir%\7-Zip.CommandLine" "%NuGetPath%" Install 7-Zip.CommandLine -Version 9.20.0 -OutputDirectory "%PackagesDir%" -ExcludeVersion
if errorlevel 1 goto :end

set branch=--envvar branch %2

:dobuild

"%FakePath%" build.fsx %1 --nocache %branch%
if errorlevel 1 goto :end
endlocal

if "%1" neq "UploadPact" goto :end

git add --force ../pact/ad_posting_api_client-ad_posting_api.json ../pact/README.md
if errorlevel 1 goto :end

git commit --author="Build Agent <%3>" --message="update pact"

git push origin HEAD
if errorlevel 1 goto :end

:end
exit /b %errorlevel%
