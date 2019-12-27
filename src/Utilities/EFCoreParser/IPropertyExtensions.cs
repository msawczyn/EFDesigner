using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreParser
{
   public static class IPropertyExtensions
   {
      public static bool IsShadowProperty(this IProperty prop)
      {
         return prop.IsShadowProperty;
      }
   }
}
