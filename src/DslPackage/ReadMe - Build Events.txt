DSL Package Build Events:

**Note:
  The pre-build event installs dotnet-warp (https://github.com/Hubert-Rybak/dotnet-warp) in order to package
  the dotnetcore parser into a single executable. Once installed it doesn't have to be installed again.


   Pre-Build
      dotnet tool install -g dotnet-warp
      dotnet-warp "$(SolutionDir)EFCoreParser\EFCoreParser.csproj" -o "$(ProjectDir)Parsers\EFCoreParser.exe"
      mkdir "$(TargetDir)Parsers"
      copy /y "$(ProjectDir)Parsers\*.*" "$(TargetDir)Parsers"

   Post-Build
      IF "$(ConfigurationName)"=="Release" copy /y "$(TargetDir)$(TargetName).vsix" "$(SolutionDir)..\dist\$(TargetName).vsix"
