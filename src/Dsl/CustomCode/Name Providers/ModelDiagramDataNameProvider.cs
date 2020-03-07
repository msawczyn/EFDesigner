// 

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel {
   public class ModelDiagramDataNameProvider : ElementNameProvider
   {
      /// <summary>Sets unique name on an element.</summary>
      /// <param name="element">Element to assign an unique name.</param>
      /// <param name="container">Container embedding the element.</param>
      /// <param name="embeddedDomainRole">Role played by the element in embedding relationship.</param>
      /// <param name="baseName">String from which generated name should be derived.</param>
      /// <exception cref="T:System.ArgumentNullException">element, container or embeddedDomainRole is a null reference.</exception>
      /// <exception cref="T:System.InvalidOperationException">When called outside of modeling transaction context,
      /// name property is calculated or other modeling constraints are not satisfied.</exception>
      /// <exception cref="T:System.NotSupportedException">There are more than <see cref="F:System.UInt64.MaxValue" /> elements in container.</exception>
      public override void SetUniqueName(ModelElement element, ModelElement container, DomainRoleInfo embeddedDomainRole, string baseName)
      {
         base.SetUniqueName(element, container, embeddedDomainRole, "Diagram");
      }
   }
}