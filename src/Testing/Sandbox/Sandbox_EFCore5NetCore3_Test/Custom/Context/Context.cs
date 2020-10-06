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
      public Context() { }
      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseInMemoryDatabase("Sandbox");
         //optionsBuilder.UseSqlServer(ConnectionString);
      }
   }
}
