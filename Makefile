
BUILD_NUMBER ?= 0
PACT_BROKER_URL ?= 
GIT_BRANCH ?= $(shell git rev-parse --abbrev-ref HEAD)
GIT_BRANCH_NORMALISED := $(shell echo "$(GIT_BRANCH)" | sed 's|[^[:alnum:]]|-|g')

IMAGE = ad-posting-api-client
APP_DOCKER_IMAGE = $(IMAGE):$(BUILD_NUMBER)

API_VERSION = 2.0
VERSION = $(API_VERSION).$(BUILD_NUMBER)
PACKAGE_VERSION = $(if $(GIT_BRANCH_NORMALISED:master=),$(VERSION)-$(GIT_BRANCH_NORMALISED),$(VERSION))

##@ Build & Test

clean: ## Cleans the output of previous build
	dotnet clean --configuration Release

test: ## Run tests
	dotnet test --configuration Release test/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj

pact-markdown: ## Generate PACT markdown
	dotnet script ./scripts/generate-pact-markdown.csx

publish-pacts: ## Publish PACTs to PACT broker
	@./scripts/publish-pacts.sh $(PACT_BROKER_URL) $(API_VERSION) $(BUILD_NUMBER)

##@ CI/CD

docker-build: ## Builds and tests API client, generates nuget package and tests nuget package
	docker build --target build --build-arg 'VERSION=$(VERSION)' --build-arg='GIT_BRANCH_NORMALISED=$(GIT_BRANCH_NORMALISED)' --build-arg='PACKAGE_VERSION=$(PACKAGE_VERSION)' -t $(APP_DOCKER_IMAGE) .

help:
	@echo
	@awk 'BEGIN {FS = ":.*##"; printf "\nUsage:\n  make \033[36m<variable>\033[0m \033[36m<target>\033[0m\n\nVariables:\n \033[36m BUILD_NUMBER\033[0m?=0\n \033[36m PACT_BROKER_URL\033[0m?= Required for target \033[36mpublish-pacts\033[0m - example: http://pactbroker \n\nTargets:\n"} /^[a-zA-Z_-]+:.*?##/ { printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)
	@echo

.DEFAULT_GOAL := help

.PHONY: clean test pact-markdown docker-build
