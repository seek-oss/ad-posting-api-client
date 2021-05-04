#!/usr/bin/env bash

set -eou pipefail

nupkgFilePath=${1?param missing - Specifies the file path to the package to be pushed. e.g. path/to/file/nuget.nupkg}
nugetApiKey=${2?param missing - The API key for the server.}
nugetServerUrl=${3?param missing - Specifies the server URL.}

dotnet nuget push ${nupkgFilePath} -k ${nugetApiKey} -s ${nugetServerUrl}