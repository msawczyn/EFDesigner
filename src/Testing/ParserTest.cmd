@echo off 
cls
setlocal EnableDelayedExpansion 
set initialdir=!cd!

REM ================================================================
set pgm[0]=EF6Parser.exe
set pgm[1]=EF6Parser.exe
set pgm[2]=EFCore2Parser.exe
set pgm[3]=EFCore2Parser.exe
set pgm[4]=EFCore2Parser.exe
set pgm[5]=EFCore3Parser.exe
set pgm[6]=EFCore3Parser.exe
set pgm[7]=EFCore3Parser.exe

set package[0]=..\DslPackage\Parsers\net472
set package[1]=..\DslPackage\Parsers\netcoreapp3.1
set package[2]=..\DslPackage\Parsers\net472
set package[3]=..\DslPackage\Parsers\netcoreapp2.2
set package[4]=..\DslPackage\Parsers\netcoreapp3.1
set package[5]=..\DslPackage\Parsers\net472
set package[6]=..\DslPackage\Parsers\netcoreapp2.2
set package[7]=..\DslPackage\Parsers\netcoreapp3.1

set publish[0]=..\Utilities\EF6Parser\bin\Debug\net472\publish
set publish[1]=..\Utilities\EF6Parser\bin\Debug\netcoreapp3.1\publish
set publish[2]=..\Utilities\EFCore2Parser\bin\Debug\net472\publish
set publish[3]=..\Utilities\EFCore2Parser\bin\Debug\netcoreapp2.2\publish
set publish[4]=..\Utilities\EFCore2Parser\bin\Debug\netcoreapp3.1\publish
set publish[5]=..\Utilities\EFCore3Parser\bin\Debug\net472\publish
set publish[6]=..\Utilities\EFCore3Parser\bin\Debug\netcoreapp2.2\publish
set publish[7]=..\Utilities\EFCore3Parser\bin\Debug\netcoreapp3.1\publish

set build[0]=..\Utilities\EF6Parser\bin\Debug\net472\win7-x86
set build[1]=..\Utilities\EF6Parser\bin\Debug\netcoreapp3.1\win7-x86
set build[2]=..\Utilities\EFCore2Parser\bin\Debug\net472\win7-x86
set build[3]=..\Utilities\EFCore2Parser\bin\Debug\netcoreapp2.2\win7-x86
set build[4]=..\Utilities\EFCore2Parser\bin\Debug\netcoreapp3.1\win7-x86
set build[5]=..\Utilities\EFCore3Parser\bin\Debug\net472\win7-x86
set build[6]=..\Utilities\EFCore3Parser\bin\Debug\netcoreapp2.2\win7-x86
set build[7]=..\Utilities\EFCore3Parser\bin\Debug\netcoreapp3.1\win7-x86

set input[0]=.\EF6\EF6NetFramework\bin\Debug\EF6NetFramework.dll
set input[1]=.\EF6\EF6NetCore3\bin\Debug\netcoreapp3.1\EF6NetCore3.dll
set input[2]=.\EF6\EF6NetStandard\bin\Debug\netstandard2.1\EF6NetStandard.dll
set input[3]=.\EFCoreV2\EFCore2NetCore2\bin\Debug\netcoreapp2.2\EFCore2NetCore2.dll
set input[4]=.\EFCoreV2\EFCore2NetCore3\bin\Debug\netcoreapp3.1\EFCore2NetCore3.dll
set input[5]=.\EFCoreV2\EFCore2NetStandard\bin\Debug\netstandard2.1\EFCore2NetStandard.dll
set input[6]=.\EFCoreV3\EFCore3NetFramework\bin\Debug\EFCore3NetFramework.dll
set input[7]=.\EFCoreV3\EFCore3NetCore2\bin\Debug\netcoreapp2.2\EFCore3NetCore2.dll
set input[8]=.\EFCoreV3\EFCore3NetCore3\bin\Debug\netcoreapp3.1\EFCore3NetCore3.dll
set input[9]=.\EFCoreV3\EFCore3NetStandard\bin\Debug\netstandard2.1\EFCore3NetStandard.dll

rem goto script

for %%t in (package,publish,build) do (
   echo ==================================================================
   echo %%t
   echo ==================================================================

   for %%p in (0,1,2,3,4,5,6,7) do (
      set fldr=!%%t[%%p]!

      echo\
      echo [%%p] !fldr!\!pgm[%%p]!

      for %%i in (0,1,2,3,4,5,6,7,8,9) do (
         cd "!fldr!"
         "!pgm[%%p]!" "!input[%%i]!" c:\temp\parsertest.log
         echo       !ERRORLEVEL! -  [%%i] !input[%%i]!
         cd "!initialdir!"
      )
   )
   echo\
   echo\
)

exit /b

:script

for %%t in (package,publish,build) do (
   echo ==================================================================
   echo %%t
   echo ==================================================================

   for %%p in (0,1,2,3,4,5,6,7) do (
      set fldr=!%%t[%%p]!

      for %%i in (0,1,2,3,4,5,6,7,8,9) do (
         echo "!fldr!\!pgm[%%p]!" "!input[%%i]!" c:\temp\parsertest.log
      )
   )
   echo\
   echo\
)

exit /b


