using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Testing
{
   public partial class AllFeatureModel
   {
      public AllFeatureModel() { }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }

      private string GetQueryEntitySqlQuery()
      {
         return "SELECT * FROM Foo";
      }
   }
}
