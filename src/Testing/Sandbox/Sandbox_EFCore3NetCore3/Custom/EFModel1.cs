using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFDesignerCoreTest
{
   /// <inheritdoc/>
   public partial class EFModel1 
   {
      public EFModel1() { }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }
   }
}
