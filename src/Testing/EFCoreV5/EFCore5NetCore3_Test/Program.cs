using System;
using System.Linq;

using EFCore5NetCore3;

using Microsoft.EntityFrameworkCore;

namespace ConsoleTestNetCore3
{
   class Program
   {
      static void Main(string[] args)
      {
         using (TestContext context = new TestContext())
         {
            context.Database.Migrate();
         }
      }
   }
}
