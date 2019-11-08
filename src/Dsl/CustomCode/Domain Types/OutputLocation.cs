using System.ComponentModel;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{

   [TypeConverter(typeof(ExpandableObjectConverter))]
   public class OutputLocation
   {
      private readonly ModelRoot modelRoot;

      public OutputLocation(ModelRoot modelRoot)
      {
         this.modelRoot = modelRoot;
      }

      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/ContextOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/ContextOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/ContextOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string DbContext
      {
         get
         {
            return modelRoot.ContextOutputDirectory;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.ContextOutputDirectory = value;
               t.Commit();
            }
         }
      }

      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Entity { get; set; }

      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Enum { get; set; }

      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Struct { get; set; }

      //public OutputLocationDetail Entity { get; set; }
      //public OutputLocationDetail Enum { get; set; }
      //public OutputLocationDetail Struct { get; set; }

      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         // to prevent unwanted text in the property editor
         return string.Empty;
      }
   }

   [TypeConverter(typeof(ExpandableObjectConverter))]
   public class OutputLocationDetail
   {
      public string Project { get; set; }
      public string Folder { get; set; }
   }

}