using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Testing
{
   partial class EFModel1
   {
      public EFModel1() { }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }

   }
}

