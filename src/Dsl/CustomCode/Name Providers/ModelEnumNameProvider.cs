using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public class ModelEnumNameProvider : ElementNameProvider
   {
      public override void SetUniqueName(ModelElement element, ModelElement container, DomainRoleInfo embeddedDomainRole, string baseName)
      {
         base.SetUniqueName(element, container, embeddedDomainRole, "Enum");
      }
   }
}
