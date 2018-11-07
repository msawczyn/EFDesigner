using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
         string current = ((ModelClass)((ClassShape)context?.Instance)?.Subject)?.Name;

         Store store = GetStore(context.Instance);

         List<string> validNames = store.ElementDirectory
                                        .FindElements<ModelClass>()
                                        .Where(e => e.Name != current)
                                        .OrderBy(c => c.Name)
                                        .Select(c => c.Name)
                                        .ToList();
         validNames.Insert(0, null);

         return new StandardValuesCollection(validNames);
      }
   }
}
