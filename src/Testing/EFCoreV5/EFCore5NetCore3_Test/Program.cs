using System;
using System.Linq;

using Testing;

using Microsoft.EntityFrameworkCore;

namespace ConsoleTestNetCore3
{
   class Program
   {
      static void Main(string[] args)
      {
         using (AllFeatureModel context = new AllFeatureModel())
         {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
         }
      }
   }
}
