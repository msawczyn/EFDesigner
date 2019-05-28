using System.ComponentModel;

namespace Sawczyn.EFDesigner.EFModel
{
   internal sealed partial class EFModelPackage
   {
      public static EFModelPackage Instance
      {
         get;
         private set;
      }

      protected override void Initialize()
      {
         Instance = this;
         TypeDescriptor.AddProvider(new ModelClassTypeDescriptionProvider(), typeof(ModelClass));
         TypeDescriptor.AddProvider(new ModelEnumTypeDescriptionProvider(), typeof(ModelEnum));
         TypeDescriptor.AddProvider(new AssociationTypeDescriptionProvider(), typeof(Association));
         TypeDescriptor.AddProvider(new ModelAttributeTypeDescriptionProvider(), typeof(ModelAttribute));
         TypeDescriptor.AddProvider(new ModelRootTypeDescriptionProvider(), typeof(ModelRoot));

         base.Initialize();
      }
   }
}
