using System;
using System.Linq;

using Testing;

namespace ConsoleTestNetCore3
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
