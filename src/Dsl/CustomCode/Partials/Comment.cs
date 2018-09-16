using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class Comment
   {
      private string GetShortTextValue()
      {
         return Text.Truncate(50);
      }
   }
}
