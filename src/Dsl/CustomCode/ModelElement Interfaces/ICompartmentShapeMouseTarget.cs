using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public interface ICompartmentShapeMouseTarget
   {
      void MoveCompartmentItem(ModelElement dragFrom, DiagramMouseEventArgs e);
   }
}