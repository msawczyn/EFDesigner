using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel {
   public interface IMouseActionTarget
   {
      void DoMouseUp(ModelElement dragFrom, DiagramMouseEventArgs e);
   }
}