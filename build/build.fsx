#r "packages/FAKE/tools/FakeLib.dll"; open Fake 
#load "build-tools/version.fsx"; open Version
open System.IO
open Fake.VSTest

let outputDir = "../out"
let version = generateVersionNumber "../src/SEEK.AdPostingApi.Client/version.txt"
let solutionDir = "../src/SEEK.AdPostingApi.SampleConsumer.sln"
let testDir = "../src/SEEK.AdPostingApi.SampleConsumer.Tests"
let clientDir = "../src/SEEK.AdPostingApi.Client"
let nugetVersion = generateNugetVersion
let buildMetaDataFile = "../out/build.metadata.json"

let authors = ["SEEK"]
let projectName = "SEEK.AdPostingApi"
let projectDescription = "SEEK.AdPostingApi"
let packagingRoot = "../out/artifacts"
let projectSummary = "SEEK.AdPostingApi"
let myAccesskey = "fake-key"

trace (sprintf "##teamcity[setParameter name='VERSION' value='%s']" version)

Target "Clean" (fun _ ->
    CleanDirs [outputDir]
)

Target "RestorePackages" (fun _ -> 
     solutionDir
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             Sources = "https://www.nuget.org/api/v2/" :: p.Sources
             OutputPath = "../src/packages"
             Retries = 4 })
 )
 
Target "Build" (fun _ ->
    !! solutionDir
      |> MSBuildRelease "" "Build"
      |> Log "AppBuild-Output: "
)


Target "Test" (fun _ ->
   !! (testDir + @"/bin/**/SEEK.AdPostingApi.SampleConsumer.Tests.dll") 
      |> VSTest (fun p -> { p with WorkingDir = testDir })
)

Target "WriteMetaData" (fun _ -> 
    WriteStringToFile false buildMetaDataFile generateMetaData
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
            AccessKey = myAccesskey
            Files = [
                (@"bin\Release\SEEK.AdPostingApi.Client.dll", Some "lib/net45", None)
                (@"bin\Release\SEEK.AdPostingApi.Client.pdb", Some "lib/net45", None)
            ]
            Publish = false }) 
            "../src/SEEK.AdPostingApi.Client/SEEK.AdPostingApi.Client.nuspec"
)

"Clean"
   ==> "RestorePackages"
   ==> "Build"
   ==> "Test"
   ==> "WriteMetaData"
   ==> "NuGet"

RunTargetOrDefault "NuGet"