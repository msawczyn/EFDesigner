using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class Unidirectional
   {
      public string SourceTypeName { get; set; }
      public int SourceMultiplicity { get; set; }
      public bool SourceIsDependent { get; set; }
      public string TargetTypeName { get; set; }
      public int TargetMultiplicity { get; set; }
      public bool TargetIsDependent { get; set; }
      public string TargetPropertyName { get; set; }
      public bool CascadeDelete { get; set; }
   }
}
