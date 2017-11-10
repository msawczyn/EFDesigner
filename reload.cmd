ECHO OFF
AT > NUL
IF %ERRORLEVEL% EQU 0 (
   ECHO ON
   VSIXInstaller.exe /q /u:"56bbe1ba-aaee-4883-848f-e3c8656f8db2"
   VSIXInstaller.exe /q "dist\Sawczyn.EFDesigner.EFModel.DslPackage.vsix"
) ELSE (
   ECHO Needs administrative access. Exiting...
   PAUSE
)
