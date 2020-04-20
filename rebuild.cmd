cd src
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\VSIXInstaller.exe" /q /u:"56bbe1ba-aaee-4883-848f-e3c8656f8db2"
del "..\dist\Sawczyn.EFDesigner.EFModel.DslPackage.vsix"
REM msbuild efdesigner.sln /t:Rebuild /p:Configuration=Debug
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe" efdesigner.sln /t:Rebuild /p:Configuration=Release
copy /Y "DslPackage\bin\Release\Sawczyn.EFDesigner.EFModel.DslPackage.vsix" ..\dist
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\VSIXInstaller.exe" /q "..\dist\Sawczyn.EFDesigner.EFModel.DslPackage.vsix"
cd ..
