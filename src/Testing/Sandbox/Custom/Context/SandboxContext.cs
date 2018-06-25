using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Sandbox
{
   public partial class SandboxContext   {
      partial void CustomInit(ref DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(ConnectionString);
      }
   }
}
