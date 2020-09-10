using System;
using System.Linq;

using Testing;

namespace EFCore3NetStandard_NetCore3Test
{
   class Program
   {
      static void Main(string[] args)
      {
         using (AllFeatureModel model = new AllFeatureModel())
         {
            Console.WriteLine(model.BaseClasses.Count());
         }
      }
   }
}
