public class BuildVersion
{
    public string Major { get; private set; }
    public string Minor { get; private set; }
    public string BuildNumber { get; private set; }
    public string BranchName { get; private set; }
    public string GitSha { get; private set; }
    public string Date { get; private set; }

    public string FileVersion => $"{Major}.{Minor}.{BuildNumber}";
    public string PactVersion => $"{Major}.{Minor}.{Date}.{BuildNumber}";

    public string PackageVersion
    {
        get
        {
            if(BranchName == "master")
            {
                return $"{Major}.{Minor}.{BuildNumber}";
            }
            return $"{Major}.{Minor}.{BuildNumber}-{BranchName}";
        }
    }

    public static BuildVersion CalculateVersion(ICakeContext context)
    {
        string buildNumber = "0";
        string branchName = "";
        string gitSha = "";

        // TeamCity is setup for server-side checkouts, thus it's not a real git repo
        if (context.TeamCity().IsRunningOnTeamCity)
        {
            buildNumber = context.TeamCity().Environment.Build.Number;
            branchName = context.EnvironmentVariable("SEEK_TEAMCITY_VCSROOT_BRANCH");
            gitSha = context.EnvironmentVariable("BUILD_VCS_NUMBER").Substring(0, 6);
        }
        else
        {
            GitBranch currentBranch = context.GitBranchCurrent(DirectoryPath.FromString(".."));
            branchName = currentBranch.FriendlyName;
            gitSha = currentBranch.Tip.Sha.Substring(0, 6);
        }
        return new BuildVersion
        {
            Major = "1",
            Minor = "0",
            BuildNumber = buildNumber,
            BranchName = branchName.Replace('/', '-'),
            GitSha = gitSha,
            Date = DateTime.Now.ToString("yyMMdd")
        };
    }
}