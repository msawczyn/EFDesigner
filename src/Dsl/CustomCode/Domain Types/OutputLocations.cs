using System;
using System.ComponentModel;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Encapsulates values of the current Output Location settings in the ModelRoot
   /// </summary>
   [Serializable]
   public class OutputLocations: IHasStore
   {
      private readonly ModelRoot modelRoot;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="modelRoot">DomainClass ModelRoot</param>
      public OutputLocations(ModelRoot modelRoot)
      {
         this.modelRoot = modelRoot;
      }

      /// <summary>
      /// Exposes the Store object.  Store is a complete model.  Stores contain both the domain data  and the model data for all the domain models in a model.
      /// </summary>
      [Browsable(false)]
      public Store Store => modelRoot?.Store;

      /// <summary>
      /// Output location value for the generated DbContext-derived object
      /// </summary>
      // ReSharper disable once UnusedMember.Global
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

      /// <summary>
      /// Output location value for the generated entity classes
      /// </summary>
      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Entity
      {
         get
         {
            return string.IsNullOrWhiteSpace(modelRoot.EntityOutputDirectory)
                      ? modelRoot.ContextOutputDirectory
                      : modelRoot.EntityOutputDirectory;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.EntityOutputDirectory = string.IsNullOrWhiteSpace(value) || value == modelRoot.EntityOutputDirectory
                                                    ? null
                                                    : value;

               t.Commit();
            }
         }
      }

      /// <summary>
      /// Output location value for the generated enumerations
      /// </summary>
      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Enumeration
      {
         get
         {
            return string.IsNullOrWhiteSpace(modelRoot.EnumOutputDirectory)
                      ? modelRoot.ContextOutputDirectory
                      : modelRoot.EnumOutputDirectory;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.EnumOutputDirectory = string.IsNullOrWhiteSpace(value) || value == modelRoot.EnumOutputDirectory
                                                  ? null
                                                  : value;

               t.Commit();
            }
         }
      }

      /// <summary>
      /// Output location value for the generated non-persistent objects
      /// </summary>
      [TypeConverter(typeof(ProjectDirectoryTypeConverter))]
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructOutputDirectory.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructOutputDirectory.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructOutputDirectory.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Struct
      {
         get
         {
            return string.IsNullOrWhiteSpace(modelRoot.StructOutputDirectory)
                      ? modelRoot.ContextOutputDirectory
                      : modelRoot.StructOutputDirectory;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.StructOutputDirectory = string.IsNullOrWhiteSpace(value) || value == modelRoot.StructOutputDirectory
                                                    ? null
                                                    : value;

               t.Commit();
            }
         }
      }

      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         // to prevent unwanted text in the property editor
         return string.Empty;
      }
   }

   // for later

   //[TypeConverter(typeof(ExpandableObjectConverter))]
   //public class OutputLocation
   //{
   //   public string Project { get; set; }
   //   public string Folder { get; set; }
   //}

   //[TypeConverter(typeof(ProjectDirectoryTypeConverter))]
   //public class ProjectFolder
   //{

   //}

   //public class Project
   //{

   //}
}