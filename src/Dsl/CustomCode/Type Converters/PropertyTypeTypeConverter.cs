using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public class PropertyTypeTypeConverter : TypeConverterBase
   {
      /// <summary>
      ///    Returns whether this converter can convert an object of the given type to the type of this converter, using
      ///    the specified context.
      /// </summary>
      /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
      }

      /// <summary>Returns whether this converter can convert the object to the specified type, using the specified context.</summary>
      /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
         return destinationType == typeof(PropertyType) || base.CanConvertTo(context, destinationType);
      }

      /// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
      /// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
      /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
      /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
         if (value is string s)
         {
            switch (s)
            {
               case "Normal":
                  return PropertyType.Normal;

               case "Computed":
                  return PropertyType.Computed;

               case "Database Computed":
                  return PropertyType.DatabaseComputed;
            }
         }

         return base.ConvertFrom(context, culture, value);
      }

      /// <summary>Converts the given value object to the specified type, using the specified context and culture information.</summary>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed. </param>
      /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
      /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to. </param>
      /// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
      /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> parameter is <see langword="null" />. </exception>
      /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
      {
         if (destinationType == typeof(string) && value is PropertyType propertyType)
         {
            switch (propertyType)
            {
               case PropertyType.Normal:
                  return "Normal";
               case PropertyType.Computed:
                  return "Computed";
               case PropertyType.DatabaseComputed:
                  return "Database Computed";
            }
         }

         return base.ConvertTo(context, culture, value, destinationType);
      }

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
         base.GetStandardValues(context);
         List<string> values = new List<string>();

         Store store = GetStore(context.Instance);

         if (store != null)
         {
            ModelRoot modelRoot = store.ModelRoot();

            // Value set changes at EFCore5
            values.AddRange(modelRoot.IsEFCore5Plus
                               ? new[]
                                 {
                                    "Normal"
                                  , "Computed"
                                  , "Database Computed"
                                 }
                               : new[]
                                 {
                                    "Normal"
                                  , "Computed"
                                 });
         }

         return new StandardValuesCollection(values);
      }

      /// <summary>
      ///    Returns whether the collection of standard values returned from
      ///    <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exclusive list of possible values,
      ///    using the specified context.
      /// </summary>
      /// <returns>
      ///    true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> returned from
      ///    <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exhaustive list of possible values;
      ///    false if other values are possible.
      /// </returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
         return true;
      }

      /// <summary>
      ///    Returns whether this object supports a standard set of values that can be picked from a list, using the
      ///    specified context.
      /// </summary>
      /// <returns>
      ///    true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> should be called to find a
      ///    common set of values the object supports; otherwise, false.
      /// </returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
         return true;
      }
   }
}