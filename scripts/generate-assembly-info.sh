#!/usr/bin/env bash

set -eou pipefail

fileVersion=${1:-}
normalisedGitBranch=${2:-}

placeHolderVersion='0.15.630.1108'
placeholderPackageVersion='-commitHashPlaceholder-commitBranchPlaceholder'

if [[ -z ${fileVersion} ]] || [[ -z ${normalisedGitBranch} ]]; then
    echo '"file version" or "normalised git branch" not input or empty - Not updating AssemblyInfoGenerated.cs'
else
    versionSuffix="-${normalisedGitBranch}"

    if [[ ${normalisedGitBranch} == 'master' ]]; then
        versionSuffix=""
    fi

    echo "Updating version to '${fileVersion}' and informational version suffix to '${versionSuffix}' in AssemblyInfoGenerated.cs"
    sed -e "s/${placeHolderVersion}/${fileVersion}/g" \
        -e "s/${placeholderPackageVersion}/${versionSuffix}/g" \
        -i './src/SEEK.AdPostingApi.Client/Properties/AssemblyInfoGenerated.cs' \
        './src/SEEK.AdPostingApi.Client/Properties/AssemblyInfoGenerated.cs'
fi
