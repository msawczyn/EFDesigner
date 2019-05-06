using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public abstract class DropProcessor
   {
      protected readonly Store store;

      protected DropProcessor(Store store)
      {
         this.store = store;
      }
   }
}
