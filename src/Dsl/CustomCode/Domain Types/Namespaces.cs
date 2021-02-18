using System;
using System.ComponentModel;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Encapsulates values of the current Namespace settings in the ModelRoot
   /// </summary>
   [Serializable]
   public class Namespaces: IHasStore
   {
      private readonly ModelRoot modelRoot;

      /// <summary>
      /// Exposes the Store object.  Store is a complete model.  Stores contain both the domain data  and the model data for all the domain models in a model.
      /// </summary>
      [Browsable(false)]
      public Store Store => modelRoot?.Store;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="modelRoot">DomainClass ModelRoot</param>
      public Namespaces(ModelRoot modelRoot)
      {
         this.modelRoot = modelRoot;
      }

      /// <summary>
      /// Namespace value for the generated DbContext-derived object
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/Namespace.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/Namespace.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/Namespace.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string DbContext
      {
         get
         {
            return modelRoot.Namespace;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.Namespace = value;
               t.Commit();
            }
         }
      }

      /// <summary>
      /// Namespace value for the generated entity classes
      /// </summary>
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityNamespace.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityNamespace.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityNamespace.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Entity
      {
         get
         {
            return string.IsNullOrWhiteSpace(modelRoot.EntityNamespace)
                      ? modelRoot.Namespace
                      : modelRoot.EntityNamespace;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.EntityNamespace = string.IsNullOrWhiteSpace(value) || value == modelRoot.Namespace
                                              ? null
                                              : value;

               t.Commit();
            }
         }
      }

      /// <summary>
      /// Namespace value for the generated enumerations
      /// </summary>
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumNamespace.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumNamespace.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EnumNamespace.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Enumeration
      {
         get
         {
            return string.IsNullOrWhiteSpace(modelRoot.EnumNamespace)
                      ? modelRoot.Namespace
                      : modelRoot.EnumNamespace;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.EnumNamespace = string.IsNullOrWhiteSpace(value) || value == modelRoot.Namespace
                                            ? null
                                            : value;

               t.Commit();
            }
         }
      }

      /// <summary>
      /// Namespace value for the generated non-persistent objects
      /// </summary>
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructNamespace.DisplayName", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructNamespace.Category", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/StructNamespace.Description", typeof(EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      public string Struct
      {
         get
         {
            return string.IsNullOrWhiteSpace(modelRoot.StructNamespace)
                      ? modelRoot.Namespace
                      : modelRoot.StructNamespace;
         }
         set
         {
            using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
            {
               modelRoot.StructNamespace = string.IsNullOrWhiteSpace(value) || value == modelRoot.Namespace
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

}