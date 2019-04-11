##@ Build & Test

clean: ## Cleans the output of previous build
	dotnet clean --configuration Release

test: ## Run tests
	dotnet test --configuration Release test/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj

pact-markdown: ## Generate PACT markdown
	dotnet script ./scripts/generate-pact-markdown.csx

##@ Helpers

help:
	@echo
	@awk 'BEGIN {FS = ":.*##"; printf "\nUsage:\n  make \033[36m<target>\033[0m\n\nTargets:\n"} /^[a-zA-Z_-]+:.*?##/ { printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)
	@echo

.DEFAULT_GOAL := help

.PHONY: clean test pact-markdown
