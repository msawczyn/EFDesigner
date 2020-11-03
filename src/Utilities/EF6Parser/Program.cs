using System;
using System.IO;
using System.Reflection;

using log4net;
using log4net.Config;
using log4net.Repository;

namespace EF6Parser
{
   internal class Program
   {
      private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            log.Error($"Expecting 2 or 3 arguments - found {args.Length}");
            Exit(BAD_ARGUMENT_COUNT);
         }

         try
         {
            string inputPath = args[0];
            string outputPath = args[1];
          
            GlobalContext.Properties["LogPath"] = Path.ChangeExtension(outputPath, "").TrimEnd('.');
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, GetLogStream());

            log.Info($"Starting {Assembly.GetEntryAssembly().Location}");
            log.Info($"Log file at {GlobalContext.Properties["LogPath"]}.log");

            string contextClassName = args.Length == 3 ? args[2] : null;

            using (StreamWriter output = new StreamWriter(outputPath))
            {
               try
               {
                  log.Info($"Loading {inputPath}");
                  Assembly assembly = Assembly.LoadFrom(inputPath);
                  Parser parser = null;

                  try
                  {
                     parser = new Parser(assembly, contextClassName);
                  }
                  // ReSharper disable once UncatchableException
                  catch (MissingMethodException ex)
                  {
                     log.Error(ex.Message);
                     Exit(CANNOT_FIND_APPROPRIATE_CONSTRUCTOR, ex);
                  }
                  catch (AmbiguousMatchException ex)
                  {
                     log.Error(ex.Message);
                     Exit(AMBIGUOUS_REQUEST, ex);
                  }
                  catch (Exception ex)
                  {
                     Exception e = ex;
                     do
                     {
                        log.Error(e.Message);
                        e = e.InnerException;
                     } while (e != null);

                     Exit(CANNOT_CREATE_DBCONTEXT, ex);
                  }

                  output.Write(parser?.Process());
                  output.Flush();
                  output.Close();
               }
               catch (Exception ex)
               {
                  log.Error(ex.Message);
                  Exit(CANNOT_LOAD_ASSEMBLY, ex);
               }
            }
         }
         catch (Exception ex)
         {
            log.Error(ex.Message);
            Exit(CANNOT_WRITE_OUTPUTFILE, ex);
         }

         log.Info("Success");
         return SUCCESS;
      }

      private static void Exit(int returnCode, Exception ex = null)
      {
         if (returnCode != 0)
         {
            log.Error($"Usage: {typeof(Program).Assembly.GetName().Name} InputFileName OutputFileName [FullyQualifiedClassName]");
            log.Error("where");
            log.Error("   (required) InputFileName           - path of assembly containing EF6 DbContext to parse");
            log.Error("   (required) OutputFileName          - path to create JSON file of results");
            log.Error("   (optional) FullyQualifiedClassName - fully-qualified name of DbContext class to process, if more than one available.");
            log.Error("                                        DbContext class must have a constructor that accepts one parameter of type DbContextOptions<>");
            log.Error("Result codes:");
            log.Error("   0   Success");
            log.Error("   1   Bad argument count");
            log.Error("   2   Cannot load assembly");
            log.Error("   3   Cannot write output file");
            log.Error("   4   Cannot create DbContext");
            log.Error("   5   Cannot find appropriate constructor");
            log.Error("   6   Ambiguous request");
            log.Error("");

            if (ex != null)
               log.Error($"Caught {ex.GetType().Name} - {ex.Message}");

            log.Error($"Exiting with return code {returnCode}");
         }         
         
         Environment.Exit(returnCode);
      }

      private static Stream GetLogStream()
      {
         MemoryStream stream = new MemoryStream();
         StreamWriter writer = new StreamWriter(stream);
         writer.Write(Resources.Log4netConfig);
         writer.Flush();
         stream.Position = 0;
         return stream;
      }
   }
}