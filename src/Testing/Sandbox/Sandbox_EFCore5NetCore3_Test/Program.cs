using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace Sandbox_EFCore5NetCore3_Test
{
   class Program
   {
      static void Main()
      {
         //using (Context context = new Context())
         //{
         //            context.Database.ExecuteSqlRaw(@"
         //use [master];
         //EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'EFC5Test';
         //ALTER DATABASE [EFC5Test] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
         //DROP DATABASE [EFC5Test];
         //CREATE DATABASE [EFC5Test];
         //");
         //            context.Database.Migrate();
         //}

         //using (Context context = new Context())
         //{
         //   context.Database.ExecuteSqlRaw("delete from Entity2");
         //   context.Database.ExecuteSqlRaw("delete from Entity1");
         //}

         using (Context context = new Context())
         {
            Entity1 e1 = new Entity1();
            Entity2[] e2 = new[] { new Entity2("Foo", e1), new Entity2("Bar", e1), new Entity2("Zoom", e1) };

            //e2.ToList().ForEach(x => e1.Entity2.Add(x));
            //context.Entity1.Add(e1);

            context.SaveChanges();
            Console.WriteLine("In context 1");
            context.Entity1.SelectMany(x => x.Entity2.Select(y => y.Property1)).ToList().ForEach(s => Console.WriteLine(s));
         }

         using (Context context = new Context())
         {
            Console.WriteLine();
            Console.WriteLine("In context 2");
            context.Entity1.SelectMany(x => x.Entity2.Select(y => y.Property1)).ToList().ForEach(s => Console.WriteLine(s));
         }
      }
   }
}
