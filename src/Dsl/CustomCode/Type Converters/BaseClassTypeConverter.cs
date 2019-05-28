using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public class BaseClassTypeConverter : TypeConverterBase
   {
      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
         return true;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
         return true;
      }

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
         Store store = GetStore(context.Instance);

         List<string> invalidOptions = new List<string>();

         if (context.Instance is Array shapeArray)
         {
            invalidOptions.AddRange(shapeArray.OfType<ClassShape>()
                                              .Where(s => s.Subject is ModelClass)
                                              .Select(s => (s.Subject as ModelClass).Name));
         }
         else
         {
            string targetClassName = ((context.Instance as ClassShape)?.Subject as ModelClass)?.Name;
            if (targetClassName != null)
               invalidOptions.Add(targetClassName);
         }

         List<string> validNames = store.ElementDirectory
                                        .FindElements<ModelClass>()
                                        .Where(e => !invalidOptions.Contains(e.Name))
                                        .OrderBy(c => c.Name)
                                        .Select(c => c.Name)
                                        .ToList();
         validNames.Insert(0, null);

         return new StandardValuesCollection(validNames);
      }
   }
}
