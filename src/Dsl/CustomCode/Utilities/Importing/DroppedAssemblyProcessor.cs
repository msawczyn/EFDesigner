using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [Serializable]
   public class DroppedAssemblyProcessor: MarshalByRefObject
   {
      private readonly Store store;
      private readonly string filename;
      private readonly AssemblyReflectionManager manager;
      private readonly EFVersion? efVersion;

      private const string WORKAREA = "workarea";

      public DroppedAssemblyProcessor(Store store, string filename)
      {
         try
         {
            AssemblyName.GetAssemblyName(filename);
         }
         catch
         {
            // whoops! not an assembly!
            return;
         }

         this.store = store;
         this.filename = filename;

         manager = new AssemblyReflectionManager();
         manager.LoadAssembly(filename, WORKAREA);

         ContextClasses = manager.Reflect(filename,
                                          a =>
                                          {
                                             List<string> names = new List<string>();
                                             Type[] types = a.GetTypes();

                                             foreach (Type t in types.Where(t => DerivesFrom(t, "System.Data.Entity.DbContext")))
                                                names.Add(t.FullName);

                                             return names;
                                          });

         if (ContextClasses.Any())
         {
            efVersion = EFVersion.EF6;
            return;
         }

         ContextClasses = manager.Reflect(filename,
                                          a =>
                                          {
                                             List<string> names = new List<string>();
                                             Type[] types = a.GetTypes();

                                             foreach (Type t in types.Where(t => DerivesFrom(t, "Microsoft.EntityFrameworkCore.DbContext")))
                                                names.Add(t.FullName);

                                             return names;
                                          });

         if (ContextClasses.Any())
         {
            efVersion = EFVersion.EFCore;
         }

      }

      public IEnumerable<string> ContextClasses
      {
         get;
      }

      public bool ProcessAssembly()
      {
         switch (efVersion)
         {
            case EFVersion.EF6:

               return ProcessEF6Assembly(ContextClasses.First());
            case EFVersion.EFCore:

               return ProcessEFCoreAssembly(ContextClasses.First());
            default:

               return false;
         }
      }

      private static bool DerivesFrom(Type t, string target)
      {
         Type type = t;
         string typeName = type.FullName;
         while (typeName != target && type.BaseType != null)
         {
            type = type.BaseType;
            typeName = type.FullName;
         }
         return typeName == target;
      }

      public bool ProcessEFCoreAssembly(string contextClassName)
      {
         return false;
      }

      public bool ProcessEF6Assembly(string contextClassName)
      {
         // calculate where our EF6 processing code lives
         string location = Assembly.GetExecutingAssembly().CodeBase;
         if (location.StartsWith("file:///")) location = location.Substring(8);
         string ef6ProcessingPath = Path.Combine(Path.GetDirectoryName(location), "EFDesigner.EF6Processing.dll");

         manager.LoadAssembly(ef6ProcessingPath, WORKAREA);

         string processingResultJSON = manager.Reflect(ef6ProcessingPath,
                                          assembly =>
                                          {
                                             object ef6Processor = assembly.CreateInstance("EFDesigner.EF6Processing.EF6Processor");
                                             MethodInfo processContextMethod = ef6Processor.GetType().GetMethod("ProcessContext");
                                             return processContextMethod.Invoke(ef6Processor, BindingFlags.Default, null, new[] {contextClassName}, null) as string;
                                          });

         return false;
      }
   }
}
