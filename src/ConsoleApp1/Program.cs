// https://www.codeproject.com/Articles/453778/Loading-Assemblies-from-Anywhere-into-a-New-AppDom

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace ConsoleApp1
{
   internal class Program
   {
      private static void Main(string[] args)
      {
         Source dbContext = new Source();
         ObjectContext objContext = ((IObjectContextAdapter)dbContext).ObjectContext;
         List<EntityType> entityTypes = objContext.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EntityType>().ToList();
      }

      public static List<PropertyInfo> GetDbSetProperties(this DbContext context)
      {
         var dbSetProperties = new List<PropertyInfo>();
         var properties = context.GetType().GetProperties();

         foreach (var property in properties)
         {
            var setType = property.PropertyType;

#if EF5 || EF6
            var isDbSet = setType.IsGenericType && (typeof (IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) || setType.GetInterface(typeof (IDbSet<>).FullName) != null);
#elif EF7
            var isDbSet = setType.IsGenericType && (typeof (DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
#endif

            if (isDbSet)
            {
               dbSetProperties.Add(property);
            }
         }

         return dbSetProperties;

      }
   }
}
