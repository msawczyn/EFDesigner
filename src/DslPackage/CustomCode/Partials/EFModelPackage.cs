using System.ComponentModel;

using Microsoft.VisualStudio.Shell;

using Sawczyn.EFDesigner.EFModel.DslPackage;

namespace Sawczyn.EFDesigner.EFModel
{
   [ProvideOptionPage(typeof(OptionsPage), "Entity Framework Visual Editor", "Visual Editor Options", 0, 0, true)]
   internal sealed partial class EFModelPackage
   {
      public static EFModelPackage Instance
      {
         get;
         private set;
      }

      public static OptionsPage Options
      {
         get => (OptionsPage)Instance.GetDialogPage(typeof(OptionsPage));
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
