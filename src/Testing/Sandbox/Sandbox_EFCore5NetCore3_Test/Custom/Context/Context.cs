using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sandbox_EFCore5NetCore3_Test
{
   /// <inheritdoc/>
   partial class Context
   {
      public Context() : base(Options)
      {
         //System.Diagnostics.Debugger.Launch();
      }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         //optionsBuilder.UseInMemoryDatabase("Sandbox");
         //optionsBuilder.UseSqlite("Data Source=c:\\temp\\efcore5test.db");
         optionsBuilder.UseSqlServer(ConnectionString);
      }

      public static DbContextOptions Options  
      {
         get 
         {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(ConnectionString);
            return optionsBuilder.Options;
         }
      }
   }
}
