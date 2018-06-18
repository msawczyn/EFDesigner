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
      #region ModelClass
      /// <summary>
      /// Returns properties that should be available or hidden in the Visual Studio PropertyGrid for the supplied class,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="modelClass">The model class in question</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a BrowsableAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetHiddenProperties(ModelClass modelClass)
      {
         List<string> result = new List<string>();
         ModelRoot modelRoot = modelClass.ModelRoot;

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            switch (modelRoot.EntityFrameworkCoreVersion)
            {
               case EFCoreVersion.EFCore20:

                  break;
               case EFCoreVersion.EFCore21:

                  break;
            }
         }

         return result;
      }

      /// <summary>
      /// Returns properties that should be readonly or not in the Visual Studio PropertyGrid for the supplied class,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="modelClass">The model class in question</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a ReadOnlyAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetReadOnlyProperties(ModelClass modelClass)
      {
         List<string> result = new List<string>();
         ModelRoot modelRoot = modelClass.ModelRoot;

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            switch (modelRoot.EntityFrameworkCoreVersion)
            {
               case EFCoreVersion.EFCore20:

                  break;
               case EFCoreVersion.EFCore21:

                  break;
            }
         }

         return result;
      }

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

      /// <summary>
      /// Returns properties that should be available or hidden in the Visual Studio PropertyGrid for the supplied attribute,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="modelAttribute">The model attribute in question</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a BrowsableAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetHiddenProperties(ModelAttribute modelAttribute)
      {
         List<string> result = new List<string>();
         ModelRoot modelRoot = modelAttribute.ModelClass.ModelRoot;

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            switch (modelRoot.EntityFrameworkCoreVersion)
            {
               case EFCoreVersion.EFCore20:

                  break;
               case EFCoreVersion.EFCore21:

                  break;
            }
         }

         return result;
      }
      /// <summary>
      /// Returns properties that should be readonly or not in the Visual Studio PropertyGrid for the supplied attribute,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="modelAttribute">The model attribute in question</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a ReadOnlyAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetReadOnlyProperties(ModelAttribute modelAttribute)
      {
         List<string> result = new List<string>();
         ModelRoot modelRoot = modelAttribute.ModelClass.ModelRoot;

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            switch (modelRoot.EntityFrameworkCoreVersion)
            {
               case EFCoreVersion.EFCore20:

                  break;
               case EFCoreVersion.EFCore21:

                  break;
            }
         }

         return result;
      }

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

      /// <summary>
      /// Returns properties that should be available or hidden in the Visual Studio PropertyGrid for the supplied class,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="modelRoot">The model root in question</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a BrowsableAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetHiddenProperties(ModelRoot modelRoot)
      {
         List<string> result = new List<string>();

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            switch (modelRoot.EntityFrameworkCoreVersion)
            {
               case EFCoreVersion.EFCore20:

                  break;
               case EFCoreVersion.EFCore21:

                  break;
            }
         }

         return result;
      }

      /// <summary>
      /// Returns properties that should be readonly or not in the Visual Studio PropertyGrid for the supplied attribute,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="modelRoot">The model root in question</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a ReadOnlyAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetReadOnlyProperties(ModelRoot modelRoot)
      {
         List<string> result = new List<string>();

         if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            switch (modelRoot.EntityFrameworkCoreVersion)
            {
               case EFCoreVersion.EFCore20:

                  break;
               case EFCoreVersion.EFCore21:

                  break;
            }
         }

         return result;
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

      #endregion ModelRoot
   }
}
