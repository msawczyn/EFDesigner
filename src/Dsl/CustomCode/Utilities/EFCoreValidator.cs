using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling;

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

         // for later

         //ModelRoot modelRoot = element.ModelRoot;
         //Store store = modelRoot.Store;
         //List<string> errorMessages = new List<string>();

         //if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         //{

         //}

         //return errorMessages;
      }

      public static void RemoveHiddenProperties(PropertyDescriptorCollection propertyDescriptors, ModelClass element)
      {
         ModelRoot modelRoot = element.ModelRoot;
         for (int index = 0; index < propertyDescriptors.Count; index++)
         {
            bool shouldRemove = false;
            switch (propertyDescriptors[index].Name)
            {
               case "IsOwned":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EF6;
                  break;

               case "IsComplexType":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;
                  break;
            }

            if (shouldRemove)
               propertyDescriptors.Remove(propertyDescriptors[index--]);
         }
      }

      #endregion ModelClass

      #region ModelAttribute

      public static IEnumerable<string> GetErrors(ModelAttribute element)
      {
         ModelRoot modelRoot = element.ModelClass.ModelRoot;
         List<string> errorMessages = new List<string>();

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            if (ModelAttribute.SpatialTypes.Contains(element.Type))
               errorMessages.Add($"{element.Type} {element.ModelClass.Name}.{element.Name}: EFCore does not (yet) support spatial types");
         }

         return errorMessages;
      }

      public static void RemoveHiddenProperties(PropertyDescriptorCollection propertyDescriptors, ModelAttribute element)
      {
         // for later

         //ModelRoot modelRoot = element.ModelClass.ModelRoot;

         //for (int index = 0; index < propertyDescriptors.Count; index++)
         //{
         //   bool shouldRemove = false;
         //   switch (propertyDescriptors[index].Name)
         //   {
         //      default:
         //         break;
         //   }

         //   if (shouldRemove)
         //      propertyDescriptors.Remove(propertyDescriptors[index--]);
         //}
      }

      #endregion ModelAttribute

      #region Association

      public static IEnumerable<string> GetErrors(Association element)
      {
         ModelRoot modelRoot = element.Source.ModelRoot;
         List<string> errorMessages = new List<string>();

         if (modelRoot.EntityFrameworkVersion > EFVersion.EF6)
         {
            if ((element.SourceMultiplicity == Multiplicity.ZeroMany) &&
                (element.TargetMultiplicity == Multiplicity.ZeroMany))
               errorMessages.Add($"EFCore does not support many-to-many associations (found one between {element.Source.Name} and {element.Target.Name})");
         }

         return errorMessages;
      }

      public static void RemoveHiddenProperties(PropertyDescriptorCollection propertyDescriptors, Association element)
      {
         // for later

         //ModelRoot modelRoot = element.Source.ModelRoot;

         //for (int index = 0; index < propertyDescriptors.Count; index++)
         //{
         //   bool shouldRemove = false;
         //   switch (propertyDescriptors[index].Name)
         //   {
         //      default:
         //         break;
         //   }

         //   if (shouldRemove)
         //      propertyDescriptors.Remove(propertyDescriptors[index--]);
         //}
      }

      #endregion Association

      #region ModelRoot

      public static IEnumerable<string> GetErrors(ModelRoot element)
      {
         ModelRoot modelRoot = element;
         Store store = modelRoot.Store;
         List<string> errorMessages = new List<string>();

         if (modelRoot.EntityFrameworkVersion > EFVersion.EF6)
         {
            if (element.InheritanceStrategy != CodeStrategy.TablePerHierarchy)
               errorMessages.Add("EFCore currently only supports Table-Per-Hierarchy inheritance strategy.");
         }

         foreach (Association association in store.ElementDirectory.AllElements.OfType<Association>().ToList())
            errorMessages.AddRange(GetErrors(association));

         foreach (ModelClass modelClass in store.ElementDirectory.AllElements.OfType<ModelClass>().ToList())
         {
            errorMessages.AddRange(GetErrors(modelClass));

            foreach (ModelAttribute modelAttribute in modelClass.Attributes)
               errorMessages.AddRange(GetErrors(modelAttribute));
         }

         return errorMessages;
      }

      /// <summary>
      /// Called by TypeDescriptors to determine what should be shown in a property editor. Removing a property hides
      /// it from the property editor in Visual Studio, nothing more.
      /// </summary>
      /// <param name="propertyDescriptors"></param>
      /// <param name="element"></param>
      public static void RemoveHiddenProperties(PropertyDescriptorCollection propertyDescriptors, ModelRoot element)
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

               case "EntityFrameworkCoreVersion":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EF6;
                  break;

               case "DatabaseType":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;
                  break;

               case "InheritanceStrategy":
                  shouldRemove = modelRoot.EntityFrameworkVersion == EFVersion.EFCore;
                  break;
            }

            if (shouldRemove)
               propertyDescriptors.Remove(propertyDescriptors[index--]);
         }

      }

      #endregion ModelRoot

   }
}
