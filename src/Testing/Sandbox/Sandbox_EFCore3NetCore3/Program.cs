using System;

namespace Testing
{
   class Program
   {
      static void Main(string[] args)
      {
         using (EFModel1 context = new EFModel1())
         {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
         }
      }
   }
}
