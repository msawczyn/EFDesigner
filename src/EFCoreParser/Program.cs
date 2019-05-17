using System;
using System.IO;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace EFCoreParser
{
   internal class Program
   {
      public const int SUCCESS = 0;
      public const int BAD_ARGUMENT_COUNT = 1;
      public const int CANNOT_LOAD_ASSEMBLY = 2;
      public const int CANNOT_WRITE_OUTPUTFILE = 3;
      public const int CANNOT_CREATE_DBCONTEXT = 4;

      private static int Main(string[] args)
      {
         if (args.Length < 2 || args.Length > 3)
            return BAD_ARGUMENT_COUNT;

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
                  DbContext dbContext = Parser.GetDbContext(assembly, contextClassName);

                  if (dbContext == null)
                     return CANNOT_CREATE_DBCONTEXT;

                  Parser parser = new Parser(dbContext);
                  output.Write(parser.Process());
                  output.Flush();
                  output.Close();
               }
               catch 
               {
                  return CANNOT_LOAD_ASSEMBLY;
               }
            }
         }
         catch 
         {
            return CANNOT_WRITE_OUTPUTFILE;
         }

         return SUCCESS;
      }
   }
}
