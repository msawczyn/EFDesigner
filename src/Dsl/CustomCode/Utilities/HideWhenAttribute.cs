using System;

using Sawczyn.EFDesigner.EFModel;

namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// Tags a model element property to indicate when it should be hidden in the property editor
   /// based on the EF version and/or EF Core version of the model. Evaluated in various type descriptors.
   /// </summary>
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
   public class HideWhenAttribute : Attribute
   {
      public HideWhenAttribute(EFVersion efVersion, EFCoreVersion efCoreVersion)
      {
         EFVersion     = efVersion;
         EFCoreVersion = efCoreVersion;
      }

      public HideWhenAttribute(EFVersion efVersion)
      {
         EFVersion     = efVersion;
         EFCoreVersion = null;
      }

      public EFVersion EFVersion { get; set; }
      public EFCoreVersion? EFCoreVersion { get; set; }

      public bool ShouldHide(ModelRoot modelRoot)
      {
         bool result = (modelRoot.EntityFrameworkVersion == EFVersion) && 
                ((EFCoreVersion == null) || (modelRoot.EntityFrameworkCoreVersion == EFCoreVersion));

         if (!result)
         {

         }

         return result;
      }
   }
}
