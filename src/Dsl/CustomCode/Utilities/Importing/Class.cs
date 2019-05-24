using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class Class: Element
   {
      public bool IsAbstract { get; set; }
      public string TableName { get; set; }
      public string DatabaseSchema { get; set; }
      public string Namespace { get; set; }
      public string DbSetName { get; set; }
      public string Name { get; set; }
      public List<Unidirectional> UnidirectionalAssociations { get; set; }
      public List<Bidirectional> BidirectionalAssociations { get; set; }
      public List<Property> Properties { get; set; }

      public Class()
      {
         UnidirectionalAssociations = new List<Unidirectional>();
         BidirectionalAssociations = new List<Bidirectional>();
         Properties = new List<Property>();
      }
   }
}
