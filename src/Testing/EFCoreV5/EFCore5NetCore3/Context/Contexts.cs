using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace EFCore5NetCore3
{
   //partial class BidirectionalAssociationTestContext
   //{
   //   public BidirectionalAssociationTestContext() { }

   //   partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
   //   {
   //      optionsBuilder.UseSqlServer(ConnectionString);
   //   }

   //}

   partial class UnidirectionalAssociationTestContext
   {
      public UnidirectionalAssociationTestContext() { }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }

   }
}
