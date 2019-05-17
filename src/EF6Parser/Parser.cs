using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

using ParsingModels;

namespace EF6Parser
{
   public class Parser
   {
      private readonly DbContext dbContext;
      private List<System.Data.Entity.Core.Metadata.Edm.NavigationProperty> processedNavigationProperties;

      public Parser(DbContext dbContext)
      {
         this.dbContext = dbContext;
      }

      public static DbContext GetDbContext(Assembly assembly, string dbContextTypeName = null)
      {
         DbContext result = null;

         Type contextType = dbContextTypeName != null
                               ? assembly.GetTypes().FirstOrDefault(t => t.FullName == dbContextTypeName)
                               : assembly.GetTypes().FirstOrDefault(t => t.IsAssignableFrom(typeof(DbContext)));

         if (contextType != null)
            result = assembly.CreateInstance(dbContextTypeName, false, BindingFlags.Default, null, new object[] {"App=EntityFramework"}, null, null) as DbContext;

         return result;
      }

      public string Process()
      {
         if (dbContext == null)
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("dbContext");

         ObjectContext objContext = ((IObjectContextAdapter)dbContext).ObjectContext;
         processedNavigationProperties = new List<System.Data.Entity.Core.Metadata.Edm.NavigationProperty>();

         foreach (EntityType entityType in objContext.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EntityType>().ToList())
            ProcessEntity(entityType);

         foreach (EnumType enumType in objContext.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EnumType>().ToList())
            ProcessEnum(enumType);

         return null;
      }

      private ModelClass ProcessEntity(EntityType entityType)
      {
         ModelClass result = null;

         return result;
      }

      private ModelEnum ProcessEnum(EnumType enumType)
      {
         ModelEnum result = null;

         return result;
      }
   }
}
