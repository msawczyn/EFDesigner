using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModel.AssemblyProcessor.Interface
{
   public class Class
   {
      public bool IsAbstract { get; set; }
      public string TableName { get; set; }
      public string DatabaseSchema { get; set; }
      public string Namespace { get; set; }
      public string DbSetName { get; set; }
      public string Name { get; set; }

   }
}
