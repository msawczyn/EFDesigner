using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using ParsingModels;

namespace EFCoreParser
{
   public class Parser
   {
      private readonly DbContext dbContext;

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
         {
            DbContextOptions options = new DbContextOptions<DbContext>();
         }

         result = assembly.CreateInstance(dbContextTypeName, false, BindingFlags.Default, null, new object[] {"App=EntityFramework"}, null, null) as DbContext;

         return result;
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
