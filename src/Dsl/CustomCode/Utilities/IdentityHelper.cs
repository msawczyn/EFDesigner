using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

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
               return modelRoot.Classes.FirstOrDefault(c => c.Name == className)?.MostDerivedClasses()?.FirstOrDefault()?.Name ?? RealizedIdentityClassName(className);
               //return modelRoot.Classes.FirstOrDefault(c => c.Name == className)?.MostDerivedClasses()?.FirstOrDefault()?.Name ?? className;
            }
         }
      }

      /// <summary>
      /// Returns the name of the generic class with appropriate generic types for the model
      /// </summary>
      /// <param name="identityClass">ModelClass of ASP.NET Identity class</param>
      /// <returns></returns>
      public string RealizedIdentityClassName(ModelClass identityClass)
      {
         return RealizedIdentityClassName(identityClass.Name);
      }

      /// <summary>
      /// Returns the name of the generic class with appropriate generic types for the model
      /// </summary>
      /// <param name="identityClassName">Name of ASP.NET Identity class (without namespace)</param>
      /// <returns></returns>
      public string RealizedIdentityClassName(string identityClassName)
      {
         try
         {
            switch (identityClassName)
            {
               case "IdentityDbContext":
                  return $"{IdentityNamespace}.IdentityDbContext<"
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUser"])}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityRole"])}, "
                       + $"{modelRoot.IdentityKeyType}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserLogin"])}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserRole"])}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserClaim"])}>";

               case "IdentityRole":
                  return $"{IdentityNamespace}.IdentityRole<"
                       + $"{modelRoot.IdentityKeyType}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserRole"])}>";

               case "IdentityUser":
                  return $"{IdentityNamespace}.IdentityUser<"
                       + $"{modelRoot.IdentityKeyType}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserLogin"])}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserRole"])}, "
                       + $"{RealizedIdentityClassName(IdentityActors["IdentityUserClaim"])}>";

               case "IdentityUserClaim":
                  return $"{IdentityNamespace}.IdentityUserClaim<{modelRoot.IdentityKeyType}>";

               case "IdentityUserLogin":
                  return $"{IdentityNamespace}.IdentityUserLogin<{modelRoot.IdentityKeyType}>";

               case "IdentityUserRole":
                  return $"{IdentityNamespace}.IdentityUserRole<{modelRoot.IdentityKeyType}>";
            }
         }
         // ReSharper disable once EmptyGeneralCatchClause
         catch // doesn't matter why ... just spit back what we were passed
         {
         }

         return identityClassName;
      }

      internal void SetIdentityKeyType(string keyType)
      {
         // set these no matter if we're IdentityDbContext or not
         // if not, they won't be used. But if (somehow) later this becomes IdentityDbContext, they'll be correct

         // Note that this is primarily for display purposes. We could simply look at ModelRoot to determine what the value should be,
         // but that would mean we'd have to change Type to a calculated property, and that's really messy.
         // Since Identity classes aren't user-editable, we're ok in assuming they'll be correct since this is the only way they can
         // be changed.

         try
         {
            // use sub-transactions since the rules will fire when they're committed (FireTime = TimeToFire.LocalCommit)
            using (Transaction transaction = modelRoot.Store.TransactionManager.BeginTransaction("BypassReadOnlyChecks_On"))
            {
               modelRoot.BypassReadOnlyChecks = true;
               transaction.Commit();
            }

            SetKeyType("IdentityRole", "Id", keyType);
            SetKeyType("IdentityUser", "Id", keyType);
            SetKeyType("IdentityLogin", "UserId", keyType);
            SetKeyType("IdentityUserLogin", "UserId", keyType);
            SetKeyType("IdentityUserRole", "UserId", keyType);
            SetKeyType("IdentityUserRole", "RoleId", keyType);
            SetKeyType("IdentityUserClaim", "Id", keyType);
            SetKeyType("IdentityUserClaim", "UserId", keyType);
         }
         finally
         {
            using (Transaction transaction = modelRoot.Store.TransactionManager.BeginTransaction("BypassReadOnlyChecks_Off"))
            {
               modelRoot.BypassReadOnlyChecks = false;
               transaction.Commit();
            }
         }

         void SetKeyType(string _className, string _attributeName, string _keyType)
         {
            using (Transaction transaction = modelRoot.Store.TransactionManager.BeginTransaction("SetKeyType"))
            {
               ModelClass     identityClass = modelRoot.Classes.Find(c => c.Name == _className);
               ModelAttribute keyAttribute  = identityClass?.Attributes.Find(a => a.Name == _attributeName);

               if (keyAttribute != null)
                  keyAttribute.Type = _keyType;
               transaction.Commit();
            }
         }
      }

      public string IdentityNamespace
      {
         get
         {
            // TODO: Not all EFCore identity classes have the same namespace
            return modelRoot.EntityFrameworkVersion == EFVersion.EF6
                      ? "Microsoft.AspNet.Identity.EntityFramework"
                      : "Microsoft.AspNetCore.Identity.EntityFrameworkCore";
         }
      }

      internal void FixupIdentityAssociations()
      {
         if (!modelRoot.IsIdentityDbContext)
            return;

         ModelClass identityRole = modelRoot.Classes.Find(c => c.Name == "IdentityRole");
         ModelClass identityUserRole = modelRoot.Classes.Find(c => c.Name == "IdentityUserRole");
         ModelClass identityUser = modelRoot.Classes.Find(c => c.Name == "IdentityUser");
         ModelClass identityUserLogin = modelRoot.Classes.Find(c => c.Name == "IdentityUserLogin");
         ModelClass identityUserClaim = modelRoot.Classes.Find(c => c.Name == "IdentityUserClaim");

         Retarget(identityRole, identityUserRole, "Users");
         Retarget(identityUser, identityUserRole, "Roles");
         Retarget(identityUser, identityUserLogin, "Logins");
         Retarget(identityUser, identityUserClaim, "Claims");
      }

      private void Retarget(ModelClass source, ModelClass target, string propertyName)
      {
         UnidirectionalAssociation association = modelRoot.Store.ElementDirectory.AllElements.OfType<UnidirectionalAssociation>()
                                                      .FirstOrDefault(a => a.Source == source &&
                                                                           a.TargetPropertyName == propertyName);

         ModelClass actualTarget = target.MostDerivedClasses().SingleOrDefault();

         if (association != null && actualTarget != null && association.Target != actualTarget)
            association.Target = actualTarget;
         else if (association == null)
         {
            // ReSharper disable once UnusedVariable
            UnidirectionalAssociation unidirectionalAssociation = 
               new UnidirectionalAssociation(source.ModelRoot.Store,
                                             new[]
                                             {
                                                new RoleAssignment(UnidirectionalAssociation.UnidirectionalSourceDomainRoleId, source),
                                                new RoleAssignment(UnidirectionalAssociation.UnidirectionalTargetDomainRoleId, actualTarget)
                                             },
                                             new[]
                                             {
                                                new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, Multiplicity.One),
                                                new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, Multiplicity.ZeroMany),
                                                new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, propertyName) //,
                                                //new PropertyAssignment(Association.TargetSummaryDomainPropertyId, targetSummary),
                                                //new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, targetDescription)
                                             });
         }
      }


      public static readonly string[] IdentityBaseClasses = { "IdentityRole", "IdentityUser", "IdentityUserClaim", "IdentityUserLogin", "IdentityUserRole" };
   }
}
