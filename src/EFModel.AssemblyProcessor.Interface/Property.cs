using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class Property
   {
      public string InitialValue { get; set; }
      public bool IsIdentity { get; set; }
      public bool Required { get; set; }
      public int MaxLength { get; set; }
   }
}
