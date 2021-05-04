#!/usr/bin/env bash

set -eou pipefail

cd "./test/SEEK.AdPostingApi.Client.Tests"
# Running Pact Test and generate Pack Contract in JSON file, the output: ./pact/ad_posting_api_client-ad_posting_api.json 
dotnet test

cd "../.."

dotnet script ./scripts/generate-pact-markdown.csx

sleep 5

git add --force ./pact
git commit -m "Update PACTs" || true
git push