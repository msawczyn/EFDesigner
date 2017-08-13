using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EFModelDiagram
   {
      public override void OnInitialize()
      {
         base.OnInitialize();

         // because we can hide elements, line routing looks odd when it thinks it's jumping over lines
         // that really aren't visible. Since replacing the routing algorithm is too hard (impossible?)
         // let's just stop it from showing jumps at all. A change to the highlighting on mouseover
         // makes it easier to see which lines are which in complex diagrams, so this doesn't hurt anything.
         RouteJumpType = VGPageLineJumpCode.NoJumps;
      }
   }
}
