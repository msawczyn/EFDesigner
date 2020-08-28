using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;
#pragma warning disable 1591

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   ///    Contains EFCore limitations as of v2.1. Review for each new release.
   /// </summary>
   public static class EFCoreValidator
   {
      #region ModelClass

      public static IEnumerable<string> GetErrors(ModelClass element)
      {
         return new string[0];

         // nothing to do here

         //ModelRoot modelRoot = element.ModelRoot;
         //Store store = modelRoot.Store;
         //List<string> errorMessages = new List<string>();

         //if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         //{

         //}

         //return errorMessages;
      }

      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, ModelClass element)
      {
         // nothing to do here

         //ModelRoot modelRoot = element.ModelRoot;
         //for (int index = 0; index < propertyDescriptors.Count; index++)
         //{
         //   bool shouldRemove = false;
         //   switch (propertyDescriptors[index].Name)
         //   {
         //   }

         //   if (shouldRemove)
         //      propertyDescriptors.Remove(propertyDescriptors[index--]);
         //}
      }

      #endregion ModelClass

      #region ModelEnum

      public static IEnumerable<string> GetErrors(ModelEnum element)
      {
         return new string[0];

         // nothing to do here

         //ModelRoot modelRoot = element.ModelRoot;
         //Store store = modelRoot.Store;
         //List<string> errorMessages = new List<string>();

         //if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         //{

         //}

         //return errorMessages;
      }

      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, ModelEnum element)
      {
         // nothing to do here

         //ModelRoot modelRoot = element.ModelRoot;
         //for (int index = 0; index < propertyDescriptors.Count; index++)
         //{
         //   bool shouldRemove = false;
         //   switch (propertyDescriptors[index].Name)
         //   {
         //   }

         //   if (shouldRemove)
         //      propertyDescriptors.Remove(propertyDescriptors[index--]);
         //}
      }

      #endregion ModelEnum

      #region ModelAttribute

      public static IEnumerable<string> GetErrors(ModelAttribute element)
      {
         return new string[0];

         // nothing to do here

         //ModelRoot modelRoot = element.ModelClass.ModelRoot;
         //Store store = modelRoot.Store;
         //List<string> errorMessages = new List<string>();

         //if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         //{

         //}

         //return errorMessages;
      }

      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, ModelAttribute element)
      {
         ModelRoot modelRoot = element.ModelClass.ModelRoot;

         if (!modelRoot.IsEFCore5Plus)
         {
            propertyDescriptors.Remove("DatabaseCollation");
            propertyDescriptors.Remove("PropertyAccessMode");
            propertyDescriptors.Remove("BackingFieldName");
         }
      }

      #endregion ModelAttribute

      #region Association

      public static IEnumerable<string> GetErrors(Association element)
      {
         ModelRoot modelRoot = element.Store.ModelRoot();
         List<string> errorMessages = new List<string>();

         if (modelRoot != null && modelRoot.EntityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus)
         {
            if ((element.SourceMultiplicity == Multiplicity.ZeroMany) &&
                (element.TargetMultiplicity == Multiplicity.ZeroMany))
               errorMessages.Add($"EFCore does not support many-to-many associations (found one between {element.Source.Name} and {element.Target.Name})");
         }

         return errorMessages;
      }

      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, Association element)
      {
         BidirectionalAssociation bidirectionalAssociation = element as BidirectionalAssociation;

         // only show backing field and property access mode for EFCore5+ non-collection associations
         if (!element.Source.ModelRoot.IsEFCore5Plus || element.TargetMultiplicity == Multiplicity.ZeroMany)
         {
            propertyDescriptors.Remove("TargetBackingFieldName");
            propertyDescriptors.Remove("TargetPropertyAccessMode");
         }

         if (!element.Source.ModelRoot.IsEFCore5Plus || bidirectionalAssociation?.SourceMultiplicity == Multiplicity.ZeroMany)
         {
            propertyDescriptors.Remove("SourceBackingFieldName");
            propertyDescriptors.Remove("SourcePropertyAccessMode");
         }

      }

      #endregion Association

      #region ModelRoot

      public static IEnumerable<string> GetErrors(ModelRoot element)
      {
         ModelRoot modelRoot = element;
         Store store = modelRoot.Store;

         List<string> errorMessages = new List<string>();

         foreach (Association association in store.GetAll<Association>().ToList())
            errorMessages.AddRange(GetErrors(association));

         foreach (ModelClass modelClass in store.GetAll<ModelClass>().ToList())
         {
            errorMessages.AddRange(GetErrors(modelClass));

            foreach (ModelAttribute modelAttribute in modelClass.Attributes)
               errorMessages.AddRange(GetErrors(modelAttribute));
         }

         foreach (ModelEnum modelEnum in store.GetAll<ModelEnum>().ToList())
            errorMessages.AddRange(GetErrors(modelEnum));

         return errorMessages;
      }

      /// <summary>
      /// Called by TypeDescriptors to determine what should be shown in a property editor. Removing a property hides
      /// it from the property editor in Visual Studio, nothing more.
      /// </summary>
      /// <param name="propertyDescriptors"></param>
      /// <param name="element"></param>
      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, ModelRoot element)
      {
         ModelRoot modelRoot = element;

         if (!modelRoot.IsEFCore5Plus)
            propertyDescriptors.Remove("DatabaseCollation");

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            propertyDescriptors.Remove("DatabaseInitializerType");
            propertyDescriptors.Remove("AutomaticMigrationsEnabled");
            propertyDescriptors.Remove("ProxyGenerationEnabled");
            propertyDescriptors.Remove("DatabaseType");
            propertyDescriptors.Remove("InheritanceStrategy");

            if (modelRoot.GetEntityFrameworkPackageVersionNum() < 2.1)
               propertyDescriptors.Remove("LazyLoadingEnabled");
         }
      }

      #endregion ModelRoot

   }
}
