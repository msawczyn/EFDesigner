using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class BidirectionalAssociation : UnidirectionalAssociation
   {
      public string SourcePropertyName { get; set; }
   }
}
