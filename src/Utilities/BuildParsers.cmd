copy /y "C:\Code\EFDesigner\src\Dsl\GeneratedCode\EFModelSchema.xsd" "C:\Code\EFDesigner\src\DslPackage\ProjectItemTemplates\EFModel.xsd"

dotnet tool update -g dotnet-warp

rem =================================
set EFVER=EF6
set NETVER=net472
call :framework

set EFVER=EF6
set NETVER=netcoreapp3.1
call :netcore3

rem =================================
set EFVER=EFCore2
set NETVER=netcoreapp2.2
call :netcore2

set EFVER=EFCore2
set NETVER=netcoreapp3.1
call :netcore3

rem =================================
set EFVER=EFCore3
set NETVER=net472
call :framework

set EFVER=EFCore3
set NETVER=netcoreapp3.1
call :netcore3

if "Release"=="Release" del "C:\Code\EFDesigner\src\DslPackage\Parsers\*.pdb

exit /b

:framework
"%UserProfile%\.dotnet\tools\.store\dotnet-warp\1.1.0\dotnet-warp\1.1.0\tools\netcoreapp2.1\any\warp\windows-x64.warp-packer.exe" --arch windows-x64 --input_dir "C:\Code\EFDesigner\src\Utilities\%EFVER%Parser\bin\Release\%NETVER%\win-x64" --exec %EFVER%Parser.exe --output C:\Code\EFDesigner\src\DslPackage\Parsers\%EFVER%_%NETVER%_Parser.exe
exit /b

:netcore2
cd "C:\Code\EFDesigner\src\Utilities\%EFVER%Parser"
dotnet-warp "C:\Code\EFDesigner\src\Utilities\%EFVER%Parser\%EFVER%Parser.csproj" -p:TargetFramework=%NETVER% -o C:\Code\EFDesigner\src\DslPackage\Parsers\%EFVER%_%NETVER%_Parser.exe -v
exit /b

:netcore3
cd "C:\Code\EFDesigner\src\Utilities\%EFVER%Parser"
dotnet publish -c Release -r win-x64 -f %NETVER% --self-contained -o C:\Code\EFDesigner\src\DslPackage\Parsers /p:PublishSingleFile=true /p:PublishTrimmed=true
del "C:\Code\EFDesigner\src\DslPackage\Parsers\%EFVER%_%NETVER%_Parser.exe"
rename "C:\Code\EFDesigner\src\DslPackage\Parsers\%EFVER%Parser.exe" "%EFVER%_%NETVER%_Parser.exe"
exit /b

