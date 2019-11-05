using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel {
   public class Namespaces
   {
      private readonly ModelRoot modelRoot;

      public Namespaces(ModelRoot modelRoot)
      {
         this.modelRoot = modelRoot;
      }

      // ReSharper disable once UnusedMember.Global
      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/Namespace.DisplayName", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/Namespace.Category", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/Namespace.Description", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
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

      [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityNamespace.DisplayName", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
      [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/EntityNamespace.Description", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
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