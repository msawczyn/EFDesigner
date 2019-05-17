using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ParsingModels;

namespace EF6Parser
{
   [SuppressMessage("ReSharper", "UnthrowableException")]
   public class Parser
   {
      private readonly DbContext dbContext;
      private readonly MetadataWorkspace metadata;
      private List<NavigationProperty> processedNavigationProperties;

      public Parser(Assembly assembly, string dbContextTypeName = null)
      {
         Type contextType;
         if (dbContextTypeName != null)
            contextType = assembly.GetExportedTypes().FirstOrDefault(t => t.FullName == dbContextTypeName);
         else
         {
            List<Type> types = assembly.GetExportedTypes().Where(t => typeof(DbContext).IsAssignableFrom(t)).ToList();
            if (types.Count != 1)
               throw new AmbiguousMatchException("Found more than one class derived from DbContext");

            contextType = types[0];
         }

         ConstructorInfo constructor = contextType.GetConstructor(new[] {typeof(string)});
         if (constructor == null)
            throw new MissingMethodException("Can't find appropriate constructor");

         dbContext = assembly.CreateInstance(contextType.FullName, false, BindingFlags.Default, null, new object[]{"App=EntityFramework"}, null, null) as DbContext;
         metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;
      }

      public string Process()
      {
         if (dbContext == null)

            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("dbContext");

         processedNavigationProperties = new List<NavigationProperty>();

         ModelRoot modelRoot = ProcessRoot();

         foreach (EntityType entityType in metadata.GetItems(DataSpace.CSpace).OfType<EntityType>().ToList())
         {
            ModelClass modelClass = ProcessEntity(entityType);
            if (modelClass != null)
               modelRoot.Classes.Add(modelClass);
         }

         foreach (EnumType enumType in metadata.GetItems(DataSpace.CSpace).OfType<EnumType>().ToList())
         {
            ModelEnum modelEnum = ProcessEnum(enumType);
            if (modelEnum != null)
               modelRoot.Enumerations.Add(modelEnum);
         }

         return JsonConvert.SerializeObject(modelRoot);
      }

      private ModelRoot ProcessRoot()
      {
         ModelRoot result = new ModelRoot();
         Type contextType = dbContext.GetType();

         result.Name = contextType.Name;
         result.Namespace = contextType.Namespace;
         return result;
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