using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

//namespace Testing
//{
//   partial class AllFeatureModel
//   {
//      public AllFeatureModel() { }

//      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
//      {
//         optionsBuilder.UseSqlServer(ConnectionString);
//      }

//   }
//}

namespace EFCore5NetCore3
{
   /// <inheritdoc/>
   public partial class TestContext 
   {
      public TestContext() { }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }
   }
}
