using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class ModelDiagram
   {
      private EFModelDiagram diagram;

      public void SetDiagram(EFModelDiagram d) { diagram = d; }

      public EFModelDiagram GetDiagram() { return diagram; }
   }
}
