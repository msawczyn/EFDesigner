using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ModelEnumExtensions
   {
      public static string GetRelativeFileName(this ModelEnum modelEnum)
      {
         return Path.Combine(modelEnum.OutputDirectory, modelEnum.Name + ".cs");
      }
   }
}
