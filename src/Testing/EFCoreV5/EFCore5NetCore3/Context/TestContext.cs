using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace EFCore5NetCore3
{
   partial class TestContext
   {
      public TestContext() { }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }

   }
}
