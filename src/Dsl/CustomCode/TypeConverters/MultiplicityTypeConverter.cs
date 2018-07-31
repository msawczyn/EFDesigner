using System;
using System.ComponentModel;
using System.Globalization;

namespace Sawczyn.EFDesigner.EFModel
{
   public class MultiplicityTypeConverter : TypeConverter
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
         return sourceType == typeof(string);
      }

      /// <summary>Returns whether this converter can convert the object to the specified type, using the specified context.</summary>
      /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
      /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
         return destinationType == typeof(Multiplicity);
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
            if (s.StartsWith("*"))
               return Multiplicity.ZeroMany;
            //if (s.StartsWith("1..*"))
            //   return Multiplicity.OneMany;
            if (s.StartsWith("0..1"))
               return Multiplicity.ZeroOne;
            if (s.StartsWith("1"))
               return Multiplicity.One;
         }

         return base.ConvertFrom(context, culture, value);
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
