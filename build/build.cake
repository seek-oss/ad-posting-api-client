#addin nuget:?package=Cake.Git&version=0.16.1
#addin nuget:?package=Newtonsoft.Json&version=10.0.3
#addin nuget:?package=PactNet&version=2.2.1
#load "./DotNetCoreSdkBootstrap.cake"
#load "./BuildVersion.cake"
#load "./Pact.cake"

const string SolutionPath = "../SEEK.AdPostingApi.Client.sln";
const string OutputDir = "../out";
const string PactDir = "../pact";
const string PackagingRoot = "../out/artifacts";

string target = Argument("target", "Help");
string configuration = Argument("configuration", "Release");
string pactBrokerUrl = EnvironmentVariable("PACT_BROKER_URL");
string pactBrokerUsername = EnvironmentVariable("PACT_BROKER_USERNAME");
string pactBrokerPassword = EnvironmentVariable("PACT_BROKER_PASSWORD");

BuildVersion version;

Setup(context => {
    version = BuildVersion.CalculateVersion(context);
    Information("Building version {0} ({1} Configuration)", version.PackageVersion, configuration);
    DotNetCoreSdkBootstrap.Bootstrap(context);
});

TaskSetup(setupContext => {
    if (TeamCity.IsRunningOnTeamCity)
    {
        TeamCity.WriteStartBlock(setupContext.Task.Name);
    }
});

TaskTeardown(teardownContext => {
    if (TeamCity.IsRunningOnTeamCity)
    {
        TeamCity.WriteEndBlock(teardownContext.Task.Name);
    }
});

Task("Clean")
.Does(() => {
    DotNetCoreClean(SolutionPath);
    CleanDirectory(Directory(OutputDir));
});

Task("Restore")
.IsDependentOn("Clean")
.Does(() => {
    DotNetCoreRestore(SolutionPath);
});

Task("GenerateVersion")
.Does(() => {
    if (TeamCity.IsRunningOnTeamCity)
    {
        TeamCity.SetBuildNumber(version.PackageVersion);
    }
    CreateAssemblyInfo("../src/SEEK.AdPostingApi.Client/Properties/AssemblyInfoGenerated.cs", new AssemblyInfoSettings {
        Version = version.FileVersion,
        FileVersion = version.FileVersion,
        InformationalVersion = version.PackageVersion
    });
});

Task("Build")
.IsDependentOn("Restore")
.IsDependentOn("GenerateVersion")
.Does(() => {
    var buildSettings = new DotNetCoreBuildSettings {
        Configuration = configuration
    };
    DotNetCoreBuild(SolutionPath);
});

Task("Test")
.IsDependentOn("Build")
.Does(() => {
    var testSettings = new DotNetCoreTestSettings {
        Configuration = configuration
    };
    DotNetCoreTest("../test/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj", testSettings);
});

Task("NuGet")
.IsDependentOn("Test")
.Does(() => {
    CreateDirectory(Directory(PackagingRoot));
    var settings = new DotNetCorePackSettings {
         Configuration = configuration,
         OutputDirectory = PackagingRoot,
         MSBuildSettings = new DotNetCoreMSBuildSettings {
             Properties = {
                 { "Version", new[] { version.PackageVersion } }
             }
         }
    };
    DotNetCorePack("../src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj", settings);
});

Task("NuGetTest")
.Does(() => {
    const string sampleProjectPath = "../sample/SEEK.AdPostingApi.SampleConsumer.csproj";
    const string clientProjectPath = "../src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.csproj";
    string packageVersion = Argument<string>("nuGetTestPackageVersion");
    string packageSource = Argument<string>("nuGetTestPackageSource");

    DotNetCoreTool(sampleProjectPath, "remove",
        new ProcessArgumentBuilder()
            .Append(sampleProjectPath)
            .Append("reference")
            .Append(clientProjectPath));

    DotNetCoreTool(sampleProjectPath, "add",
        new ProcessArgumentBuilder()
            .Append(sampleProjectPath)
            .Append("package")
            .Append("--version")
            .Append(packageVersion)
            .Append("--no-restore")
            .Append("SEEK.AdPostingApi.Client"));

    DotNetCoreTool(sampleProjectPath, "restore",
        new ProcessArgumentBuilder()
            .Append(sampleProjectPath)
            .Append("--no-cache")
            .Append("--source")
            .Append(packageSource)
            .Append("--source")
            .Append("https://www.nuget.org/"));

    var buildSettings = new DotNetCoreBuildSettings {
        Configuration = configuration,
        NoRestore = true
    };
    DotNetCoreBuild(sampleProjectPath);
});

Task("PactMarkdown")
.Does(() => {
    // PactNet does not have support for generating Markdown from PACT files
    // Doing it manually avoids a build dependency on Ruby
    void GenerateMarkdown(FilePath pactJsonPath, FilePath destinationMarkdownPath)
    {
        PactFile pactFile = PactFile.FromFile(pactJsonPath.ToString());
        var generator = new PactMarkdownGenerator(pactFile);
        generator.WriteFile(destinationMarkdownPath.ToString());
    }

    GenerateMarkdown(File($"{PactDir}/ad_posting_api_client-ad_posting_api.json"), File($"{PactDir}/README.md"));

    DirectoryPath markdownOutputPath = Directory($"{OutputDir}/markdown");
    CreateDirectory(markdownOutputPath);
    CopyFiles($"{PactDir}/*.md", markdownOutputPath);
});

Task("UploadPact")
.IsDependentOn("NuGet")
.Does(() => {
    PactNet.PactUriOptions options = null;
    if (!string.IsNullOrEmpty(pactBrokerUsername))
    {
        options = new PactNet.PactUriOptions(pactBrokerUsername, pactBrokerPassword);
        Information($"Using {pactBrokerUsername} to authenticate against the PACT broker");
    }

    Information($"Publishing PACTs with version {version.PactVersion} and branch {version.BranchName} to broker {pactBrokerUrl}");

    var pactPublisher = new PactNet.PactPublisher(pactBrokerUrl, options);
    FilePathCollection pactFilePaths = GetFiles($"{PactDir}/*.json");
    foreach (FilePath pactFilePath in pactFilePaths)
    {
        Information($"Publishing '{pactFilePath.ToString()}'");
        pactPublisher.PublishToBroker(pactFilePath.ToString(), version.PactVersion, new[] { version.BranchName });
    }
});

Task("CI")
.IsDependentOn("UploadPact")
.IsDependentOn("PactMarkdown");

Task("Help")
.Does(() => {
    Information(@"
        Please specify the target by calling 'build -Target <Target>'
        * Help                       - Display this help
        * Build                      - Build
        * Test                       - Build and run all tests
        * NuGet                      - Build, run all tests, and generate a NuGet package
        * NuGetTest                  - Modify the sample client to use a specific NuGet package and build it
        * PactMarkdown               - Generate Markdown of the PACTs in pact/ 
        * UploadPact                 - Build, run all tests, generate a NuGet package, and publish the PACTs to the broker
        * CI                         - The target used by continuous integration, builds, runs tests, generates a NuGet package, generates PACT documentation and publishes PACTs
    ");
});

Task("Default").IsDependentOn("Help");

RunTarget(target);
