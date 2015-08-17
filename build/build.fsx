#r "packages/FAKE/tools/FakeLib.dll"; open Fake 
#load "build-tools/version.fsx"; open Version
#load "build-tools/pact.fsx"; open Pact
open System.IO
open Fake.VSTest

let outputDir = "../out"
let srcDir = "../src"
let version = generateVersionNumber "../src/SEEK.AdPostingApi.Client/version.txt"
let solutionDir = srcDir + "/SEEK.AdPostingApi.SampleConsumer.sln"
let testDir = srcDir + "/SEEK.AdPostingApi.SampleConsumer.Tests"
let clientDir = srcDir + "/SEEK.AdPostingApi.Client"
let nugetVersion = generateNugetVersion

let authors = ["SEEK"]
let projectName = "SEEK.AdPostingApi.Client"
let projectDescription = "SEEK.AdPostingApi.Client"
let packagingRoot = outputDir + "/artifacts"
let projectSummary = "SEEK.AdPostingApi.Client"

trace (sprintf "##teamcity[setParameter name='VERSION' value='%s']" (version |> String.concat "."))

Target "Clean" (fun _ ->
    CleanDirs [outputDir; testDir + @"/bin/"]
)

Target "RestorePackages" (fun _ -> 
     solutionDir
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             Sources = "https://www.nuget.org/api/v2/" :: p.Sources
             OutputPath = srcDir + "/packages"
             Retries = 4 })
 )
 
Target "Build" (fun _ ->
    !! solutionDir
      |> MSBuildRelease "" "Build"
      |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
   !! (testDir + @"/bin/**/SEEK.AdPostingApi.SampleConsumer.Tests.dll") 
      |> NUnit (fun p -> { p with Domain = NUnitDomainModel.NoDomainModel
                                  ToolPath = "../src/packages/NUnit.Runners.2.6.4/tools" })
)

Target "UploadPact" (fun _ ->
   (!! "../**/pacts/*.json") |> PublishPact version
)

Target "NuGet" (fun _ ->
    CreateDir packagingRoot

    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName
            Description = projectDescription                               
            OutputPath = packagingRoot
            Summary = projectSummary
            WorkingDir = clientDir
            Version = nugetVersion
            Files = [
                (@"bin/Release/SEEK.AdPostingApi.Client.dll", Some "lib/net45", None)
            ]
            Publish = false }) 
            (srcDir + "/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.nuspec")
)

"Clean"
   ==> "RestorePackages"
   ==> "Build"
   ==> "Test"
   ==> "NuGet"
   ==> "UploadPact"

RunTargetOrDefault "NuGet"
