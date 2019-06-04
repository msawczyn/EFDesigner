using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   public interface IDynamicCanDelete
   {
      bool CanDelete();
   }

   public interface IDynamicCanCopy
   {
      bool CanCopy();
   }

   public interface IDynamicCanPaste
   {
      bool CanPaste();
   }
}
