using System.ComponentModel;

namespace Sawczyn.EFDesigner.EFModel
{
   internal sealed partial class EFModelPackage
   {
      protected override void Initialize()
      {
         TypeDescriptor.AddProvider(new ModelClassTypeDescriptionProvider(), typeof(ModelClass));
         TypeDescriptor.AddProvider(new ModelEnumTypeDescriptionProvider(), typeof(ModelEnum));
         TypeDescriptor.AddProvider(new AssociationTypeDescriptionProvider(), typeof(Association));

         base.Initialize();
      }
   }
}
