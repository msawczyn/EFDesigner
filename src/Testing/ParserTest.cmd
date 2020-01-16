@echo off 
setlocal EnableDelayedExpansion 

set parser[0]=..\DslPackage\Parsers\net472\EF6Parser.exe
set parser[1]=..\DslPackage\Parsers\net472\EFCore2Parser.exe
set parser[2]=..\DslPackage\Parsers\net472\EFCore3Parser.exe
set parser[3]=..\DslPackage\Parsers\netcoreapp2.1\EFCore2Parser.exe
set parser[4]=..\DslPackage\Parsers\netcoreapp2.1\EFCore3Parser.exe
set parser[5]=..\DslPackage\Parsers\netcoreapp3.1\EF6Parser.exe
set parser[6]=..\DslPackage\Parsers\netcoreapp3.1\EFCore2Parser.exe
set parser[7]=..\DslPackage\Parsers\netcoreapp3.1\EFCore3Parser.exe

set input[0]=.\EF6\EF6NetCore3\bin\Debug\netcoreapp3.1\EF6NetCore3.dll
set input[1]=.\EF6\EF6NetFramework\bin\Debug\EF6NetFramework.dll
set input[2]=.\EF6\EF6NetStandard\bin\Debug\netstandard2.1\EF6NetStandard.dll
set input[3]=.\EFCoreV2\EFCore2NetCore2\bin\Debug\netcoreapp2.2\EFCore2NetCore2.dll
set input[4]=.\EFCoreV2\EFCore2NetCore3\bin\Debug\netcoreapp3.1\EFCore2NetCore3.dll
set input[5]=.\EFCoreV2\EFCore2NetFramework\bin\Debug\EFCore2NetFramework.dll
set input[6]=.\EFCoreV2\EFCore2NetStandard\bin\Debug\netstandard2.1\EFCore2NetStandard.dll
set input[7]=.\EFCoreV3\EFCore3NetCore2\bin\Debug\netcoreapp2.2\EFCore3NetCore2.dll
set input[8]=.\EFCoreV3\EFCore3NetCore3\bin\Debug\netcoreapp3.1\EFCore3NetCore3.dll
set input[9]=.\EFCoreV3\EFCore3NetFramework\bin\Debug\EFCore3NetFramework.dll
set input[10]=.\EFCoreV3\EFCore3NetStandard\bin\Debug\netstandard2.1\EFCore3NetStandard.dll

for %%p in (0,1,2,3,4,5,6,7) do (
   for %%i in (0,1,2,3,4,5,6,7,8,9,10) do (
      "!parser[%%p]!" "!input[%%i]!" parsertest.txt
      echo %ERRORLEVEL% - "!parser[%%p]!" "!input[%%i]!" 

      REM echo !parser[%%p]!
      REM echo !input[%%i]!
   )
)
