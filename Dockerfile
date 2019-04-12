FROM microsoft/dotnet:2.1-sdk AS restore
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj ./src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj
COPY test/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj ./test/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj
COPY sample/SEEK.AdPostingApi.SampleConsumer.csproj ./sample/SEEK.AdPostingApi.SampleConsumer.csproj
RUN dotnet restore

# build and test the source
FROM restore AS build
ARG VERSION
ARG PACKAGE_VERSION
ARG GIT_BRANCH_NORMALISED
COPY . .
RUN ./scripts/generate-assembly-info.sh $VERSION $GIT_BRANCH_NORMALISED
RUN dotnet build ./SEEK.AdPostingApi.Client.sln --configuration Release --no-restore
RUN dotnet test ./test/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj --configuration Release --no-restore --no-build 
RUN dotnet pack ./src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj  --configuration Release --no-restore --no-build --output /app/out/artifacts -p:PackageVersion=$PACKAGE_VERSION

# test nuget package
FROM build as package-test
ARG PACKAGE_VERSION
RUN rm -rf logs/ pact/ sample/bin/ sample/obj/ src/ test/ scripts/ SEEK.AdPostingApi.Client.sln
RUN dotnet remove sample/SEEK.AdPostingApi.SampleConsumer.csproj reference ../src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj
RUN dotnet add sample/SEEK.AdPostingApi.SampleConsumer.csproj package SEEK.AdPostingApi.Client --version $PACKAGE_VERSION --source /app/out/artifacts/
RUN dotnet build sample/SEEK.AdPostingApi.SampleConsumer.csproj --configuration Release --no-restore
