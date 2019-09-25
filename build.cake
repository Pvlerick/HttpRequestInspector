#tool "nuget:?package=GitVersion.CommandLine&version=5.0.0"

var target = Argument("target", "Default");

Task("Build")
  .DoesForEach(GetFiles("src/**/*.*proj"), file =>
  {
    DotNetCoreClean(file.FullPath);
    DotNetCoreRestore(file.FullPath);
    DotNetCoreBuild(file.FullPath, new DotNetCoreBuildSettings
    {
      Configuration = "Release"
    });
  });

Task("Pack")
  .DoesForEach(GetFiles("src/**/*.*proj"), file =>
  {
    DotNetCorePack(
      file.FullPath, 
      new DotNetCorePackSettings()
      {
        Configuration = "Release",
        ArgumentCustomization = args => args.Append("/p:Version=" + GitVersion().NuGetVersion)
      });
  });

Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Pack");

RunTarget(target);
