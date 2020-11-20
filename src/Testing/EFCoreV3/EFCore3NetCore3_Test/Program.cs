using System;
using System.Linq;

namespace EFCore3NetCore3_Test
{
   class Program
   {
      static void Main(string[] args)
      {
         using (Testing.AllFeatureModel context = new Testing.AllFeatureModel())
         {
            int x = context.UChilds.Count();
         }
      }
   }
}
