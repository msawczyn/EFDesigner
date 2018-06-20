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

         //ModelRoot modelRoot = element.ModelRoot;
         //Store store = modelRoot.Store;
         //List<string> errorMessages = new List<string>();

         //if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         //{

         //}

         //return errorMessages;
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

      #endregion ModelRoot

      internal static void RemoveHiddenProperties(PropertyDescriptorCollection propertyDescriptors, ModelRoot modelRoot)
      {
         for (int index = 0; index < propertyDescriptors.Count; index++)
         {
            HideWhenAttribute hideWhenAttribute = propertyDescriptors[index]
                                                 .Attributes.OfType<HideWhenAttribute>()
                                                 .FirstOrDefault();

            if (hideWhenAttribute?.ShouldHide(modelRoot) == true)
               propertyDescriptors.Remove(propertyDescriptors[index--]);
         }
      }

   }
}
