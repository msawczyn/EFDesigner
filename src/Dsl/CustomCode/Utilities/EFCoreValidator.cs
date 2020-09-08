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

#endregion ModelRoot

   }
}
