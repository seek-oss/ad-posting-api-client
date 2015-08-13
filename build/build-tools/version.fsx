#r "../packages/FAKE/tools/FakeLib.dll";
#r "System.Runtime.Serialization"

open Fake 
open Fake.Git
open Fake.EnvironmentHelper
open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open System
open System.IO
open System.Text

[<DataContract>]
type Version = {
        [<field: DataMember(Name = "major")>]
        major:string
        [<field: DataMember(Name = "minor")>]
        minor:string
    }

type BuildVars = { branch: string; gitHash: string }

let private deserialiseVersion (s:string) =
    let json = new DataContractJsonSerializer(typeof<Version>)
    let byteArray = Encoding.UTF8.GetBytes(s)
    let stream = new MemoryStream(byteArray)
    json.ReadObject(stream) :?> Version


let private getBuildVarsLocal _ =
    let isGitRepository = Directory.Exists("../.git")
    let gitInstalled = match tryFindFileOnPath (if isUnix then "git" else "git.exe") with
                                    | Some(a) -> true
                                    | None -> false

    let gitHash = if isGitRepository && gitInstalled then Information.getCurrentSHA1(".").[0..6] else "xxxxxxx"
    let branch = if isGitRepository && gitInstalled then Information.getBranchName(".") else "unknown"

    { branch = branch; gitHash = gitHash }

let private removeBranchPrefix (branchName:string) =
    let slashIndex = branchName.LastIndexOf('/')

    if slashIndex = -1 then branchName else branchName.Substring(slashIndex + 1)

let private getBuildVarsTeamcity _ =
    if environVar "SEEK_TEAMCITY_VCSROOT_URL" = null || environVar "SEEK_TEAMCITY_VCSROOT_BRANCH" = null then failwith "One of the required variables (SEEK_TEAMCITY_VCSROOT_URL, SEEK_TEAMCITY_VCSROOT_BRANCH) is not defined. Did you forget to attach 'Template: Build | Upload v2' template? There is a jive page containing how to migrate builds. https://seek.jiveon.com/docs/DOC-9633"

    let gitHash = (environVar "BUILD_VCS_NUMBER").[0..6]
    let branch = removeBranchPrefix (environVar "SEEK_TEAMCITY_VCSROOT_BRANCH")

    { branch = branch; gitHash = gitHash }

let generateVersionNumber path =
    let buildVars = if System.String.IsNullOrEmpty(environVar "TEAMCITY_VERSION") then getBuildVarsLocal() else getBuildVarsTeamcity()
    let buildNumber = environVarOrDefault "BUILD_NUMBER" "0"
    let version = deserialiseVersion(File.ReadAllText("./version.json"))
    let date = DateTime.Now.ToString("yyMMdd")
    let version = [ buildVars.branch; version.major; version.minor; date; buildNumber; buildVars.gitHash] |> String.concat "."

    File.WriteAllText(path, version)

    version

let generateNugetVersion =
    let buildNumber = environVarOrDefault "BUILD_NUMBER" "0"
    let version = deserialiseVersion(File.ReadAllText("./version.json"))
    let date = DateTime.Now.ToString("yyMMdd")
    let nugetVersion = [ version.major; version.minor; date; buildNumber] |> String.concat "."
    
    nugetVersion

let generateMetaData =
    let buildNumber = environVarOrDefault "BUILD_NUMBER" "0"
    let version = generateNugetVersion
    let date = DateTime.Now.ToString("yyMMdd")
    let buildMetaData = @"{buildNumber: """ + buildNumber + @""", timestamp: """ + date + @""", nugetVersion:  """ + version + @""" }"

    buildMetaData
