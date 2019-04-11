#!/usr/bin/env bash

set -eou pipefail

pactBrokerUrl=${1?param missing - PACT broker URL is required e.g. http://pactbroker}
apiVersion=${2?param missing - API version e.g. 1.0}
buildNumber=${3?param missing - build number is missing e.g. 42}

dateStamp=$(date +"%Y%m%d")
pactVersion=${apiVersion}.${dateStamp}.${buildNumber}
gitBranch=$(git rev-parse --abbrev-ref HEAD)
pactTag=$(echo "${gitBranch}" | sed 's|[^[:alnum:]]|-|g')

function publishPact {
  local pactConsumer="Ad%20Posting%20API%20Client"
  local pactFile=${1}
  local pactProvider=${2}

  # Publish PACT
  curl -XPUT \-H "Content-Type: application/json" \
  -d "@./pact/${pactFile}.json" \
  "${pactBrokerUrl}/pacts/provider/${pactProvider}/consumer/${pactConsumer}/version/${pactVersion}" \
  --fail ${PACT_BROKER_USERNAME:+ -u ${PACT_BROKER_USERNAME}${PACT_BROKER_PASSWORD:+\:${PACT_BROKER_PASSWORD}}}

  # Tag PACT
  curl -XPUT \-H "Content-Type: application/json" -H "Accept: application/hal+json" \
  "${pactBrokerUrl}/pacticipants/${pactConsumer}/versions/${pactVersion}/tags/${pactTag}" \
  --fail ${PACT_BROKER_USERNAME:+ -u ${PACT_BROKER_USERNAME}${PACT_BROKER_PASSWORD:+\:${PACT_BROKER_PASSWORD}}}
}

publishPact 'ad_posting_api_client-ad_posting_api' 'Ad%20Posting%20API'
publishPact 'ad_posting_api_client-ad_posting_logo_api' 'Ad%20Posting%20Logo%20API'
publishPact 'ad_posting_api_client-ad_posting_template_api' 'Ad%20Posting%20Template%20API'
