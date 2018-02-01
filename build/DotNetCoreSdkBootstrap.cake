public static class DotNetCoreSdkBootstrap
{
    private const string SdkVersion = "2.1.4";
    private static readonly string WindowSdkUrl = $"https://download.microsoft.com/download/1/1/5/115B762D-2B41-4AF3-9A63-92D9680B9409/dotnet-sdk-2.1.4-win-x64.zip";

    public static void Bootstrap(ICakeContext context)
    {
        FilePath zipPath = context.File($"lib/dotnetsdk/{SdkVersion}/dotnet.zip");
        DirectoryPath extractDirectory = context.Directory($"lib/dotnetsdk/{SdkVersion}");
        FilePath cliBinary = extractDirectory.GetFilePath("dotnet.exe");

        if (!context.FileExists(cliBinary))
        {
            context.CreateDirectory(extractDirectory);
            context.Information($"Downloading .NET Core SDK {SdkVersion}");
            context.DownloadFile(WindowSdkUrl, zipPath);
            context.Information($"Unzipping to {extractDirectory}");
            context.Unzip(zipPath, extractDirectory);
        }
        context.Information($"Using .NET Core SDK from {extractDirectory}");
        context.Tools.RegisterFile(cliBinary);
    }
}