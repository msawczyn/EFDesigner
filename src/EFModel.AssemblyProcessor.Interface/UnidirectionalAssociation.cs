using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class UnidirectionalAssociation
   {
      public int SourceMultiplicity { get; set; }
      public int TargetMultiplicity { get; set; }
      public string TargetPropertyName { get; set; }
      public bool SourceIsDependent { get; set; }
      public bool TargetIsDependent { get; set; }

   }
}
