@echo off 
cls
setlocal EnableDelayedExpansion 
set initialdir=!cd!

REM ================================================================
set srcRoot=C:\Code\EFDesigner\src
set inputRoot=C:\Code\EFDesigner\src\testing

set package[0]=!srcRoot!\DslPackage\Parsers\EF6_net472_Parser.exe
set package[1]=!srcRoot!\DslPackage\Parsers\EF6_netcoreapp3.1_Parser.exe
set package[2]=!srcRoot!\DslPackage\Parsers\EFCore2_netcoreapp2.2_Parser.exe
set package[3]=!srcRoot!\DslPackage\Parsers\EFCore2_netcoreapp3.1_Parser.exe
set package[4]=!srcRoot!\DslPackage\Parsers\EFCore3_net472_Parser.exe
set package[5]=!srcRoot!\DslPackage\Parsers\EFCore3_netcoreapp3.1_Parser.exe

set input[0]=!inputRoot!\EF6\EF6NetCore3\bin\Debug\netcoreapp3.1\EF6NetCore3.dll
set input[1]=!inputRoot!\EF6\EF6NetFramework\bin\Debug\EF6NetFramework.dll
rem set input[2]=!inputRoot!\EF6\EF6NetStandard\bin\Debug\netstandard2.1\EF6NetStandard.dll
set input[3]=!inputRoot!\EFCoreV2\EFCore2NetCore2\bin\Debug\netcoreapp2.2\EFCore2NetCore2.dll
set input[4]=!inputRoot!\EFCoreV2\EFCore2NetCore3\bin\Debug\netcoreapp3.1\EFCore2NetCore3.dll
set input[5]=!inputRoot!\EFCoreV2\EFCore2NetFramework\bin\Debug\EFCore2NetFramework.dll
rem set input[6]=!inputRoot!\EFCoreV2\EFCore2NetStandard\bin\Debug\netstandard2.1\EFCore2NetStandard.dll
set input[7]=!inputRoot!\EFCoreV3\EFCore3NetCore2\bin\Debug\netcoreapp2.2\EFCore3NetCore2.dll
set input[8]=!inputRoot!\EFCoreV3\EFCore3NetCore3\bin\Debug\netcoreapp3.1\EFCore3NetCore3.dll
set input[9]=!inputRoot!\EFCoreV3\EFCore3NetFramework\bin\Debug\EFCore3NetFramework.dll
rem set input[10]=!inputRoot!\EFCoreV3\EFCore3NetStandard\bin\Debug\netstandard2.1\EFCore3NetStandard.dll

if "%1"=="script" goto script

for %%p in (0,1,2,3,4,5) do (
   echo\
   echo [%%p] !package[%%p]!

   for %%i in (0,1,3,4,5,7,8,9) do (
      "!package[%%p]!" "!input[%%i]!" !temp!\parsertest.json
      echo       !ERRORLEVEL! -  [%%i] !input[%%i]!
   )
)

echo\
exit /b

:script

for %%p in (0,1,2,3,4,5) (
   for %%i in (0,1,3,4,5,7,8,9) do (
      echo "!package[%%p]!" "!input[%%i]!" !temp!\parsertest.json
   )
)

exit /b


