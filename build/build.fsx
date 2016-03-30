#r "packages/FAKE/tools/FakeLib.dll"; open Fake 
#load "build-tools/version.fsx"; open Version
#load "build-tools/pact.fsx"; open Pact
open System.IO
open Fake.VSTest

let nugetPath = findNuget (".." @@ ".nuget")
let branchName = getBuildParamOrDefault "branch" "master"
let outputDir = "../out"
let srcDir = "../src"
let version = generateVersionNumber "../src/SEEK.AdPostingApi.Client/version.txt"
let solutionDir = srcDir + "/SEEK.AdPostingApi.Client.sln"
let testDir = srcDir + "/SEEK.AdPostingApi.Client.Tests"
let clientDir = srcDir + "/SEEK.AdPostingApi.Client"
let nugetVersion = generateNugetVersion
let packagingRoot = outputDir + "/artifacts"

trace (sprintf "branch name set to " + branchName)
trace (sprintf "##teamcity[setParameter name='VERSION' value='%s']" (version |> String.concat "."))

Target "Clean" (fun _ ->
    CleanDirs [outputDir; testDir + @"/bin/"]
)

Target "RestorePackages" (fun _ -> 
     solutionDir
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             ToolPath = nugetPath
             OutputPath = srcDir + "/packages"
             Retries = 4 })
)
 
Target "Build" (fun _ ->
    !! solutionDir
      |> MSBuildRelease "" "Build"
      |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
   !! (testDir + "/bin/**/*.Tests.dll")
      |> VSTest (fun p -> { p with TestAdapterPath = "../src/packages/xunit.runner.visualstudio.2.1.0/build/_common/" })
)

Target "UploadPact" (fun _ ->
   (!! "../**/pact/*.json") |> PublishPact (version, branchName)
)

Target "NuGet" (fun _ ->
    CreateDir packagingRoot

    NuGet (fun p -> 
        {p with
            ToolPath = nugetPath
            OutputPath = packagingRoot
            WorkingDir = clientDir
            Version = nugetVersion
            Files = [ (@"bin/Release/SEEK.AdPostingApi.Client.dll", Some "lib/net452", None) ]
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
