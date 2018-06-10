using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   ///    Contains EFCore limitations as of v2.0. Review for each new release.
   /// </summary>
   public static class EFCoreValidator
   {
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

      public static IEnumerable<string> GetErrors(ModelAttribute element)
      {
         ModelRoot modelRoot = element.ModelClass.ModelRoot;
         List<string> errorMessages = new List<string>();

         if (modelRoot.EntityFrameworkVersion > EFVersion.EF6)
         {
            if (ModelAttribute.SpatialTypes.Contains(element.Type))
               errorMessages.Add($"{element.Type} {element.ModelClass.Name}.{element.Name}: EFCore does not (yet) support spatial types");
         }

         return errorMessages;
      }

      public static IEnumerable<string> GetErrors(Association element)
      {
         ModelRoot modelRoot = element.Source.ModelRoot;
         List<string> errorMessages = new List<string>();

         if (modelRoot.EntityFrameworkVersion > EFVersion.EF6)
         {
            if (element.SourceMultiplicity == Multiplicity.ZeroMany &&
                element.TargetMultiplicity == Multiplicity.ZeroMany)
               errorMessages.Add($"EFCore does not support many-to-many associations (found one between {element.Source.Name} and {element.Target.Name})");
         }

         return errorMessages;
      }

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
   }
}
