using System;
using System.Reflection;

namespace EFModel.AssemblyProcessor
{
   public class TypeProxy : MarshalByRefObject
   {
      public Type LoadFromAssembly(string assemblyPath, string typeName)
      {
         try
         {
            Assembly asm = Assembly.LoadFile(assemblyPath);
            return asm.GetType(typeName);
         }
         catch (Exception) { return null; }
      }
   }
}