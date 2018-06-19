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
      /// <summary>
      /// Returns properties that should be available or hidden in the Visual Studio PropertyGrid for the supplied attribute,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="version">The EF version in question</param>
      /// <param name="coreVersion">The EF Core version in question, if any</param>
      /// <param name="shouldHide">If true, return properties that should be hidden. Otherwise, return properties that should be made visible (if they aren't already).</param>
      /// <returns>
      /// <p>
      /// If shouldHide is true, returns a collection of property names that should be hidden (depending on EF version).
      /// If shouldHide is false, returns a collection of property names that should be shown (depending on EF version).</p>
      /// <p>
      /// Note that these properties must already have a BrowsableAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetBrowsableProperties<T>(EFVersion version, EFCoreVersion? coreVersion, bool shouldHide)
      {
         if (typeof(T) == typeof(ModelRoot))
            return GetBrowsableModelRootProperties(version, coreVersion, shouldHide);

         // add additional classes that have interesting properties here
         return new string[0];
      }

      /// <summary>
      /// Returns properties that should be readonly or not in the Visual Studio PropertyGrid for the supplied class,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="version">The EF version in question</param>
      /// <param name="coreVersion">The EF Core version in question, if any</param>
      /// <param name="shouldMakeReadOnly">If true, return properties that should be made read-only. Otherwise, return properties that should be made writable (if they aren't already).</param>
      /// <returns>
      /// <p>If EF Core is targeted, returns a collection of property names that should be hidden (depending on EF Core version).
      /// If EF6 is targeted, returns all possible hidden properties so they may be unhidden.</p>
      /// <p>Note that these properties must already have a ReadOnlyAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetReadOnlyProperties<T>(EFVersion version, EFCoreVersion? coreVersion, bool shouldMakeReadOnly)
      {
         if (typeof(T) == typeof(ModelRoot))
            return GetReadOnlyModelRootProperties(version, coreVersion, shouldMakeReadOnly);

         // add additional classes that have interesting properties here
         return new string[0];
      }
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

      /// <summary>
      /// Returns properties that should be available or hidden in the Visual Studio PropertyGrid for the supplied attribute,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="version">The EF version in question</param>
      /// <param name="coreVersion">The EF Core version in question, if any</param>
      /// <param name="shouldHide">If true, return properties that should be hidden. Otherwise, return properties that should be made visible (if they aren't already).</param>
      /// <returns>
      /// <p>
      /// If shouldHide is true, returns a collection of property names that should be hidden (depending on EF version).
      /// If shouldHide is false, returns a collection of property names that should be shown (depending on EF version).</p>
      /// <p>
      /// Note that these properties must already have a BrowsableAttribute on them.</p>
      /// </returns>
      public static IEnumerable<string> GetBrowsableModelRootProperties(EFVersion version, EFCoreVersion? coreVersion, bool shouldHide)
      {

         switch (version)
         {
            case EFVersion.EF6:
               return shouldHide
                         ? new[]
                           {"EFCoreVersion"}
                         : new[]
                           {
                              "DatabaseInitializerType",
                              "AutomaticMigrationsEnabled"
                           };

            default:
               if (coreVersion == null)
                  return new string[0];

               switch (coreVersion.Value)
               {
                  case EFCoreVersion.EFCore20:
                     return shouldHide
                               ? new[]
                                 {
                                       "DatabaseInitializerType",
                                       "AutomaticMigrationsEnabled"
                                 }
                               : new[]
                                 {"EFCoreVersion"};

                  default:
                     return shouldHide
                               ? new[]
                                 {
                                       "DatabaseInitializerType",
                                       "AutomaticMigrationsEnabled"
                                 }
                               : new[]
                                 {"EFCoreVersion"};
               }
         }
      }

      /// <summary>
      /// Returns properties that should be readonly or not in the Visual Studio PropertyGrid for the supplied attribute,
      /// depending on whether EFCore is targeted or not.
      /// </summary>
      /// <param name="version">The EF version in question</param>
      /// <param name="coreVersion">The EF Core version in question, if any</param>
      /// <param name="shouldMakeReadOnly">If true, return properties that should be readonly. Otherwise, return properties that should be made writable (if they aren't already).</param>
      /// <returns>
      /// <p>
      /// If shouldMakeReadOnly is true, returns a collection of property names that should be readonly (depending on EF version).
      /// If shouldMakeReadOnly is false, returns a collection of property names that should be writable (depending on EF version).</p>
      /// <p>
      /// Note that these properties must already have a ReadOnlyAttribute on them.</p>
      /// </returns>
      private static IEnumerable<string> GetReadOnlyModelRootProperties(EFVersion version, EFCoreVersion? coreVersion, bool shouldMakeReadOnly)
      {
         switch (version)
         {
            case EFVersion.EF6:
               return shouldMakeReadOnly
                         ? new string[0]
                         : new[]
                           {
                              "InheritanceStrategy"
                           };

            default:
               switch (coreVersion)
               {
                  case EFCoreVersion.EFCore20:
                     return shouldMakeReadOnly
                               ? new[]
                                 {
                                    "InheritanceStrategy"
                                 }
                               : new string[0];

                  default:
                     return shouldMakeReadOnly
                               ? new[]
                                 {
                                    "InheritanceStrategy"
                                 }
                               : new string[0];
               }
         }
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
