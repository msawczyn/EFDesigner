using System.Collections.Generic;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel
{
   public class IdentityHelper
   {
      private readonly ModelRoot modelRoot;

      public IdentityHelper(ModelRoot modelRoot)
      {
         this.modelRoot = modelRoot;
      }

      private Dictionary<string, string> _identityActors;
      public Dictionary<string, string> IdentityActors
      {
         get
         {
            return _identityActors 
                ?? (_identityActors = new Dictionary<string, string>
                                          {
                                             {"IdentityUser", Leaf("IdentityUser")},
                                             {"IdentityRole", Leaf("IdentityRole")},
                                             {"IdentityUserClaim", Leaf("IdentityUserClaim")},
                                             {"IdentityUserLogin", Leaf("IdentityUserLogin")},
                                             {"IdentityUserRole", Leaf("IdentityUserRole")},
                                          });

            string Leaf(string className)
            {
               return modelRoot.Classes.FirstOrDefault(c => c.Name == className)?.MostDerivedClasses()?.FirstOrDefault()?.Name ?? className;
            }
         }
      }

      public string RealizedIdentityClassName(ModelClass identityClass)
      {
         return RealizedIdentityClassName(identityClass.Name);
      }

      public string RealizedIdentityClassName(string identityClassName)
      {
         switch (identityClassName)
         {
            case "IdentityDbContext":
               return  $"{modelRoot.IdentityNamespace}.IdentityDbContext<"
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUser"])}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityRole"])}, "
                     + $"{modelRoot.IdentityKeyType}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserLogin"])}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserRole"])}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserClaim"])}>";

            case "IdentityRole":
               return  $"{modelRoot.IdentityNamespace}.IdentityRole<"
                     + $"{modelRoot.IdentityKeyType}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserRole"])}>";

            case "IdentityUser":
               return  $"{modelRoot.IdentityNamespace}.IdentityUser<"
                     + $"{modelRoot.IdentityKeyType}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserLogin"])}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserRole"])}, "
                     + $"{RealizedIdentityClassName(IdentityActors["IdentityUserClaim"])}>";

            case "IdentityUserClaim":
               return $"{modelRoot.IdentityNamespace}.IdentityUserClaim<{modelRoot.IdentityKeyType}>";

            case "IdentityUserLogin":
               return $"{modelRoot.IdentityNamespace}.IdentityUserLogin<{modelRoot.IdentityKeyType}>";

            case "IdentityUserRole":
               return $"{modelRoot.IdentityNamespace}.IdentityUserRole<{modelRoot.IdentityKeyType}>";
         }

         return identityClassName;
      }
   }
}
