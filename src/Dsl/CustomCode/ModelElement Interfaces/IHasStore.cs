using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel {
   public interface IHasStore
   {
      Store Store { get; }
   }
}