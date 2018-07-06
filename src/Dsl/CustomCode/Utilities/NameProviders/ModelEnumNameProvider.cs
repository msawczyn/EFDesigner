using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
