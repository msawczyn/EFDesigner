using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary_Core
{
   partial class EFModel
   {
      static partial void ModifyCustomContextOptions(DbContextOptions<EFModel> options)
      {
         DbContextOptionsBuilder<EFModel> builder = new DbContextOptionsBuilder<EFModel>(options);
         builder.UseSqlServer(ConnectionString);
      }
   }
}
