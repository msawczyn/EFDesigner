using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Utilities
{
   internal static class PropertyGridUtility
   {
      private static readonly BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance;

      /// <summary>
      ///    Set the Browsable property.
      ///    NOTE: Be sure to decorate the property with [Browsable(true)]
      /// </summary>
      /// <param name="propertyName">Name of the variable</param>
      /// <param name="value">Browsable Value</param>
      public static void SetBrowsable<T>(string propertyName, bool value)
      {
         PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
         BrowsableAttribute browsableAttribute = (BrowsableAttribute)propertyDescriptor.Attributes[typeof(BrowsableAttribute)];
         FieldInfo          browsableField     = browsableAttribute.GetType().GetField("Browsable", bindingFlags);
         browsableField.SetValue(browsableAttribute, value);
      }

      public static void SetReadOnly<T>(string propertyName, bool value)
      {
         PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
         ReadOnlyAttribute  readonlyAttribute  = (ReadOnlyAttribute)propertyDescriptor.Attributes[typeof(ReadOnlyAttribute)];
         FieldInfo          readonlyField      = readonlyAttribute.GetType().GetField("IsReadOnly", bindingFlags);
         readonlyField.SetValue(readonlyAttribute, value);
      }
   }
}
