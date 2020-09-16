using System;
using System.Linq;

using Testing;

namespace Sandbox_EFCore5NetCore3_Test
{
   class Program
   {
      static void Main(string[] args)
      {
         using (AllFeatureModel model = new AllFeatureModel())
         {
            Console.WriteLine(model.Entity1.Count());
         }
      }
   }
}
