using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace ParserTests
{

   [TestFixture]
   public class ParserTest
   {
      [TestCaseSource(typeof(TestData), nameof(TestData.TargetTestCases))]
      public void TestParserTargets(string parser, string input)
      {
         string args = parser.IndexOf("net472", StringComparison.Ordinal) == -1
                             ? $@"{parser} {input} c:\temp\parsertest.json"
                             : $@"{input} c:\temp\parsertest.json";

         string cmd = parser.IndexOf("net472", StringComparison.Ordinal) == -1
                         ? "dotnet"
                         : parser;

         ProcessStartInfo ps = new ProcessStartInfo(cmd, args);
         Process process = Process.Start(ps);
         process.WaitForExit();

         Assert.AreEqual(0, process.ExitCode);
      }


      [TestCaseSource(typeof(TestData), nameof(TestData.PackageTestCases))]
      public void TestParserPackage(string parser, string input)
      {
         string args = $@"{input} c:\temp\parsertest.json";

         ProcessStartInfo ps = new ProcessStartInfo(parser, args);
         Process process = Process.Start(ps);
         process.WaitForExit();

         Assert.AreEqual(0, process.ExitCode);
      }
   }

   public class TestData
   {
      private static string _dir;
      private static string dir //= Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).TrimEnd('\\');
      {
         get
         {
            if (_dir == null)
            {
               List<string> parts = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Split('\\').ToList();

               while (parts.Last().ToLower() != "src")
                  parts.RemoveAt(parts.Count - 1);

               parts.RemoveAt(0);

               _dir = string.Join('\\', parts);
            }

            return _dir;
         }
      }

      //static TestData()
      //{
      //   ProcessStartInfo ps = new ProcessStartInfo("dotnet.exe", "msbuild /t:PublishAll /p:Configuration=Debug")
      //                         {
      //                            WorkingDirectory = dir,
      //                            CreateNoWindow = false,
      //                            UseShellExecute = true
      //                         };
      //   Process process = Process.Start(ps);

      //   process.WaitForExit();
      //}

      public static IEnumerable TargetTestCases
      {
         get
         {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string parser in Parsers)
            {
               foreach (string input in Inputs)
                  yield return new TestCaseData(Path.Combine(dir, parser), Path.Combine(dir, input));
            }
         }
      }

      public static IEnumerable PackageTestCases
      {
         get
         {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string parser in PackagedParsers)
            {
               foreach (string input in Inputs)
                  yield return new TestCaseData(Path.Combine(dir, parser), Path.Combine(dir, input));
            }
         }
      }

      public static string[] Inputs =
      {
         @"Testing\EF6\EF6NetCore3\bin\Debug\netcoreapp3.1\EF6NetCore3.dll"
       , @"Testing\EF6\EF6NetFramework\bin\Debug\EF6NetFramework.dll"
       , @"Testing\EF6\EF6NetStandard\bin\Debug\netstandard2.1\EF6NetStandard.dll"
       , @"Testing\EFCoreV2\EFCore2NetCore2\bin\Debug\netcoreapp2.2\EFCore2NetCore2.dll"
       , @"Testing\EFCoreV2\EFCore2NetCore3\bin\Debug\netcoreapp3.1\EFCore2NetCore3.dll"
       , @"Testing\EFCoreV2\EFCore2NetFramework\bin\Debug\EFCore2NetFramework.dll"
       , @"Testing\EFCoreV2\EFCore2NetStandard\bin\Debug\netstandard2.1\EFCore2NetStandard.dll"
       , @"Testing\EFCoreV3\EFCore3NetCore2\bin\Debug\netcoreapp2.2\EFCore3NetCore2.dll"
       , @"Testing\EFCoreV3\EFCore3NetCore3\bin\Debug\netcoreapp3.1\EFCore3NetCore3.dll"
       , @"Testing\EFCoreV3\EFCore3NetFramework\bin\Debug\EFCore3NetFramework.dll"
       , @"Testing\EFCoreV3\EFCore3NetStandard\bin\Debug\netstandard2.1\EFCore3NetStandard.dll"
      };

      public static string[] Parsers =
      {
         @"Utilities\EF6Parser\bin\Debug\net472\EF6Parser.exe"
       , @"Utilities\EF6Parser\bin\Debug\netcoreapp3.1\EF6Parser.exe"
       , @"Utilities\EF6Parser\bin\Debug\netcoreapp3.1\published\EF6Parser.exe"
       , @"Utilities\EFCore2Parser\bin\Debug\net472\EFCore2Parser.exe"
       , @"Utilities\EFCore2Parser\bin\Debug\netcoreapp3.1\EFCore2Parser.exe"
       , @"Utilities\EFCore2Parser\bin\Debug\published\EFCore2Parser.exe"
       , @"Utilities\EFCore3Parser\bin\Debug\net472\EFCore3Parser.exe"
       , @"Utilities\EFCore3Parser\bin\Debug\netcoreapp3.1\EFCore3Parser.exe"
       , @"Utilities\EFCore3Parser\bin\Debug\published\EFCore3Parser.exe"
      };

      public static string[] PackagedParsers =
      {
         @"DslPackage\Parsers\net472\EF6Parser.exe"
       , @"DslPackage\Parsers\net472\EFCore2Parser.exe"
       , @"DslPackage\Parsers\net472\EFCore3Parser.exe"
       , @"DslPackage\Parsers\netcoreapp2.1\EFCore2Parser.exe"
       , @"DslPackage\Parsers\netcoreapp2.1\EFCore3Parser.exe"
       , @"DslPackage\Parsers\netcoreapp3.1\EF6Parser.exe"
       , @"DslPackage\Parsers\netcoreapp3.1\EFCore2Parser.exe"
       , @"DslPackage\Parsers\netcoreapp3.1\EFCore3Parser.exe"
      };


   }
}

