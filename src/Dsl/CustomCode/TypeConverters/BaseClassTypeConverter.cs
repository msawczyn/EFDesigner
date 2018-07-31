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
         List<ModelClass> selectedClasses = GetSelectedElements(context.Instance).OfType<ModelClass>().Distinct().ToList();

         Store store = GetStore(context.Instance);
         List<string> validNames = store.ElementDirectory.FindElements<ModelClass>().Where(e => !selectedClasses.Contains(e)).OrderBy(c => c.Name).Select(c => c.Name).ToList();

         return new StandardValuesCollection(validNames);
      }
   }
}
