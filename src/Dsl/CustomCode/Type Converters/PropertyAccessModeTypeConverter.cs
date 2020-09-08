using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public class PropertyAccessModeTypeConverter : TypeConverterBase
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
         return destinationType == typeof(PropertyAccessMode);
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
               case "Field":
                  return PropertyAccessMode.Field;

               case "FieldDuringConstruction":
                  return PropertyAccessMode.FieldDuringConstruction;

               case "PreferField":
                  return PropertyAccessMode.PreferField;

               case "PreferFieldDuringConstruction":
                  return PropertyAccessMode.PreferFieldDuringConstruction;

               case "PreferProperty":
                  return PropertyAccessMode.PreferProperty;

               case "Property":
                  return PropertyAccessMode.Property;
            }
         }

         return base.ConvertFrom(context, culture, value);
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

         // Try to get a store from the current context  
         // "context.Instance"  returns the element(s) that are currently selected i.e. whose values are being shown in the property grid.   
         // Note that the user could have selected multiple objects, in which case context.Instance will be an array.  
         IHasStore currentElement = context.Instance is object[] objects && objects.Length > 0
                                       ? objects[0] as IHasStore
                                       : context.Instance as IHasStore;

         Store store = GetStore(currentElement);

         if (store != null)
         {
            ModelRoot modelRoot = store.ModelRoot();

            // Value set changes at EFCore3
            if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
               return new StandardValuesCollection(values);

            if (modelRoot.GetEntityFrameworkPackageVersionNum() < 3)
            {
               values.AddRange(new[]
                               {
                                  "Field"
                                , "FieldDuringConstruction"
                                , "Property"
                               });
            }
            else
            {
               values.AddRange(new[]
                               {
                                  "Field"
                                , "FieldDuringConstruction"
                                , "PreferField"
                                , "PreferFieldDuringConstruction"
                                , "PreferProperty"
                                , "Property"
                               });
            }
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