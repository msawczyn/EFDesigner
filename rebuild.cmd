cd src
rem VSIXInstaller.exe /q /u:"56bbe1ba-aaee-4883-848f-e3c8656f8db2"
msbuild efdesigner.sln /t:Rebuild /p:Configuration=Debug
msbuild efdesigner.sln /t:Rebuild /p:Configuration=Release
copy /Y "DslPackage\bin\Release\Sawczyn.EFDesigner.EFModel.DslPackage.vsix" ..\dist
rem VSIXInstaller.exe /q "DslPackage\bin\Release\Sawczyn.EFDesigner.EFModel.DslPackage.vsix"
