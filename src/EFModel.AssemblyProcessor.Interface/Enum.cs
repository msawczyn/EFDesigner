using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class Enum
   {
      public string ValueType { get; set; }
      public string Namespace { get; set; }
      public string Name { get; set; }
      public bool IsFlags { get; set; }
   }
}
