using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
   class Program
   {
      static void Main(string[] args)
      {
         Program p = new Program();
         p.Run();
      }

      internal void Run()
      {
         using (SandboxContext db = new SandboxContext())
         {
            db.Blogs.Add
         }
      }
   }
}
