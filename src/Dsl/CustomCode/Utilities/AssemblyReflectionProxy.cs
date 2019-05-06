// https://www.codeproject.com/Articles/453778/Loading-Assemblies-from-Anywhere-into-a-New-AppDom

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sawczyn.EFDesigner.EFModel {
   public class AssemblyReflectionProxy : MarshalByRefObject
   {
      private string _assemblyPath;

      public void LoadAssembly(string assemblyPath)
      {
         try
         {
            _assemblyPath = assemblyPath;
            Assembly.ReflectionOnlyLoadFrom(assemblyPath);
         }
         catch (FileNotFoundException)
         {
            // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain.
         }
      }

      private Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
      {
         Assembly loadedAssembly =
            AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                     .FirstOrDefault(asm => string.Equals(asm.FullName,
                                                          args.Name,
                                                          StringComparison.OrdinalIgnoreCase));

         if (loadedAssembly != null)
         {
            return loadedAssembly;
         }

         AssemblyName assemblyName =
            new AssemblyName(args.Name);

         string dependentAssemblyFilename =
            Path.Combine(directory.FullName,
                         assemblyName.Name + ".dll");

         if (File.Exists(dependentAssemblyFilename))
         {
            return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);
         }

         return Assembly.ReflectionOnlyLoad(args.Name);
      }

      public TResult Reflect<TResult>(Func<Assembly, TResult> func)
      {
         DirectoryInfo directory = new FileInfo(_assemblyPath).Directory;

         AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveEventHandler;

         Assembly assembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(a => a.Location.CompareTo(_assemblyPath) == 0);

         TResult result = func(assembly);

         AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ResolveEventHandler;

         return result;

         Assembly ResolveEventHandler(object s, ResolveEventArgs e)
         {
            return OnReflectionOnlyResolve(e, directory);
         }


      }
   }
}