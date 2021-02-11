using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.4
      // Copyright (c) 2017-2021 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public abstract class EFCoreModelGenerator : EFModelGenerator
      {
         protected EFCoreModelGenerator(GeneratedTextTransformation host) : base(host) { }

         public static string[] SpatialTypes
         {
            get
            {
               return new[]
                      {
                         "Geometry"
                       , "GeometryPoint"
                       , "GeometryLineString"
                       , "GeometryPolygon"
                       , "GeometryCollection"
                       , "GeometryMultiPoint"
                       , "GeometryMultiLineString"
                       , "GeometryMultiPolygon"
                      };
            }
         }

         public override void Generate(Manager manager)
         {
            // Entities
            string fileNameMarker = string.IsNullOrEmpty(modelRoot.FileNameMarker) ? string.Empty : $".{modelRoot.FileNameMarker}";

            foreach (ModelClass modelClass in modelRoot.Classes.Where(e => e.GenerateCode))
            {
               ClearIndent();
               manager.StartNewFile(Path.Combine(modelClass.EffectiveOutputDirectory, $"{modelClass.Name}{fileNameMarker}.cs"));
               WriteClass(modelClass);
            }

            // Enums
            foreach (ModelEnum modelEnum in modelRoot.Enums.Where(e => e.GenerateCode))
            {
               ClearIndent();
               manager.StartNewFile(Path.Combine(modelEnum.EffectiveOutputDirectory, $"{modelEnum.Name}{fileNameMarker}.cs"));
               WriteEnum(modelEnum);
            }

            // Context
            ClearIndent();
            manager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}{fileNameMarker}.cs"));
            WriteDbContext();

            // Context factory
            if (modelRoot.GenerateDbContextFactory)
            {
               ClearIndent();
               manager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}Factory{fileNameMarker}.cs"));
               WriteDbContextFactory();
            }
         }

         protected void WriteDbContextFactory()
         {
            Output("using System;");
            Output("using System.Collections.Generic;");
            Output("using System.Linq;");
            Output("using System.Text;");
            Output("using System.Threading.Tasks;");
            NL();

            Output("using Microsoft.EntityFrameworkCore;");
            Output("using Microsoft.EntityFrameworkCore.Design;");
            NL();

            BeginNamespace(modelRoot.Namespace);

            Output("/// <summary>");
            Output("/// A factory for creating derived DbContext instances. Implement this interface to enable design-time services for context ");
            Output("/// types that do not have a public default constructor. At design-time, derived DbContext instances can be created in order ");
            Output("/// to enable specific design-time experiences such as Migrations. Design-time services will automatically discover ");
            Output("/// implementations of this interface that are in the startup assembly or the same assembly as the derived context.");
            Output("/// </summary>");

            Output($"public class {modelRoot.EntityContainerName}Factory: IDesignTimeDbContextFactory<{modelRoot.EntityContainerName}>");
            Output("{");
            Output("/// <summary>Creates a new instance of a derived context.</summary>");
            Output("/// <param name=\"args\"> Arguments provided by the design-time service. </param>");
            Output($"/// <returns> An instance of <see cref=\"{modelRoot.Namespace}.{modelRoot.EntityContainerName}\" />.</returns>");
            Output($"public {modelRoot.EntityContainerName} CreateDbContext(string[] args)");
            Output("{");
            Output($"DbContextOptionsBuilder<{modelRoot.EntityContainerName}> optionsBuilder = new DbContextOptionsBuilder<{modelRoot.EntityContainerName}>();");
            NL();

            Output($"// Please provide the {modelRoot.EntityContainerName}.ConfigureOptions(optionsBuilder) in the partial class as");
            Output("//    public static void ConfigureOptions(DbContextOptionsBuilder optionsBuilder) {{ ... }}");
            Output("// If you have custom initialization for the context, you can then consolidate the code by defining the CustomInit partial as");
            Output("//    partial void CustomInit(DbContextOptionsBuilder optionsBuilder) => ConfigureOptions(optionsBuilder);");
            Output($"{modelRoot.EntityContainerName}.ConfigureOptions(optionsBuilder);");
            Output($"return new {modelRoot.EntityContainerName}(optionsBuilder.Options);");
            Output("}");
            Output("}");
            EndNamespace(modelRoot.Namespace);
         }

         protected override List<string> GetAdditionalUsingStatements()
         {
            List<string> result = new List<string>();
            List<string> attributeTypes = modelRoot.Classes.SelectMany(c => c.Attributes).Select(a => a.Type).Distinct().ToList();

            if (attributeTypes.Intersect(modelRoot.SpatialTypes).Any())
               result.Add("using NetTopologySuite.Geometries;");

            return result;
         }

         protected virtual void ConfigureModelClasses(List<string> segments, ModelClass[] classesWithTables, List<string> foreignKeyColumns, List<Association> visited)
         {
            foreach (ModelClass modelClass in modelRoot.Classes.OrderBy(x => x.Name))
            {
               segments.Clear();
               foreignKeyColumns.Clear();
               NL();

               if (modelClass.IsDependentType)
               {
                  segments.Add($"modelBuilder.Owned<{modelClass.FullName}>()");
                  Output(segments);
                  continue;
               }

               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               foreach (ModelAttribute transient in modelClass.Attributes.Where(x => !x.Persistent))
                  segments.Add($"Ignore(t => t.{transient.Name})");

               //if (modelRoot.InheritanceStrategy == CodeStrategy.TablePerConcreteType && modelClass.Superclass != null)
               //   segments.Add("Map(x => x.MapInheritedProperties())");

               if (classesWithTables.Contains(modelClass))
               {
                  if (modelClass.IsQueryType)
                  {
                     Output($"// There is no storage defined for {modelClass.Name} because its IsQueryType value is");
                     Output($"// set to 'true'. Please provide the {modelRoot.FullName}.Get{modelClass.Name}SqlQuery() method in the partial class.");
                     Output("// ");
                     Output($"// private string Get{modelClass.Name}SqlQuery()");
                     Output("// {");
                     Output($"//    return the defining SQL query that pulls all the properties for {modelClass.FullName}");
                     Output("// }");

                     segments.Add($"ToSqlQuery(Get{modelClass.Name}SqlQuery())");
                  }
                  else
                     ConfigureTable(segments, modelClass);
               }

               if (segments.Count > 1 || modelClass.IsDependentType)
                  Output(segments);

               // attribute level
               ConfigureModelAttributes(segments, modelClass);

               bool hasDefinedConcurrencyToken = modelClass.AllAttributes.Any(x => x.IsConcurrencyToken);

               if (!hasDefinedConcurrencyToken && modelClass.EffectiveConcurrency == ConcurrencyOverride.Optimistic)
                  Output($@"modelBuilder.Entity<{modelClass.FullName}>().Property<byte[]>(""Timestamp"").IsConcurrencyToken();");

               // Navigation endpoints are distingished as Source and Target. They are also distinguished as Principal
               // and Dependent. So how do these map to each other? Short answer: they don't - they're orthogonal concepts.
               // Source and Target are accidents of where the user started drawing the association, and help define where the
               // properties are in unidirectional associations. Principal and Dependent define where the foreign keys go in 
               // the persistence mechanism.

               // What matters to code generation is the Principal and Dependent classifications, so we focus on those. 
               // In the case of 1-1 or 0/1-0/1, it's situational, so the user has to tell us.
               // In all other cases, we can tell by the cardinalities of the associations.

               // navigation properties
               List<string> declaredShadowProperties = new List<string>();

               if (!modelClass.IsDependentType)
               {
                  ConfigureUnidirectionalAssociations(modelClass, visited, foreignKeyColumns, declaredShadowProperties);
                  ConfigureBidirectionalAssociations(modelClass, visited, foreignKeyColumns, declaredShadowProperties);
               }
            }
         }

         protected virtual void ConfigureTable(List<string> segments, ModelClass modelClass)
         {
            string tableName = string.IsNullOrEmpty(modelClass.TableName) ? modelClass.Name : modelClass.TableName;
            string schema = string.IsNullOrEmpty(modelClass.DatabaseSchema) || modelClass.DatabaseSchema == modelClass.ModelRoot.DatabaseSchema ? string.Empty : ", \"{modelClass.DatabaseSchema}\"";

            segments.Add($"ToTable(\"{tableName}\"{schema})");

            // primary key code segments must be output last, since HasKey returns a different type
            List<ModelAttribute> identityAttributes = modelClass.IdentityAttributes.ToList();

            if (identityAttributes.Count == 1)
               segments.Add($"HasKey(t => t.{identityAttributes[0].Name})");
            else if (identityAttributes.Count > 1)
               segments.Add($"HasKey(t => new {{ t.{string.Join(", t.", identityAttributes.Select(ia => ia.Name))} }})");
         }

         protected virtual void ConfigureModelAttributes(List<string> segments, ModelClass modelClass)
         {
            foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.Persistent && !SpatialTypes.Contains(x.Type)))
            {
               segments.Clear();

               segments.AddRange(GatherModelAttributeSegments(modelAttribute));

               if (segments.Any())
               {
                  segments.Insert(0, $"modelBuilder.Entity<{modelClass.FullName}>()");
                  segments.Insert(1, $"Property(t => t.{modelAttribute.Name})");

                  Output(segments);
               }

               if (modelAttribute.Indexed && !modelAttribute.IsIdentity)
               {
                  segments.Clear();

                  segments.Add($"modelBuilder.Entity<{modelClass.FullName}>().HasIndex(t => t.{modelAttribute.Name})");

                  if (modelAttribute.IndexedUnique)
                     segments.Add("IsUnique()");

                  Output(segments);
               }
            }
         }

         protected virtual List<string> GatherModelAttributeSegments(ModelAttribute modelAttribute)
         {
            List<string> segments = new List<string>();

            if (modelAttribute.MaxLength != null && modelAttribute.MaxLength > 0)
               segments.Add($"HasMaxLength({modelAttribute.MaxLength})");

            if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
               segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

            if (!modelAttribute.AutoProperty)
            {
               segments.Add($"HasField(\"{modelAttribute.BackingFieldName}\")");
               segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{modelAttribute.PropertyAccessMode})");
            }

            if (!string.IsNullOrEmpty(modelAttribute.ColumnType) && modelAttribute.ColumnType.ToLowerInvariant() != "default")
            {
               if (modelAttribute.ColumnType.ToLowerInvariant() == "varchar" || modelAttribute.ColumnType.ToLowerInvariant() == "nvarchar" || modelAttribute.ColumnType.ToLowerInvariant() == "char")
                  segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}({(modelAttribute.MaxLength > 0 ? modelAttribute.MaxLength.ToString() : "max")})\")");
               else
                  segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}\")");
            }

            if (modelAttribute.IsConcurrencyToken)
               segments.Add("IsRowVersion()");

            if (modelAttribute.IsIdentity)
            {
               segments.Add(modelAttribute.IdentityType == IdentityType.AutoGenerated
                               ? "ValueGeneratedOnAdd()"
                               : "ValueGeneratedNever()");
            }

            if (modelAttribute.Required)
               segments.Add("IsRequired()");

            return segments;
         }

         protected virtual void WriteDbContext()
         {
            List<string> segments = new List<string>();
            ModelClass[] classesWithTables = null;

            // Note: TablePerConcreteType not yet available, but it doesn't hurt for it to be here since they shouldn't make it past the designer's validations
            switch (modelRoot.InheritanceStrategy)
            {
               case CodeStrategy.TablePerType:
               case CodeStrategy.TablePerConcreteType:
                  classesWithTables = modelRoot.Classes
                                               .Where(mc => (!mc.IsDependentType || !string.IsNullOrEmpty(mc.TableName))
                                                         && mc.GenerateCode)
                                               .OrderBy(x => x.Name)
                                               .ToArray();
                  break;

               //case CodeStrategy.TablePerConcreteType:
               //   classesWithTables = modelRoot.Classes
               //                                .Where(mc => (!mc.IsDependentType || !string.IsNullOrEmpty(mc.TableName)) 
               //                                          && !mc.IsAbstract 
               //                                          && mc.GenerateCode)
               //                                .OrderBy(x => x.Name)
               //                                .ToArray();
               //   break;

               case CodeStrategy.TablePerHierarchy:
                  classesWithTables = modelRoot.Classes
                                               .Where(mc => (!mc.IsDependentType || !string.IsNullOrEmpty(mc.TableName))
                                                         && mc.Superclass == null
                                                         && mc.GenerateCode)
                                               .OrderBy(x => x.Name)
                                               .ToArray();
                  break;
            }

            Output("using System;");
            Output("using System.Collections.Generic;");
            Output("using System.Linq;");
            Output("using System.ComponentModel.DataAnnotations.Schema;");
            Output("using Microsoft.EntityFrameworkCore;");
            NL();

            BeginNamespace(modelRoot.Namespace);

            WriteDbContextComments();

            string baseClass = string.IsNullOrWhiteSpace(modelRoot.BaseClass) ? "Microsoft.EntityFrameworkCore.DbContext" : modelRoot.BaseClass;
            Output($"{modelRoot.EntityContainerAccess.ToString().ToLower()} partial class {modelRoot.EntityContainerName} : {baseClass}");
            Output("{");

            if (classesWithTables?.Any() == true)
               WriteDbSets();

            WriteContextConstructors();
            if (!modelRoot.GenerateDbContextFactory)
               WriteOnConfiguring(segments);
            WriteOnModelCreate(segments, classesWithTables);

            Output("}");

            EndNamespace(modelRoot.Namespace);
         }

         protected void WriteDbSets()
         {
            Output("#region DbSets");
            PluralizationService pluralizationService = ModelRoot.PluralizationService;

            foreach (ModelClass modelClass in modelRoot.Classes.Where(x => !x.IsDependentType).OrderBy(x => x.Name))
            {
               string dbSetName;

               if (!string.IsNullOrEmpty(modelClass.DbSetName))
                  dbSetName = modelClass.DbSetName;
               else
               {
                  dbSetName = pluralizationService?.IsSingular(modelClass.Name) == true
                                 ? pluralizationService.Pluralize(modelClass.Name)
                                 : modelClass.Name;
               }

               if (!string.IsNullOrEmpty(modelClass.Summary))
               {
                  NL();
                  Output("/// <summary>");
                  WriteCommentBody($"Repository for {modelClass.FullName} - {modelClass.Summary}");
                  Output("/// </summary>");
               }

               Output($"{modelRoot.DbSetAccess.ToString().ToLower()} virtual Microsoft.EntityFrameworkCore.DbSet<{modelClass.FullName}> {dbSetName} {{ get; set; }}");
            }

            NL();
            Output("#endregion DbSets");
            NL();
         }

         protected void WriteContextConstructors()
         {
            if (!string.IsNullOrEmpty(modelRoot.ConnectionString) || !string.IsNullOrEmpty(modelRoot.ConnectionStringName))
            {
               string connectionString = string.IsNullOrEmpty(modelRoot.ConnectionString)
                                            ? $"Name={modelRoot.ConnectionStringName}"
                                            : modelRoot.ConnectionString;

               Output("/// <summary>");
               Output("/// Default connection string");
               Output("/// </summary>");
               Output($"public static string ConnectionString {{ get; set; }} = @\"{connectionString}\";");
               NL();
            }

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(DbContextOptions<{modelRoot.EntityContainerName}> options) : base(options)");
            Output("{");
            Output("}");
            NL();

            Output("partial void CustomInit(DbContextOptionsBuilder optionsBuilder);");
            NL();
         }

         protected void WriteOnConfiguring(List<string> segments)
         {
            Output("/// <inheritdoc />");
            Output("protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");
            Output("{");

            segments.Clear();

            if (modelRoot.GetEntityFrameworkPackageVersionNum() >= 2.1 && modelRoot.LazyLoadingEnabled)
               segments.Add("UseLazyLoadingProxies()");

            if (segments.Any())
            {
               segments.Insert(0, "optionsBuilder");

               Output(segments);
               NL();
            }

            Output("CustomInit(optionsBuilder);");
            Output("}");
            NL();
         }

         protected virtual void WriteOnModelCreate(List<string> segments, ModelClass[] classesWithTables)
         {
            Output("partial void OnModelCreatingImpl(ModelBuilder modelBuilder);");
            Output("partial void OnModelCreatedImpl(ModelBuilder modelBuilder);");
            NL();

            Output("/// <inheritdoc />");
            Output("protected override void OnModelCreating(ModelBuilder modelBuilder)");
            Output("{");
            Output("base.OnModelCreating(modelBuilder);");
            Output("OnModelCreatingImpl(modelBuilder);");
            NL();

            if (!string.IsNullOrEmpty(modelRoot.DatabaseSchema))
               Output($"modelBuilder.HasDefaultSchema(\"{modelRoot.DatabaseSchema}\");");

            List<Association> visited = new List<Association>();
            List<string> foreignKeyColumns = new List<string>();

            ConfigureModelClasses(segments, classesWithTables, foreignKeyColumns, visited);

            NL();

            Output("OnModelCreatedImpl(modelBuilder);");
            Output("}");
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected virtual void ConfigureBidirectionalAssociations(ModelClass modelClass
                                                                  , List<Association> visited
                                                                  , List<string> foreignKeyColumns
                                                                  , List<string> declaredShadowProperties)
         {
            WriteBidirectionalNonDependentAssociations(modelClass, visited, foreignKeyColumns);
            WriteBidirectionalDependentAssociations(modelClass, $"modelBuilder.Entity<{modelClass.FullName}>()", visited);
         }

         protected virtual void WriteBidirectionalDependentAssociations(ModelClass sourceInstance, string baseSegment, List<Association> visited)
         {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (BidirectionalAssociation association in Association.GetLinksToTargets(sourceInstance)
                                                                        .OfType<BidirectionalAssociation>()
                                                                        .Where(x => x.Persistent && x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               List<string> segments = new List<string>();
               string separator = sourceInstance.ModelRoot.ShadowKeyNamePattern == ShadowKeyPattern.TableColumn ? "" : "_";

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                  {
                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add($"ToTable(\"{(string.IsNullOrEmpty(association.Target.TableName) ? association.Target.Name : association.Target.TableName)}\")");
                     Output(segments);

                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add($"WithOwner(\"{association.SourcePropertyName}\")");
                     segments.Add($"HasForeignKey(\"{association.SourcePropertyName}{separator}Id\")");
                     Output(segments);

                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add($"Property<{modelRoot.DefaultIdentityType}>(\"Id\")");

                     Output(segments);

                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add("HasKey(\"Id\")");

                     Output(segments);

                     WriteBidirectionalDependentAssociations(association.Target, $"{baseSegment}.OwnsMany(p => p.{association.TargetPropertyName})", visited);

                     break;
                  }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                  {
                     segments.Add(baseSegment);
                     segments.Add($"OwnsOne(p => p.{association.TargetPropertyName})");
                     segments.Add($"WithOwner(p => p.{association.SourcePropertyName})");
                     Output(segments);

                     if (!string.IsNullOrEmpty(association.Target.TableName))
                     {
                        segments.Add(baseSegment);
                        segments.Add($"OwnsOne(p => p.{association.TargetPropertyName})");
                        segments.Add($"ToTable(\"{association.Target.TableName}\")");
                        Output(segments);
                     }

                     foreach (ModelAttribute modelAttribute in association.Target.AllAttributes)
                     {
                        segments.Add($"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName}).Property(p => p.{modelAttribute.Name})");

                        if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
                           segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

                        if (modelAttribute.Required)
                           segments.Add("IsRequired()");

                        if (segments.Count > 1)
                           Output(segments);

                        segments.Clear();
                     }

                     WriteBidirectionalDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})", visited);

                     break;
                  }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                  {
                     segments.Add(baseSegment);
                     segments.Add($"OwnsOne(p => p.{association.TargetPropertyName})");
                     segments.Add($"WithOwner(p => p.{association.SourcePropertyName})");
                     Output(segments);

                     if (!string.IsNullOrEmpty(association.Target.TableName))
                     {
                        segments.Add(baseSegment);
                        segments.Add($"OwnsOne(p => p.{association.TargetPropertyName})");
                        segments.Add($"ToTable(\"{association.Target.TableName}\")");
                        Output(segments);
                     }

                     foreach (ModelAttribute modelAttribute in association.Target.AllAttributes)
                     {
                        segments.Add($"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName}).Property(p => p.{modelAttribute.Name})");

                        if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
                           segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

                        if (modelAttribute.Required)
                           segments.Add("IsRequired()");

                        if (segments.Count > 1)
                           Output(segments);

                        segments.Clear();
                     }

                     WriteBidirectionalDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})", visited);

                     break;
                  }
               }
            }
         }

         protected virtual void WriteBidirectionalNonDependentAssociations(ModelClass modelClass, List<Association> visited, List<string> foreignKeyColumns)
         {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (BidirectionalAssociation association in Association.GetLinksToTargets(modelClass)
                                                                        .OfType<BidirectionalAssociation>()
                                                                        .Where(x => x.Persistent && !x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               List<string> segments = new List<string>();
               bool required = false;

               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                     break;
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"WithMany(p => p.{association.SourcePropertyName})");

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName)
                                             ? $"{association.Target.Name}_{association.SourcePropertyName}_x_{association.Source.Name}_{association.TargetPropertyName}"
                                             : association.JoinTableName;

                        segments.Add($"UsingEntity(x => x.ToTable(\"{tableMap}\"))");
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"WithOne(p => p.{association.SourcePropertyName})");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"WithOne(p => p.{association.SourcePropertyName})");

                     break;
               }

               string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

               if (!string.IsNullOrEmpty(foreignKeySegment))
                  segments.Add(foreignKeySegment);

               WriteSourceDeleteBehavior(association, segments);

               if (required
                && (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                 || association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
                  segments.Add("IsRequired()");

               Output(segments);
            }
         }

         protected virtual void WriteTargetDeleteBehavior(UnidirectionalAssociation association, List<string> segments)
         {
            if (!association.Source.IsDependentType
             && !association.Target.IsDependentType
             && (association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal))
            {
               DeleteAction deleteAction = association.SourceRole == EndpointRole.Principal
                                              ? association.SourceDeleteAction
                                              : association.TargetDeleteAction;

               switch (deleteAction)
               {
                  case DeleteAction.None:
                     segments.Add("OnDelete(DeleteBehavior.Restrict)");

                     break;

                  case DeleteAction.Cascade:
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");

                     break;
               }
            }
         }

         protected virtual void WriteSourceDeleteBehavior(BidirectionalAssociation association, List<string> segments)
         {
            if (!association.Source.IsDependentType
             && !association.Target.IsDependentType
             && (association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal))
            {
               DeleteAction deleteAction = association.SourceRole == EndpointRole.Principal
                                              ? association.SourceDeleteAction
                                              : association.TargetDeleteAction;

               switch (deleteAction)
               {
                  case DeleteAction.None:
                     segments.Add("OnDelete(DeleteBehavior.Restrict)");

                     break;

                  case DeleteAction.Cascade:
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");

                     break;
               }
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected virtual void ConfigureUnidirectionalAssociations(ModelClass modelClass
                                                                  , List<Association> visited
                                                                  , List<string> foreignKeyColumns
                                                                  , List<string> declaredShadowProperties)
         {
            WriteUnidirectionalNonDependentAssociations(modelClass, visited, foreignKeyColumns);
            WriteUnidirectionalDependentAssociations(modelClass, $"modelBuilder.Entity<{modelClass.FullName}>()", visited);
         }

         protected virtual void WriteUnidirectionalDependentAssociations(ModelClass sourceInstance, string baseSegment, List<Association> visited)
         {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (UnidirectionalAssociation association in Association.GetLinksToTargets(sourceInstance)
                                                                         .OfType<UnidirectionalAssociation>()
                                                                         .Where(x => x.Persistent && x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               List<string> segments = new List<string>();
               string separator = sourceInstance.ModelRoot.ShadowKeyNamePattern == ShadowKeyPattern.TableColumn ? "" : "_";

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                  {
                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add($"WithOwner(\"{association.Source.Name}_{association.TargetPropertyName}\")");
                     segments.Add($"HasForeignKey(\"{association.Source.Name}_{association.TargetPropertyName}{separator}Id\")");
                     Output(segments);

                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add($"Property<{modelRoot.DefaultIdentityType}>(\"Id\")");

                     Output(segments);

                     segments.Add(baseSegment);
                     segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                     segments.Add("HasKey(\"Id\")");

                     Output(segments);

                     WriteUnidirectionalDependentAssociations(association.Target, $"{baseSegment}.OwnsMany(p => p.{association.TargetPropertyName})", visited);

                     break;
                  }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                  {
                     foreach (ModelAttribute modelAttribute in association.Target.AllAttributes)
                     {
                        segments.Add($"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName}).Property(p => p.{modelAttribute.Name})");

                        if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
                           segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

                        if (modelAttribute.Required)
                           segments.Add("IsRequired()");

                        Output(segments);
                     }

                     WriteUnidirectionalDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})", visited);

                     break;
                  }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                  {
                     foreach (ModelAttribute modelAttribute in association.Target.AllAttributes)
                     {
                        segments.Add($"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName}).Property(p => p.{modelAttribute.Name})");

                        if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
                           segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

                        if (modelAttribute.Required)
                           segments.Add("IsRequired()");

                        Output(segments);
                     }

                     WriteUnidirectionalDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})", visited);

                     break;
                  }
               }
            }
         }

         protected virtual void WriteUnidirectionalNonDependentAssociations(ModelClass modelClass, List<Association> visited, List<string> foreignKeyColumns)
         {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (UnidirectionalAssociation association in Association.GetLinksToTargets(modelClass)
                                                                         .OfType<UnidirectionalAssociation>()
                                                                         .Where(x => x.Persistent && !x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               List<string> segments = new List<string>();
               bool required = false;

               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                     break;
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add("WithMany()");

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName)
                                             ? $"{association.Target.Name}_x_{association.Source.Name}_{association.TargetPropertyName}"
                                             : association.JoinTableName;

                        segments.Add($"UsingEntity(x => x.ToTable(\"{tableMap}\"))");
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add("WithOne()");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add("WithOne()");

                     break;
               }

               string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

               if (!string.IsNullOrEmpty(foreignKeySegment))
                  segments.Add(foreignKeySegment);
               else if (association.Is(Sawczyn.EFDesigner.EFModel.Multiplicity.One, Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                     || association.Is(Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne, Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne))
                  segments.Add($"HasForeignKey<{association.Dependent.FullName}>()");

               WriteTargetDeleteBehavior(association, segments);

               if (required
                && (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                 || association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
                  segments.Add("IsRequired()");

               Output(segments);
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected virtual string CreateForeignKeySegment(Association association, List<string> foreignKeyColumns)
         {
            List<string> foreignKeys = GetForeignKeys(association, foreignKeyColumns).ToList();

            if (!foreignKeys.Any()) // only happens if many-to-many
               return null;

            // 1-1, 1-0/1 and 0/1-0/1  
            bool dependentClassDesignationRequired = association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany
                                                  && association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany;
            string result = string.Join(",", foreignKeys);

            if (foreignKeys.First().StartsWith("\""))
            {
               // foreign keys are shadow properties
               result = dependentClassDesignationRequired
                           ? $"HasForeignKey(\"{association.Dependent.Name}\", {result})"
                           : $"HasForeignKey({result})";
            }
            else
            {
               // foreign keys are real properties
               result = foreignKeys.Count == 1
                           ? $"k => {result}"
                           : $"k => new {{ {result} }}";

               result = dependentClassDesignationRequired
                           ? $"HasForeignKey<{association.Dependent.FullName}>({result})"
                           : $"HasForeignKey({result})";
            }

            return result;
         }

         protected virtual IEnumerable<string> GetForeignKeys(Association association, List<string> foreignKeyColumns)
         {
            // final collection of foreign key property names, real or shadow
            // shadow properties will be double quoted, real properties won't
            IEnumerable<string> result = new List<string>();

            // foreign key definitions always go in the table representing the Dependent end of the association
            // if there is no dependent end (i.e., many-to-many), there are no foreign keys
            ModelClass principal = association.Principal;
            ModelClass dependent = association.Dependent;

            if (principal != null && dependent != null)
            {
               if (string.IsNullOrWhiteSpace(association.FKPropertyName))
                  result = principal.AllIdentityAttributes.Select(identity => $"\"{CreateShadowPropertyName(association, foreignKeyColumns, identity)}\"").ToList();
               else
               {
                  // defined properties
                  result = association.FKPropertyName.Split(',').Select(prop => "k." + prop.Trim()).ToList();
                  foreignKeyColumns.AddRange(result);
               }
            }

            return result;
         }
      }
      #endregion Template
   }
}

