using Microsoft.EntityFrameworkCore;

using System;

namespace Sandbox_EFCore5NetCore3_Test
{
   class Program
   {
      static void Main()
      {
         using (Context context = new Context())
         {
            context.Database.ExecuteSqlRaw(@"
use [master];
EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'EFC5Test'
ALTER DATABASE [EFC5Test] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE [EFC5Test]
CREATE DATABASE [EFC5Test]
");
            context.Database.Migrate();
         }
      }
   }
}
