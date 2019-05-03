using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EFModel.AssemblyProcessor
{
   public class AssemblyProcessor
   {
      private readonly string assemblyDirectory;
      private readonly string assemblyPath;

      public AssemblyProcessor(string filepath) : this(AssemblyName.GetAssemblyName(filepath))
      {
      }

      public AssemblyProcessor(AssemblyName assemblyName)
      {
         assemblyPath = assemblyName.CodeBase;
         if (assemblyPath.StartsWith(@"file:\"))
            assemblyPath = assemblyPath.Substring(6);

         assemblyDirectory = Path.GetDirectoryName(assemblyPath);

         AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;

         try
         {
            // find the first DbContext defined in the assembly. First try EF6
            Assembly loadedAssembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            ContextTypes = GetLoadableTypes(loadedAssembly).Where(t => DerivesFrom(t, "System.Data.Entity.DbContext")).ToList();

            // if couldn't find one, try EFCore
            if (!ContextTypes.Any())
               ContextTypes = GetLoadableTypes(loadedAssembly).Where(t => DerivesFrom(t, "Microsoft.EntityFrameworkCore.DbContext")).ToList();
         }
         finally
         {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ResolveAssembly;
         }
      }

      public IEnumerable<Type> ContextTypes { get; }

      public bool Process(Type contextType)
      {
         if (contextType == null)
            return false;

         //  create AppDomain to load the assembly into
         AppDomain workDomain = AppDomain.CreateDomain("workDomain");
         workDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;

         // find EFModel.AssemblyProcessor and load it into that AppDomain so it can do the work on the assembly.
         // 

         workDomain.Load(GetType().Assembly.GetName());

         DbContext context = (DbContext)workDomain.CreateInstanceAndUnwrap(AssemblyName.GetAssemblyName(assemblyPath).FullName,
                                                                           contextType.FullName,
                                                                           false,
                                                                           BindingFlags.Default,
                                                                           null,
                                                                           new object[] { "App=EntityFramework" },
                                                                           null,
                                                                           null);
         ObjectContext objContext = ((IObjectContextAdapter)context).ObjectContext;

         List<EntityType> entityTypes = objContext.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EntityType>().ToList();
         List<NavigationProperty> processed = new List<System.Data.Entity.Core.Metadata.Edm.NavigationProperty>();
         List<string> displays = new List<string>();

         foreach (EntityType entityType in entityTypes.OrderBy(x => x.Name))
         {
            foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties)
            {
               ReadOnlyMetadataCollection<EdmProperty> depProperties = GetDependentProperties(navigationProperty);
               ReadOnlyMetadataCollection<EdmProperty> prnProperties = GetPrincipalProperties(navigationProperty);

               if (processed.Contains(navigationProperty)) continue;

               NavigationProperty inverse = Inverse(navigationProperty);
               processed.Add(inverse);

               string className = navigationProperty.DeclaringType.Name;
               string propertyTypeName = navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many
                                          ? $"IEnumerable<{navigationProperty.ToEndMember.GetEntityType().Name}>"
                                          : navigationProperty.ToEndMember.GetEntityType().Name;
               string propertyName = navigationProperty.Name;
               string cardinality = SplitCamelCase(navigationProperty.ToEndMember.RelationshipMultiplicity.ToString()).ToLower();

               string otherClassName = navigationProperty.ToEndMember.GetEntityType().Name;
               string otherPropertyTypeName = "";
               string otherPropertyName = "";
               string otherCardinality = SplitCamelCase(navigationProperty.FromEndMember.RelationshipMultiplicity.ToString()).ToLower();

               if (inverse != null)
               {
                  otherPropertyTypeName = navigationProperty.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many
                                             ? $"IEnumerable<{navigationProperty.FromEndMember.GetEntityType().Name}>"
                                             : navigationProperty.FromEndMember.GetEntityType().Name;
                  otherPropertyName = inverse.Name;
               }

               string associationTypeDisplay = inverse == null ? "Unidirectional" : "Bidirectional";
               string cardinalityDisplay = $"{cardinality}-to-{otherCardinality}";
               string lhs = $"{propertyTypeName} {className}.{propertyName}";
               string rhs = $"{otherPropertyTypeName} {otherClassName}.{otherPropertyName}";

               string display = $"{associationTypeDisplay} {cardinalityDisplay}: {className} has {cardinality} {otherClassName} as \"{propertyTypeName} {propertyName}\", and {otherClassName} has {(inverse == null ? "no inverse property" : $"{otherCardinality} {className} as \"{otherPropertyTypeName} {otherPropertyName}\"")}";
               displays.Add(display);
            }
         }

         return false;
      }

      ReadOnlyMetadataCollection<EdmProperty> GetDependentProperties(NavigationProperty navProperty)
      {
         if (navProperty == null)
            throw new ArgumentNullException("navProperty");

         ReadOnlyMetadataCollection<ReferentialConstraint> constraints = ((AssociationType)navProperty.RelationshipType).ReferentialConstraints;
         return (constraints.Any())
                   ? ((AssociationType)navProperty.RelationshipType).ReferentialConstraints[0].ToProperties
                   : null;
      }

      ReadOnlyMetadataCollection<EdmProperty> GetPrincipalProperties(NavigationProperty navProperty)
      {
         if (navProperty == null)
            throw new ArgumentNullException("navProperty");

         ReadOnlyMetadataCollection<ReferentialConstraint> constraints = ((AssociationType)navProperty.RelationshipType).ReferentialConstraints;
         return (constraints.Any())
                   ? ((AssociationType)navProperty.RelationshipType).ReferentialConstraints[0].FromProperties
                   : null;
      }

      NavigationProperty Inverse(NavigationProperty navProperty)
      {
         if (navProperty == null)
            return null;

         EntityType toEntity = navProperty.ToEndMember.GetEntityType();
         return toEntity.NavigationProperties
                        .SingleOrDefault(n => Object.ReferenceEquals(n.RelationshipType, navProperty.RelationshipType) &&
                                              !Object.ReferenceEquals(n, navProperty));
      }

      string SplitCamelCase(string input)
      {
         return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
      }
      private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
      {
         if (assembly == null) throw new ArgumentNullException(nameof(assembly));
         try
         {
            return assembly.GetTypes();
         }
         catch (ReflectionTypeLoadException e)
         {
            return e.Types.Where(t => t != null).ToList();
         }
      }

      private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
      {
         Assembly dep = null;

         try
         {
            // Try to load the dependency from the same location as the original assembly.
            dep = Assembly.ReflectionOnlyLoadFrom(Path.Combine(assemblyDirectory,
                                                               args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll"));

            if (dep != null)
               return dep;
         }
         catch (FileNotFoundException)
         {
            dep = null;
         }

         try
         {
            // Try to load from the GAC.
            dep = Assembly.ReflectionOnlyLoad(args.Name);

            if (dep != null)
               return dep;
         }
         catch (FileLoadException)
         {
            dep = null;
         }


         return null;
      }

      private bool DerivesFrom(Type t, string typeName)
      {
         Type type = t;
         while (type.FullName != typeName && type.BaseType != null)
            type = type.BaseType;
         return type.FullName == typeName;
      }
   }
}