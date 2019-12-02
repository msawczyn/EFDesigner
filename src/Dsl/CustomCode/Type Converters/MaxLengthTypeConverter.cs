using System;
using System.ComponentModel;
using System.Globalization;

namespace Sawczyn.EFDesigner.EFModel
{
   class MaxLengthTypeConverter : TypeConverterBase
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
         return destinationType == typeof(int?);
      }

      /// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
      /// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
      /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
      /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
         if (value is string s && !string.IsNullOrWhiteSpace(s))
         {
            if (s.ToLowerInvariant() == "max")
               return ModelAttribute.MAXLENGTH_MAX;

            if (int.TryParse(s, out int val))
               return val;
         }

         return null;
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

         if (value?.ToString()?.ToLowerInvariant() == "max")
            return "max";

         if (value is int i)
         {
            if (i == ModelAttribute.MAXLENGTH_UNDEFINED)
               return null;

            if (i == ModelAttribute.MAXLENGTH_MAX)
               return "max";

            return i.ToString();
         }

         return base.ConvertTo(context, culture, value, destinationType);
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
         return false;
      }

   }
}
