#!/usr/bin/env bash

set -eou pipefail

version=${1?param missing - version e.g. \'1.2.3\' or \'1.2.4-pre-release\'}


sed -i '' -e "s/<PackageVersion>.*<\/PackageVersion>/<PackageVersion>${version}<\/PackageVersion>/g" \
        './src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj'

git add ./src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj
git commit -m "Update build version to ${version}" || true
git push

