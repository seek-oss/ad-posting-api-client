#addin nuget:?package=Cake.Git&version=0.16.1
#addin nuget:?package=Newtonsoft.Json&version=10.0.3
#addin nuget:?package=PactNet&version=2.2.1
#load "./DotNetCoreSdkBootstrap.cake"
#load "./BuildVersion.cake"
#load "./Pact.cake"

const string SolutionPath = "../src/SEEK.AdPostingApi.Client.sln";
const string OutputDir = "../out";
const string PactDir = "../pact";
const string PackagingRoot = "../out/artifacts";

string target = Argument("target", "Help");
string configuration = Argument("configuration", "Release");
string pactBrokerUrl = EnvironmentVariable("PACT_BROKER_URL");
string pactBrokerUsername = EnvironmentVariable("PACT_BROKER_USERNAME");
string pactBrokerPassword = EnvironmentVariable("PACT_BROKER_PASSWORD");
string pactCommitEmail = EnvironmentVariable("PACT_COMMIT_EMAIL");

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
    DotNetCoreTest("../src/SEEK.AdPostingApi.Client.Tests/SEEK.AdPostingApi.Client.Tests.csproj", testSettings);
});

Task("NuGet")
.IsDependentOn("Test")
.Does(() => {
    var nuGetPackSettings = new NuGetPackSettings {
        Id = "SEEK.AdPostingApi.Client",
        Version = version.PackageVersion,
        Title = "SEEK.AdPostingApi.Client",
        Authors = new[] {"SEEK"},
        Owners = new[] {"SEEK"},
        Description = "SEEK.AdPostingApi.Client",
        Summary = "SEEK.AdPostingApi.Client",
        Copyright = "Copyright 2018",
        RequireLicenseAcceptance = false,
        Symbols = false,
        NoPackageAnalysis = true,
        Dependencies = new[] {
            new NuSpecDependency {
                Id = "Marvin.JsonPatch",
                Version = "0.9.0"
            },
            new NuSpecDependency {
                Id = "Newtonsoft.Json",
                Version = "10.0.3"
            },
            new NuSpecDependency {
                Id = "Tavis.UriTemplates",
                Version = "1.0.0"
            }
        },
        Files = new [] {
            new NuSpecContent {
                Source = $"bin/{configuration}/net452/SEEK.AdPostingApi.Client.dll",
                Target = "lib/net452"
            }
        },
        BasePath = "../src/SEEK.AdPostingApi.Client",
        OutputDirectory = PackagingRoot
    };
    CreateDirectory(Directory(PackagingRoot));
    NuGetPack(nuGetPackSettings);
});

Task("PactMarkdown")
.IsDependentOn("Test")
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
.IsDependentOn("Test")
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

Task("CommitPact")
.IsDependentOn("UploadPact")
.IsDependentOn("PactMarkdown")
.Does(() => {
    // Cake.Git doesn't support SSH, so use command-line git for now
    FilePath gitPath = Context.Tools.Resolve("git.exe");
    if (gitPath == null)
    {
        throw new Exception("Failed to find git.exe, is it on the PATH?");
    }

    int addResult = StartProcess(gitPath, new ProcessSettings {
        Arguments = new ProcessArgumentBuilder()
            .Append("add")
            .Append("--force")
            .Append(MakeAbsolute(Directory(PactDir)).ToString())
    });
    if (addResult != 0)
    {
        throw new Exception($"Failed to stage PACT files, git returned {addResult}");
    }

    int commitResult = StartProcess(gitPath, new ProcessSettings {
        Arguments = new ProcessArgumentBuilder()
            .Append("commit")
            .Append($"--author=\"Build Agent <{pactCommitEmail}>\"")
            .Append("--message=\"Update PACTs\"")
            .Append(MakeAbsolute(Directory(PactDir)).ToString())
    });
    if (commitResult != 0)
    {
        throw new Exception($"Failed to commit PACT files, git returned {commitResult}");
    }

    int pushResult = StartProcess(gitPath, new ProcessSettings {
        Arguments = new ProcessArgumentBuilder()
            .Append("push")
            .Append("origin")
            .Append("HEAD")
    });
    if (pushResult != 0)
    {
        throw new Exception($"Failed to push PACT commit, git returned {pushResult}");
    }
});

Task("Help")
.Does(() => {
    Information(@"
        Please specify the target by calling 'build -Target <Target>'
        * Help                       - Display this help
        * Build                      - Build
        * Test                       - Build and run all tests
        * NuGet                      - Build, run all tests, and generate a NuGet package
        * PactMarkdown               - Generate a human readable Markdown representation of the PACTs
        * UploadPact                 - Build, run all tests, and publish the PACTs to the broker
        * CommitPact                 - Build, run all tests, publish the PACTs to the broker, and commit the PACTs to git
    ");
});

Task("Default").IsDependentOn("Help");

RunTarget(target);
