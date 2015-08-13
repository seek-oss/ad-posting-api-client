docker run \
    --rm \
    -v $(which docker):/usr/local/bin/docker \
    -v /var/run/docker.sock:/var/run/docker.sock \
    -v $(pwd):/var/app \
    -e "SEEK_TEAMCITY_VCSROOT_URL=$SEEK_TEAMCITY_VCSROOT_URL" -e "SEEK_TEAMCITY_VCSROOT_BRANCH=$SEEK_TEAMCITY_VCSROOT_BRANCH" -e "BUILD_VCS_NUMBER=$BUILD_VCS_NUMBER" -e "TEAMCITY_VERSION=$TEAMCITY_VERSION" -e "BUILD_NUMBER=$BUILD_NUMBER" \
    dockerregistry.seekinfra.com/advertiser/adpostingapi_buildcontainer /var/app/build/build.sh "$@"
