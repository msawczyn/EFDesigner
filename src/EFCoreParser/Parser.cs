using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using ParsingModels;

namespace EFCoreParser
{
   public class Parser
   {
      private readonly Assembly assembly;
      private readonly DbContext dbContext;
      private IModel model;
      //Microsoft.EntityFrameworkCore.Metadata.INavigation;
      public Parser(Assembly assembly, string dbContextTypeName = null)
      {
         this.assembly = assembly;
         Type contextType;

         if (dbContextTypeName != null)
            contextType = assembly.GetExportedTypes().FirstOrDefault(t => t.FullName == dbContextTypeName);
         else
         {
            List<Type> types = assembly.GetExportedTypes().Where(t => typeof(DbContext).IsAssignableFrom(t)).ToList();

            // ReSharper disable once UnthrowableException
            if (types.Count != 1)
               throw new AmbiguousMatchException("Found more than one class derived from DbContext");

            contextType = types[0];
         }

         ConstructorInfo constructor = contextType.GetConstructor(Type.EmptyTypes);

         // ReSharper disable once UnthrowableException
         if (constructor == null)
            throw new MissingMethodException("Can't find default constructor");

         dbContext = assembly.CreateInstance(contextType.FullName) as DbContext;
         model = dbContext.Model;
      }

      private static Multiplicity ConvertMultiplicity(RelationshipMultiplicity relationshipMultiplicity)
      {
         Multiplicity multiplicity = Multiplicity.ZeroOne;

         switch (relationshipMultiplicity)
         {
            case RelationshipMultiplicity.ZeroOrOne:
               multiplicity = Multiplicity.ZeroOne;

               break;

            case RelationshipMultiplicity.One:
               multiplicity = Multiplicity.One;

               break;

            case RelationshipMultiplicity.Many:
               multiplicity = Multiplicity.ZeroMany;

               break;
         }

         return multiplicity;
      }

      public string Process()
      {
         if (dbContext == null)
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("dbContext");


         return null;
      }

      private ModelClass ProcessEntity(EntityType entityType)
      {
         ModelClass result = null;

         return result;
      }

   }
}
