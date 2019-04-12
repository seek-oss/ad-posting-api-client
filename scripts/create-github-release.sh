#!/usr/bin/env bash

set -eou pipefail

remoteName=${1?param missing - git remote name e.g. origin}
githubOrg=${2?param missing - github organisation e.g. seek-oss}
githubRepo=${3?param missing - github repo e.g. ad-posting-api-client}
version=${4?param missing - version e.g. \'1.2.3\' or \'1.2.4-pre-release\'}
nonInteractive=${5:-false}

branchName=$(git rev-parse --abbrev-ref HEAD)

function exitWithError {
  printf "\e[1;31mERROR: %s\e[0m\n" "${1}"
  exit 1
}

function prompt {
  printf "\e[1;33m%s\e[1;34m\n" "${1}"
  if [[ "${nonInteractive}" == "false" ]]; then
    read -n1 -rs -p 'Press <y> to continue OR any other key to abort...' key
    printf "\e[0m\n"
    if [[ "${key}" != 'y' ]]; then
      exitWithError "Aborting"
    fi
  else
    printf "%s\e[0m\n" "Non-interactive - continuing"
  fi
}

if [[ -z "${branchName}" ]]; then
  exitWithError "branchName is required"
fi

if [[ "${branchName}" != 'master' ]]; then
  prompt "Branch is not 'master' - Are you sure you want to create a release?"
fi

if [[ ! -f 'ReleaseNotes.md' ]]; then
  exitWithError "'ReleaseNotes.md' not found - please run script in repo root."
fi

if [[ -z "$(git remote | grep ^${remoteName}$)" ]]; then
  exitWithError "Git remote '${remoteName}' does not exist"
fi

if [[ ! -z $(git remote -v | grep -iE -m1 "^${remoteName}\\s+.+[/:]${githubOrg}/${githubRepo}.git\\s+(push)") ]]; then
  exitWithError "Git remote '${remoteName}' does not match organisation '${githubOrg}' and repo '${githubRepo}'"
fi

if [[ -z $(git status -sb | grep -E "^##\\s+${branchName}\\.{3}${remoteName}/${branchName}\\s*") ]]; then
  exitWithError "Git branch '${branchName}' is not tracking remote branch '${remoteName}/${branchName}'"
fi

prompt "Have you updated, committed and pushed 'ReleaseNotes.md' to '${branchName}' branch?"

printf "\e[1;37m%s\e[0m\n" "Pulling '${branchName}' branch"
git pull ${remoteName} ${branchName}

if [[ -z $(git tag | grep -E "^${version}$") ]]; then
  printf "\e[1;37m%s\e[0m\n" "Tagging as ${version} and annotating with 'ReleaseNotes.md'"
  git tag -a "${version}" -F "ReleaseNotes.md"
else
  prompt "Warning: Tag ${version} already exists"
fi

printf "\e[1;37m%s\e[0m\n" "Pushing to '${branchName}' branch including tags"
git push --follow-tags ${remoteName} ${branchName}
