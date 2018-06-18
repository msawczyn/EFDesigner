using System.ComponentModel;
using System.Reflection;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Utilities
{
   internal static class PropertyGridUtility
   {
      private static readonly BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance;

      public static void FixupBrowsability(ModelRoot element) 
      {
         // Hide EFCore properties that aren't appropriate to the version chosen
         foreach (string hiddenProperty in EFCoreValidator.GetBrowsableProperties(element, true))
            SetBrowsable<ModelRoot>(hiddenProperty, false);
         foreach (string visibleProperty in EFCoreValidator.GetBrowsableProperties(element, false))
            SetBrowsable<ModelRoot>(visibleProperty, true);
      }
      public static void FixupBrowsability(ModelClass element) 
      {
         // Hide EFCore properties that aren't appropriate to the version chosen
         foreach (string hiddenProperty in EFCoreValidator.GetBrowsableProperties(element, true))
            SetBrowsable<ModelClass>(hiddenProperty, false);
         foreach (string visibleProperty in EFCoreValidator.GetBrowsableProperties(element, false))
            SetBrowsable<ModelClass>(visibleProperty, true);
      }
      public static void FixupBrowsability(ModelAttribute element) 
      {
         // Hide EFCore properties that aren't appropriate to the version chosen
         foreach (string hiddenProperty in EFCoreValidator.GetBrowsableProperties(element, true))
            SetBrowsable<ModelAttribute>(hiddenProperty, false);
         foreach (string visibleProperty in EFCoreValidator.GetBrowsableProperties(element, false))
            SetBrowsable<ModelAttribute>(visibleProperty, true);
      }

      public static void FixupReadability(ModelRoot element)
      {
         // Set EFCore properties to readonly if appropriate to the version chosen
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties(element, true))
            SetBrowsable<ModelRoot>(readonlyProperty, false);
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties(element, false))
            SetBrowsable<ModelRoot>(readonlyProperty, true);
      }
      public static void FixupReadability(ModelClass element)
      {
         // Set EFCore properties to readonly if appropriate to the version chosen
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties(element, true))
            SetBrowsable<ModelClass>(readonlyProperty, false);
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties(element, false))
            SetBrowsable<ModelClass>(readonlyProperty, true);
      }
      public static void FixupReadability(ModelAttribute element)
      {
         // Set EFCore properties to readonly if appropriate to the version chosen
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties(element, true))
            SetBrowsable<ModelAttribute>(readonlyProperty, false);
         foreach (string readonlyProperty in EFCoreValidator.GetReadOnlyProperties(element, false))
            SetBrowsable<ModelAttribute>(readonlyProperty, true);
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
               FieldInfo browsableField = browsableAttribute.GetType().GetField("Browsable", bindingFlags);
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
               FieldInfo readonlyField = readonlyAttribute.GetType().GetField("IsReadOnly", bindingFlags);
               if (readonlyField != null)
                  readonlyField.SetValue(readonlyAttribute, value);
            }
         }
      }
   }
}
