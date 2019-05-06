using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class Property
   {
      public string TypeName { get; set; }
      public string Name { get; set; }
      public string InitialValue { get; set; }
      public bool IsIdentity { get; set; }
      public bool Required { get; set; }
      public int MaxLength { get; set; }
   }
}
