using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Sawczyn.EFDesigner.EFModel
{
   public class NavigationEndpointTypeConverter : TypeConverterBase
   {
      /// <summary>
      ///    Returns a collection of standard values for the data type this type converter is designed for when provided
      ///    with a format context.
      /// </summary>
      /// <param name="context">
      ///    An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context
      ///    that can be used to extract additional information about the environment from which this converter is invoked. This
      ///    parameter or properties of this parameter can be null.
      /// </param>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of
      ///    valid values, or null if the data type does not support a standard set of values.
      /// </returns>
      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
         // "context.Instance"  returns the element(s) that are currently selected i.e. whose values are being shown in the property grid.   
         // Note that the user could have selected multiple objects, in which case context.Instance will be an array. 
         if (context.Instance.GetType().IsArray) return null;

         Association association = context.Instance as Association;
         if (association == null) return null;

         List<string> values = new List<string> {association.Source.Name, association.Target.Name};
         values.Sort();
         return new StandardValuesCollection(values);
      }

      /// <summary>
      ///    Returns whether the collection of standard values returned from
      ///    <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exclusive list of possible values,
      ///    using the specified context.
      /// </summary>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <returns>
      ///    true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> returned from
      ///    <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exhaustive list of possible values;
      ///    false if other values are possible.
      /// </returns>
      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
         return true;
      }

      /// <summary>
      ///    Returns whether this object supports a standard set of values that can be picked from a list, using the
      ///    specified context.
      /// </summary>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <returns>
      ///    true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> should be called to find a
      ///    common set of values the object supports; otherwise, false.
      /// </returns>
      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
         return true;
      }

      /// <summary>
      ///    Returns whether this converter can convert an object of the given type to the type of this converter, using the
      ///    specified context.
      /// </summary>
      /// <param name="context">
      ///    An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.
      /// </param>
      /// <param name="sourceType">
      ///    A <see cref="T:System.Type" /> that represents the type you want to convert from.
      /// </param>
      /// <returns>
      ///    true if this converter can perform the conversion; otherwise, false.
      /// </returns>
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         return sourceType == typeof(string);
      }

      /// <summary>
      ///    Converts the given object to the type of this converter, using the specified context and culture information.
      /// </summary>
      /// <param name="context">
      ///    An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.
      /// </param>
      /// <param name="culture">
      ///    The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.
      /// </param>
      /// <param name="value">
      ///    The <see cref="T:System.Object" /> to convert.
      /// </param>
      /// <returns>
      ///    An <see cref="T:System.Object" /> that represents the converted value.
      /// </returns>
      /// <exception cref="T:System.NotSupportedException">
      ///    The conversion cannot be performed.
      /// </exception>
      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
         return value?.ToString();
      }
   }
}