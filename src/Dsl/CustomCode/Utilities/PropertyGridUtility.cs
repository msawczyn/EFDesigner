using System.ComponentModel;
using System.Reflection;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Utilities
{
   internal static class PropertyGridUtility
   {
      private static readonly BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance;

      public static void FixupBrowsability<T>(ModelRoot modelRoot) 
      {
         // Hide EFCore properties that aren't appropriate to the version chosen
         foreach (string hiddenProperty in EFCoreValidator.GetBrowsableProperties<T>(modelRoot.EntityFrameworkVersion, modelRoot.EntityFrameworkCoreVersion, true))
            SetBrowsable<T>(hiddenProperty, false);
         foreach (string visibleProperty in EFCoreValidator.GetBrowsableProperties<T>(modelRoot.EntityFrameworkVersion, modelRoot.EntityFrameworkCoreVersion, false))
            SetBrowsable<T>(visibleProperty, true);
      }

      public static void FixupReadability<T>(ModelRoot modelRoot)
      {
         // Set EFCore properties to readonly if appropriate to the version chosen
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties<T>(modelRoot.EntityFrameworkVersion, modelRoot.EntityFrameworkCoreVersion, true))
            SetReadOnly<ModelRoot>(readonlyProperty, false);
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties<T>(modelRoot.EntityFrameworkVersion, modelRoot.EntityFrameworkCoreVersion, false))
            SetReadOnly<ModelRoot>(readonlyProperty, true);
      }

      /// <summary>
      ///    Set the Browsable property.
      ///    NOTE: Be sure to decorate the property with [Browsable(true)]
      /// </summary>
      /// <param name="propertyName">Name of the variable</param>
      /// <param name="value">Browsable Value</param>
      public static void SetBrowsable<T>(string propertyName, bool value)
      {
         if (propertyName != null)
         {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
            BrowsableAttribute browsableAttribute = (BrowsableAttribute)propertyDescriptor?.Attributes[typeof(BrowsableAttribute)];
            if (browsableAttribute != null)
            {
               FieldInfo browsableField = browsableAttribute.GetType().GetField("browsable", bindingFlags);
               if (browsableField != null)
                  browsableField.SetValue(browsableAttribute, value);
            }
         }
      }

      public static void SetReadOnly<T>(string propertyName, bool value)
      {
         if (propertyName != null)
         {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
            ReadOnlyAttribute readonlyAttribute = (ReadOnlyAttribute)propertyDescriptor?.Attributes[typeof(ReadOnlyAttribute)];
          
            if (readonlyAttribute != null)
            {
               FieldInfo readonlyField = readonlyAttribute.GetType().GetField("isReadOnly", bindingFlags);
               if (readonlyField != null)
                  readonlyField.SetValue(readonlyAttribute, value);
            }
         }
      }
   }
}
