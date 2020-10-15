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
      #region ModelAttribute

      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, ModelAttribute element)
      {
         ModelRoot modelRoot = element.ModelClass.ModelRoot;

         for (int index = 0; index < propertyDescriptors.Count; index++)
         {
            bool shouldRemove = false;

            switch (propertyDescriptors[index].Name)
            {
               case "PersistencePoint":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EF6;

                  break;

                  // add more as needed
            }

            if (shouldRemove)
               propertyDescriptors.Remove(propertyDescriptors[index--]);
         }
      }

      #endregion ModelAttribute

      #region Association

      public static IEnumerable<string> GetErrors(Association element)
      {
         ModelRoot modelRoot = element.Store.ModelRoot();
         List<string> errorMessages = new List<string>();

         if (modelRoot?.EntityFrameworkVersion == EFVersion.EFCore && modelRoot?.IsEFCore5Plus == false)
         {
            if ((element.SourceMultiplicity == Multiplicity.ZeroMany) && (element.TargetMultiplicity == Multiplicity.ZeroMany))
               errorMessages.Add($"EFCore does not support many-to-many associations (found one between {element.Source.Name} and {element.Target.Name})");
         }

         return errorMessages;
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

         return errorMessages;
      }

      /// <summary>
      ///    Called by TypeDescriptors to determine what should be shown in a property editor. Removing a property hides
      ///    it from the property editor in Visual Studio, nothing more.
      /// </summary>
      /// <param name="propertyDescriptors"></param>
      /// <param name="element"></param>
      public static void AdjustEFCoreProperties(PropertyDescriptorCollection propertyDescriptors, ModelRoot element)
      {
         ModelRoot modelRoot = element;

         for (int index = 0; index < propertyDescriptors.Count; index++)
         {
            bool shouldRemove = false;

            switch (propertyDescriptors[index].Name)
            {
               case "DatabaseInitializerType":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;

                  break;

               case "AutomaticMigrationsEnabled":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;

                  break;

               case "ProxyGenerationEnabled":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;

                  break;

               case "DatabaseType":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;

                  break;

               case "InheritanceStrategy":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus;

                  break;

               case "LazyLoadingEnabled":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore && modelRoot.GetEntityFrameworkPackageVersionNum() < 2.1;

                  break;
            }

            if (shouldRemove)
               propertyDescriptors.Remove(propertyDescriptors[index--]);
         }
      }

      #endregion ModelRoot
   }
}