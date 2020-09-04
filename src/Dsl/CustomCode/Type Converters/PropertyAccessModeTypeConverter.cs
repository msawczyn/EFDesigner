using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public class PropertyAccessModeTypeConverter : EnumConverter
   {
      /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EnumConverter" /> class for the given type.</summary>
      /// <param name="type">A <see cref="T:System.Type" /> that represents the type of enumeration to associate with this enumeration converter. </param>
      public PropertyAccessModeTypeConverter() : base(typeof(PropertyAccessMode)) { }

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

         Store store = currentElement?.Store;
         if (store != null)
         {
            ModelRoot modelRoot = store.ModelRoot();

            // Value set changes at EFCore3
            if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
               return new StandardValuesCollection(values);

            if (modelRoot.GetEntityFrameworkPackageVersionNum() < 3)
               values.AddRange(new[]
                               {
                                  "Field"
                                , "FieldDuringConstruction"
                                , "Property"
                               });
            else
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

         return new StandardValuesCollection(values);
      }

   }
}
