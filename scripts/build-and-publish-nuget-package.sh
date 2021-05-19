#!/usr/bin/env bash

set -eou pipefail

if [[ $# -lt 2 ]]; then
  printf "\e[1;37mUsage:\n\e[0m%s\n\n\e[1;37mArguments:\e[0m\n%s\n%s\n\n\e[1;37mExample:\e[0m\n%s\n%s\n\n" \
    "`basename ${0}` <version> <publish-stage>" \
    "version: nuget package version (e.g. '1.2.3' or '1.2.4-pre-release')"\
    "public-stage: publish the package to nuget internal (proget) or public (nuget.org) (e.g. 'internal' or 'public')"\
    "`basename ${0}` 2.0.100 public" \
    "`basename ${0}` 2.1.100-beta internal"
  
  exit 1
fi

RED=`tput setaf 1`
GREEN=`tput setaf 2`
RESET=`tput sgr0`

printBreakLine() {
    printf %100s |tr " " "="
    printf "\n"
}

printError() {
    echo "${RED}$1"
}

printStep() {
    local stepMessage=${1?param missing - stepMessage}
    echo "${GREEN}${stepMessage} ...${RESET}"
}

version=${1?param missing - version e.g. \'1.2.3\' or \'1.2.4-pre-release\'}
publishStage=${2?param missing - stage e.g. \'internal\' or \'public\'}

source "./scripts/aws-secretmanager.sh"

nugetSecretPublicId="prod/ad-posting-api-client/nuget/public"
nugetSecretInternalId="prod/ad-posting-api-client/nuget/internal"


nugetSecretPublicValue=$(getSecretValue "${nugetSecretPublicId}")
nugetSecretInternalValue=$(getSecretValue "${nugetSecretInternalId}")

if [ -z "$nugetSecretPublicValue" ]; then
    printError "ERROR: Nuget secret not found.";
    exit 1
fi

nugetApiPublicKey=$(getSecretValueProperty "${nugetSecretPublicValue}" "nugetApiKey")
nugetServerPublicUrl=$(getSecretValueProperty "${nugetSecretPublicValue}" "nugetServerUrl")
nugetApiInternalKey=$(getSecretValueProperty "${nugetSecretInternalValue}" "nugetApiKey")
nugetServerInternalUrl=$(getSecretValueProperty "${nugetSecretInternalValue}" "nugetServerUrl")

#-------------
printStep "Update build version ${version} ..."

sh ./scripts/update-build-version.sh ${version}

printStep "UPDATED BUILD VERSION ${version} SUCCESSFUL!"

#-------------

printBreakLine

printStep "PACKING THE NUGET PACKAGE..."

dotnet pack

printStep "PACKING THE NUGET PACKAGE 'SEEK.AdPostingApi.Client.${version}.nupkg' SUCCESSFUL!"

#-------------

printBreakLine

# Always publish internal first to verify the package
printStep "PUBLISHING NUGET PACKAGE - INTERNAL..."

nugetPackage="SEEK.AdPostingApi.Client.${version}.nupkg"

nugetPackageFilePath="./src/SEEK.AdPostingApi.Client/bin/debug/${nugetPackage}"
sh ./scripts/publish-nuget-package.sh ${nugetPackageFilePath} ${nugetApiInternalKey} ${nugetServerInternalUrl}

printStep "PUBLISHED NUGET PACKAGE (${nugetPackage}) SUCCESSFUL- INTERNAL."
#-------------

printBreakLine

printStep "BUILD SAMPLE PROJECT AGAINST 'SEEK.AdPostingApi.Client.${version}.nupkg'..."

dotnet add sample/SEEK.AdPostingApi.SampleConsumer.csproj package SEEK.AdPostingApi.Client --version ${version} -n
dotnet build sample/SEEK.AdPostingApi.SampleConsumer.csproj --no-cache

printStep "BUILT SAMPLE PROJECT AGAINST 'SEEK.AdPostingApi.Client.${version}.nupkg' SUCCESSFUL!"

#-------------
printBreakLine

printStep "UPDATING PACT CONTRACT..."

sh ./scripts/generate-pact-contract.sh ${version}

printStep "UPDATED PACT CONTRACT SUCCESSFUL"

# #-------------
# printBreakLine

# printStep "PUBLISHING PACT TO PACK BROKER ..."

#-------------
if [ $publishStage == "public" ]; then
    #-------------
    printBreakLine

    printStep "CREATING GITHUB RELEASE IN SEEK-JOBs..."

    git checkout master
    git pull --rebase

    sh ./scripts/create-github-release.sh origin seek ad-posting-api-client ${version}

    #-------------
    printBreakLine

    printStep "SYNC TO GITHUB SEEK-OSS"
    
    git checkout master
    git push oss master

    sh ./scripts/create-github-release.sh oss seek ad-posting-api-client ${version}

    #-------------

    printBreakLine

    printStep "PUBLISHING NUGET PACKAGE - ${publishStage}..."

    nugetPackage="SEEK.AdPostingApi.Client.${version}.nupkg"

    nugetPackageFilePath="./src/SEEK.AdPostingApi.Client/bin/debug/${nugetPackage}"
    sh ./scripts/publish-nuget-package.sh ${nugetPackageFilePath} ${nugetApiPublicKey} ${nugetServerPublicUrl}

    printStep "PUBLISHED NUGET PACKAGE (${nugetPackage}) SUCCESSFUL- ${publishStage}."
fi 

printStep "THE PACKAGE HAS BEEN PUBLISHED SUCCESSFULLY WITH ALL STEPS- ${publishStage}"