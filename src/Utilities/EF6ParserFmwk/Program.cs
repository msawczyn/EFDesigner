using System;
using System.IO;
using System.Reflection;

namespace EF6Parser
{
   internal class Program
   {
      public const int CANCELLED = -1;
      public const int SUCCESS = 0;
      public const int BAD_ARGUMENT_COUNT = 1;
      public const int CANNOT_LOAD_ASSEMBLY = 2;
      public const int CANNOT_WRITE_OUTPUTFILE = 3;
      public const int CANNOT_CREATE_DBCONTEXT = 4;
      public const int CANNOT_FIND_APPROPRIATE_CONSTRUCTOR = 5;
      public const int AMBIGUOUS_REQUEST = 6;

      private static int Main(string[] args)
      {
         if (args.Length < 2 || args.Length > 3)
         {
            Usage();
            return BAD_ARGUMENT_COUNT;
         }

         try
         {
            string inputPath = args[0];
            string outputPath = args[1];
            string contextClassName = args.Length == 3 ? args[2] : null;

            using (StreamWriter output = new StreamWriter(outputPath))
            {
               try
               {
                  Assembly assembly = Assembly.LoadFrom(inputPath);
                  Parser parser = null;

                  try
                  {
                     parser = new Parser(assembly, contextClassName);
                     output.Write(parser.Process());
                  }

                  // ReSharper disable once UncatchableException
                  catch (MissingMethodException)
                  {
                     return CANNOT_FIND_APPROPRIATE_CONSTRUCTOR;
                  }
                  catch (AmbiguousMatchException)
                  {
                     foreach (string className in parser.DbContextClasses)
                        output.WriteLine(className);

                     Usage();
                     return AMBIGUOUS_REQUEST;
                  }
                  catch
                  {
                     Usage();
                     return CANNOT_CREATE_DBCONTEXT;
                  }
               }
               catch
               {
                  Usage();
                  return CANNOT_LOAD_ASSEMBLY;
               }
               finally
               {
                  output.Flush();
                  output.Close();
               }
            }
         }
         catch 
         {
            Usage();
            return CANNOT_WRITE_OUTPUTFILE;
         }

         return SUCCESS;
      }

      private static void Usage()
      {
         Console.Error.WriteLine("Usage: EF6Parser InputFileName OutputFileName [FullyQualifiedClassName]");
         Console.Error.WriteLine("where");
         Console.Error.WriteLine("   (required) InputFileName           - path of assembly containing EF6 DbContext to parse");
         Console.Error.WriteLine("   (required) OutputFileName          - path to create JSON file of results");
         Console.Error.WriteLine("   (optional) FullyQualifiedClassName - fully-qualified name of DbContext class to process, if more than one available.");
         Console.Error.WriteLine("                                        DbContext class must have a constructor that takes a connection string name");
         Console.Error.WriteLine("                                        or value, or a constructor that takes a DbConnection object");
         Console.Error.WriteLine();
      }
   }
}