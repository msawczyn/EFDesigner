using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

// ReSharper disable UnusedMember.Global

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.3
      // Copyright (c) 2017-2021 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EF6ModelGenerator : EFModelGenerator
      {
         public EF6ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         private string[] SpatialTypes
         {
            get
            {
               return new[]
                      {
                         "Geography"
                       , "GeographyCollection"
                       , "GeographyLineString"
                       , "GeographyMultiLineString"
                       , "GeographyMultiPoint"
                       , "GeographyMultiPolygon"
                       , "GeographyPoint"
                       , "GeographyPolygon"
                       , "Geometry"
                       , "GeometryCollection"
                       , "GeometryLineString"
                       , "GeometryMultiLineString"
                       , "GeometryMultiPoint"
                       , "GeometryMultiPolygon"
                       , "GeometryPoint"
                       , "GeometryPolygon"
                      };
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         private string CreateForeignKeySegment(Association association, List<string> foreignKeyColumns)
         {
            // foreign key definitions always go in the table representing the Dependent end of the association
            // if there is no dependent end (i.e., many-to-many), there are no foreign keys
            ModelClass principal;
            ModelClass dependent;

            // declaring foreign keys can only happen on ..n multiplicities
            // otherwise, primary keys are required to be used, and the framework takes care of that
            if (association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany && association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
               return null;

            if (association.SourceRole == EndpointRole.Dependent)
            {
               dependent = association.Source;
               principal = association.Target;
            }
            else if (association.TargetRole == EndpointRole.Dependent)
            {
               dependent = association.Target;
               principal = association.Source;
            }
            else
               return null;

            string[] columnNameList = null;

            // shadow properties
            if (string.IsNullOrWhiteSpace(association.FKPropertyName))
            {
               columnNameList = principal.AllIdentityAttributes
                                         .Select(identityAttribute => CreateShadowPropertyName(association, foreignKeyColumns, identityAttribute))
                                         .ToArray();
            }
            else
            {
               columnNameList = association.FKPropertyName
                                           .Split(',')
                                           .Select(name => name.Trim())
                                           .ToArray();

               foreignKeyColumns.AddRange(columnNameList);
            }

            if (!columnNameList.Any())
               return null;

            string[] dependentPropertyNames = dependent.AllPropertyNames.ToArray();

            int existingPropertyCount = columnNameList.Intersect(dependentPropertyNames).Count();

            if (existingPropertyCount > 0)
            {
               return existingPropertyCount == 1
                         ? $"HasForeignKey(p => p.{columnNameList[0]})"
                         : $"HasForeignKey(p => new {{ {string.Join(", ", columnNameList.Select(n => $"p.{n}"))} }}";
            }

            return $"Map(x => x.MapKey({string.Join(", ", columnNameList.Select(n => $"\"{n}\""))}))";
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         private void DefineBidirectionalAssociations(ModelClass modelClass
                                                    , List<Association> visited
                                                    , List<string> segments
                                                    , List<string> foreignKeyColumns
                                                    , List<string> declaredShadowProperties)
         {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (BidirectionalAssociation association in Association.GetLinksToSources(modelClass)
                                                                        .OfType<BidirectionalAssociation>()
                                                                        .Where(x => x.Persistent))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               segments.Clear();
               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.SourceMultiplicity) // realized by property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany(x => x.{association.SourcePropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasRequired(x => x.{association.SourcePropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOptional(x => x.{association.SourcePropertyName})");

                     break;

                     //one or more constraint not supported in EF.
                     // TODO: make this possible ... later
                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add($"HasMany(x => x.{association.SourcePropertyName})");
                     //   break;
               }

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"WithMany(x => x.{association.TargetPropertyName})");

                     if (association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName) ? $"{association.SourcePropertyName}_x_{association.TargetPropertyName}" : association.JoinTableName;
                        string suffix1 = association.Source == association.Target ? "A" : "";
                        string suffix2 = association.Source == association.Target ? "B" : "";
                        string sourceMap = string.Join(", ", association.Source.AllIdentityAttributeNames.Select(n => $@"""{association.Source.Name}_{n}{suffix1}""").ToList());
                        string targetMap = string.Join(", ", association.Target.AllIdentityAttributeNames.Select(n => $@"""{association.Target.Name}_{n}{suffix2}""").ToList());

                        segments.Add(modelClass == association.Source
                                        ? $@"Map(x => {{ x.ToTable(""{tableMap}""); x.MapLeftKey({sourceMap}); x.MapRightKey({targetMap}); }})"
                                        : $@"Map(x => {{ x.ToTable(""{tableMap}""); x.MapLeftKey({targetMap}); x.MapRightKey({sourceMap}); }})");
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     if (association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                     {
                        segments.Add(association.SourceRole == EndpointRole.Dependent
                                        ? $"WithRequiredDependent(x => x.{association.TargetPropertyName})"
                                        : $"WithRequiredPrincipal(x => x.{association.TargetPropertyName})");
                     }
                     else
                        segments.Add($"WithRequired(x => x.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     if (association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne)
                     {
                        segments.Add(association.SourceRole == EndpointRole.Dependent
                                        ? $"WithOptionalDependent(x => x.{association.TargetPropertyName})"
                                        : $"WithOptionalPrincipal(x => x.{association.TargetPropertyName})");
                     }
                     else
                        segments.Add($"WithOptional(x => x.{association.TargetPropertyName})");

                     break;

                     //one or more constraint not supported in EF. TODO: make this possible ... later
                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add($"HasMany(x => x.{association.TargetPropertyName})");
                     //   break;
               }

               string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

               // can't shadow properties twice
               if (foreignKeySegment != null)
               {
                  if (!foreignKeySegment.Contains("MapKey"))
                     segments.Add(foreignKeySegment);
                  else if (!declaredShadowProperties.Contains(foreignKeySegment))
                  {
                     declaredShadowProperties.Add(foreignKeySegment);
                     segments.Add(foreignKeySegment);
                  }
               }

               if ((association.TargetDeleteAction != DeleteAction.Default && association.TargetRole == EndpointRole.Principal)
                || (association.SourceDeleteAction != DeleteAction.Default && association.SourceRole == EndpointRole.Principal))
               {
                  string willCascadeOnDelete = association.TargetDeleteAction != DeleteAction.Default && association.TargetRole == EndpointRole.Principal
                                                  ? (association.TargetDeleteAction == DeleteAction.Cascade).ToString().ToLowerInvariant()
                                                  : (association.SourceDeleteAction == DeleteAction.Cascade).ToString().ToLowerInvariant();

                  segments.Add($"WillCascadeOnDelete({willCascadeOnDelete})");
               }

               Output(segments);
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         private void DefineUnidirectionalAssociations(ModelClass modelClass
                                                     , List<Association> visited
                                                     , List<string> segments
                                                     , List<string> foreignKeyColumns
                                                     , List<string> declaredShadowProperties)
         {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (UnidirectionalAssociation association in Association.GetLinksToTargets(modelClass)
                                                                         .OfType<UnidirectionalAssociation>()
                                                                         .Where(x => x.Persistent && !x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               segments.Clear();
               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany(x => x.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasRequired(x => x.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOptional(x => x.{association.TargetPropertyName})");

                     break;

                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add($"HasMany(x => x.{association.TargetPropertyName})");
                     //   break;
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add("WithMany()");

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName) ? $"{association.Source.Name}_x_{association.TargetPropertyName}" : association.JoinTableName;
                        string suffix1 = association.Source == association.Target ? "A" : "";
                        string suffix2 = association.Source == association.Target ? "B" : "";
                        string sourceMap = string.Join(", ", association.Source.AllIdentityAttributeNames.Select(n => $@"""{association.Source.Name}_{n}{suffix1}""").ToList());
                        string targetMap = string.Join(", ", association.Target.AllIdentityAttributeNames.Select(n => $@"""{association.Target.Name}_{n}{suffix2}""").ToList());

                        segments.Add(modelClass == association.Source
                                        ? $@"Map(x => {{ x.ToTable(""{tableMap}""); x.MapLeftKey({sourceMap}); x.MapRightKey({targetMap}); }})"
                                        : $@"Map(x => {{ x.ToTable(""{tableMap}""); x.MapLeftKey({targetMap}); x.MapRightKey({sourceMap}); }})");
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                     {
                        segments.Add(association.TargetRole == EndpointRole.Dependent
                                        ? "WithRequiredDependent()"
                                        : "WithRequiredPrincipal()");
                     }
                     else
                        segments.Add("WithRequired()");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne)
                     {
                        segments.Add(association.TargetRole == EndpointRole.Dependent
                                        ? "WithOptionalDependent()"
                                        : "WithOptionalPrincipal()");
                     }
                     else
                        segments.Add("WithOptional()");

                     break;

                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add("HasMany()");
                     //   break;
               }

               string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

               // can't include shadow properties twice
               if (foreignKeySegment != null)
               {
                  if (!foreignKeySegment.Contains("MapKey"))
                     segments.Add(foreignKeySegment);
                  else if (!declaredShadowProperties.Contains(foreignKeySegment))
                  {
                     declaredShadowProperties.Add(foreignKeySegment);
                     segments.Add(foreignKeySegment);
                  }
               }

               // Certain associations cascade delete automatically. Also, the user may ask for it.
               // We only generate a cascade delete call if the user asks for it. 
               if ((association.TargetDeleteAction != DeleteAction.Default && association.TargetRole == EndpointRole.Principal)
                || (association.SourceDeleteAction != DeleteAction.Default && association.SourceRole == EndpointRole.Principal))
               {
                  string willCascadeOnDelete = association.TargetDeleteAction != DeleteAction.Default && association.TargetRole == EndpointRole.Principal
                                                  ? (association.TargetDeleteAction == DeleteAction.Cascade).ToString().ToLowerInvariant()
                                                  : (association.SourceDeleteAction == DeleteAction.Cascade).ToString().ToLowerInvariant();

                  segments.Add($"WillCascadeOnDelete({willCascadeOnDelete})");
               }

               Output(segments);
            }
         }

         public override void Generate(Manager efModelFileManager)
         {
            // Entities
            string fileNameMarker = string.IsNullOrEmpty(modelRoot.FileNameMarker) ? string.Empty : $".{modelRoot.FileNameMarker}";

            foreach (ModelClass modelClass in modelRoot.Classes.Where(e => e.GenerateCode))
            {
               ClearIndent();
               efModelFileManager.StartNewFile(Path.Combine(modelClass.EffectiveOutputDirectory, $"{modelClass.Name}{fileNameMarker}.cs"));
               WriteClass(modelClass);
            }

            // Enums

            foreach (ModelEnum modelEnum in modelRoot.Enums.Where(e => e.GenerateCode))
            {
               ClearIndent();
               efModelFileManager.StartNewFile(Path.Combine(modelEnum.EffectiveOutputDirectory, $"{modelEnum.Name}{fileNameMarker}.cs"));
               WriteEnum(modelEnum);
            }

            // Context

            if (modelRoot.DatabaseInitializerType != DatabaseInitializerKind.None)
            {
               ClearIndent();
               efModelFileManager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}DatabaseInitializer{fileNameMarker}.cs"));
               WriteDatabaseInitializer();
            }

            ClearIndent();
            efModelFileManager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}DbMigrationConfiguration{fileNameMarker}.cs"));
            WriteMigrationConfiguration();

            ClearIndent();
            efModelFileManager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}{fileNameMarker}.cs"));
            WriteDbContext();
         }

         protected override List<string> GetAdditionalUsingStatements()
         {
            List<string> result = new List<string>();
            List<string> attributeTypes = modelRoot.Classes.SelectMany(c => c.Attributes).Select(a => a.Type).Distinct().ToList();

            if (attributeTypes.Intersect(modelRoot.SpatialTypes).Any())
               result.Add("using System.Data.Entity.Spatial;");

            return result;
         }

         private void WriteConstructors()
         {
            Output("#region Constructors");
            NL();
            Output("partial void CustomInit();");
            NL();

            if (!string.IsNullOrEmpty(modelRoot.ConnectionString) || !string.IsNullOrEmpty(modelRoot.ConnectionStringName))
            {
               string connectionString = string.IsNullOrEmpty(modelRoot.ConnectionString)
                                            ? $"Name={modelRoot.ConnectionStringName}"
                                            : modelRoot.ConnectionString;

               Output("/// <summary>");
               Output("/// Default connection string");
               Output("/// </summary>");
               Output($"public static string ConnectionString {{ get; set; }} = @\"{connectionString}\";");

               Output("/// <inheritdoc />");
               Output($"public {modelRoot.EntityContainerName}() : base(ConnectionString)");
               Output("{");
               Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
               Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

               Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                         ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                         : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

               Output("CustomInit();");
               Output("}");
               NL();
            }
            else
            {
               Output($"#warning Default constructor not generated for {modelRoot.EntityContainerName} since no default connection string was specified in the model");
               NL();
            }

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(string connectionString) : base(connectionString)");
            Output("{");
            Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
            Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                      ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                      : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

            Output("CustomInit();");
            Output("}");
            NL();

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)");
            Output("{");
            Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
            Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                      ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                      : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

            Output("CustomInit();");
            Output("}");
            NL();

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)");
            Output("{");
            Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
            Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                      ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                      : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

            Output("CustomInit();");
            Output("}");
            NL();

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)");
            Output("{");
            Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
            Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                      ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                      : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

            Output("CustomInit();");
            Output("}");
            NL();

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(System.Data.Entity.Infrastructure.DbCompiledModel model) : base(model)");
            Output("{");
            Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
            Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                      ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                      : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

            Output("CustomInit();");
            Output("}");
            NL();

            Output("/// <inheritdoc />");
            Output($"public {modelRoot.EntityContainerName}(System.Data.Entity.Core.Objects.ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)");
            Output("{");
            Output($"Configuration.LazyLoadingEnabled = {modelRoot.LazyLoadingEnabled.ToString().ToLower()};");
            Output($"Configuration.ProxyCreationEnabled = {modelRoot.ProxyGenerationEnabled.ToString().ToLower()};");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.None
                      ? $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(null);"
                      : $"System.Data.Entity.Database.SetInitializer<{modelRoot.EntityContainerName}>(new {modelRoot.EntityContainerName}DatabaseInitializer());");

            Output("CustomInit();");
            Output("}");
            NL();
            Output("#endregion Constructors");
            NL();
         }

         private void WriteDatabaseInitializer()
         {
            Output("using System.Data.Entity;");
            NL();

            BeginNamespace(modelRoot.Namespace);

            Output("/// <inheritdoc/>");

            Output(modelRoot.DatabaseInitializerType == DatabaseInitializerKind.MigrateDatabaseToLatestVersion
                      ? $"public partial class {modelRoot.EntityContainerName}DatabaseInitializer : MigrateDatabaseToLatestVersion<{modelRoot.Namespace}.{modelRoot.EntityContainerName}, {GetMigrationNamespace()}.{modelRoot.EntityContainerName}DbMigrationConfiguration>"
                      : $"public partial class {modelRoot.EntityContainerName}DatabaseInitializer : {modelRoot.DatabaseInitializerType}<{modelRoot.Namespace}.{modelRoot.EntityContainerName}>");

            Output("{");
            Output("}");
            EndNamespace(modelRoot.Namespace);
         }

         private void WriteDbContext()
         {
            ModelClass[] classesWithTables = null;

            switch (modelRoot.InheritanceStrategy)
            {
               case CodeStrategy.TablePerType:
                  classesWithTables = modelRoot.Classes.Where(mc => !mc.IsDependentType && mc.GenerateCode).OrderBy(x => x.Name).ToArray();

                  break;

               case CodeStrategy.TablePerConcreteType:
                  classesWithTables = modelRoot.Classes.Where(mc => !mc.IsDependentType && !mc.IsAbstract && mc.GenerateCode).OrderBy(x => x.Name).ToArray();

                  break;

               case CodeStrategy.TablePerHierarchy:
                  classesWithTables = modelRoot.Classes.Where(mc => !mc.IsDependentType && mc.Superclass == null && mc.GenerateCode).OrderBy(x => x.Name).ToArray();

                  break;
            }

            Output("using System;");
            Output("using System.Collections.Generic;");
            Output("using System.Linq;");
            Output("using System.ComponentModel.DataAnnotations.Schema;");
            Output("using System.Data.Entity;");
            Output("using System.Data.Entity.Infrastructure.Annotations;");
            NL();

            BeginNamespace(modelRoot.Namespace);

            WriteDbContextComments();

            string baseClass = string.IsNullOrWhiteSpace(modelRoot.BaseClass) ? "System.Data.Entity.DbContext" : modelRoot.BaseClass;

            Output($"{modelRoot.EntityContainerAccess.ToString().ToLower()} partial class {modelRoot.EntityContainerName} : {baseClass}");
            Output("{");

            if (classesWithTables?.Any() == true)
               WriteDbSets();

            WriteConstructors();
            WriteOnModelCreate(classesWithTables);

            Output("}");

            EndNamespace(modelRoot.Namespace);
         }

         private void WriteDbSets()
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

               Output($"{modelRoot.DbSetAccess.ToString().ToLower()} virtual System.Data.Entity.DbSet<{modelClass.FullName}> {dbSetName} {{ get; set; }}");
            }

            Output("#endregion DbSets");
            NL();
         }

         private void WriteMigrationConfiguration()
         {
            Output("using System.Data.Entity.Migrations;");
            NL();

            BeginNamespace(GetMigrationNamespace());
            Output("/// <inheritdoc/>");
            Output($"public sealed partial class {modelRoot.EntityContainerName}DbMigrationConfiguration : DbMigrationsConfiguration<{modelRoot.Namespace}.{modelRoot.EntityContainerName}>");

            Output("{");
            Output("partial void Init();");
            NL();

            Output("/// <inheritdoc/>");
            Output($"public {modelRoot.EntityContainerName}DbMigrationConfiguration()");
            Output("{");
            Output($"AutomaticMigrationsEnabled = {modelRoot.AutomaticMigrationsEnabled.ToString().ToLower()};");
            Output("AutomaticMigrationDataLossAllowed = false;");
            Output("Init();");
            Output("}");

            Output("}");
            EndNamespace(modelRoot.Namespace);
         }

         private void WriteOnModelCreate(ModelClass[] classesWithTables)
         {
            Output("partial void OnModelCreatingImpl(System.Data.Entity.DbModelBuilder modelBuilder);");
            Output("partial void OnModelCreatedImpl(System.Data.Entity.DbModelBuilder modelBuilder);");
            NL();

            Output("/// <inheritdoc />");
            Output("protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)");
            Output("{");
            Output("base.OnModelCreating(modelBuilder);");
            Output("OnModelCreatingImpl(modelBuilder);");
            NL();

            if (!string.IsNullOrEmpty(modelRoot.DatabaseSchema))
               Output($"modelBuilder.HasDefaultSchema(\"{modelRoot.DatabaseSchema}\");");

            List<string> segments = new List<string>();

            List<Association> visited = new List<Association>();
            List<string> foreignKeyColumns = new List<string>();
            List<string> declaredShadowProperties = new List<string>();

            foreach (ModelClass modelClass in modelRoot.Classes.OrderBy(x => x.Name))
            {
               segments.Clear();
               foreignKeyColumns.Clear();
               declaredShadowProperties.Clear();
               NL();

               // class level
               bool isDependent = modelClass.IsDependentType;
               segments.Add($"modelBuilder.{(isDependent ? "ComplexType" : "Entity")}<{modelClass.FullName}>()");

               foreach (ModelAttribute transient in modelClass.Attributes.Where(x => !x.Persistent))
                  segments.Add($"Ignore(t => t.{transient.Name})");

               // note: this must come before the 'ToTable' call or there's a runtime error
               if (modelRoot.InheritanceStrategy == CodeStrategy.TablePerConcreteType && modelClass.Superclass != null)
                  segments.Add("Map(x => x.MapInheritedProperties())");

               if (classesWithTables.Contains(modelClass))
               {
                  if (modelRoot.InheritanceStrategy != CodeStrategy.TablePerConcreteType || !modelClass.IsAbstract)
                  {
                     segments.Add(string.IsNullOrEmpty(modelClass.DatabaseSchema) || modelClass.DatabaseSchema == modelClass.ModelRoot.DatabaseSchema
                                     ? $"ToTable(\"{modelClass.TableName}\")"
                                     : $"ToTable(\"{modelClass.TableName}\", \"{modelClass.DatabaseSchema}\")");
                  }

                  // primary key code segments must be output last, since HasKey returns a different type
                  List<ModelAttribute> identityAttributes = modelClass.IdentityAttributes.ToList();

                  if (identityAttributes.Count == 1)
                     segments.Add($"HasKey(t => t.{identityAttributes[0].Name})");
                  else if (identityAttributes.Count > 1)
                     segments.Add($"HasKey(t => new {{ t.{string.Join(", t.", identityAttributes.Select(ia => ia.Name))} }})");
               }

               Output(segments);

               if (modelClass.IsDependentType)
                  continue;

               // attribute level
               foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.Persistent && !SpatialTypes.Contains(x.Type)))
               {
                  segments.Clear();

                  if ((modelAttribute.MaxLength ?? 0) > 0)
                     segments.Add($"HasMaxLength({modelAttribute.MaxLength})");

                  if (modelAttribute.Required)
                     segments.Add("IsRequired()");

                  if (!string.IsNullOrEmpty(modelAttribute.ColumnName) && modelAttribute.ColumnName != modelAttribute.Name)
                     segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

                  if (!string.IsNullOrEmpty(modelAttribute.ColumnType) && modelAttribute.ColumnType.ToLowerInvariant() != "default")
                     segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}\")");

                  if (modelAttribute.Indexed && !modelAttribute.IsIdentity)
                     segments.Add("HasColumnAnnotation(\"Index\", new IndexAnnotation(new IndexAttribute()))");

                  if (modelAttribute.IsConcurrencyToken)
                     segments.Add("IsRowVersion()");

                  if (modelAttribute.IsIdentity)
                  {
                     segments.Add(modelAttribute.IdentityType == IdentityType.AutoGenerated
                                     ? "HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)"
                                     : "HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)");
                  }

                  if (segments.Any())
                  {
                     segments.Insert(0, $"modelBuilder.{(isDependent ? "ComplexType" : "Entity")}<{modelClass.FullName}>()");
                     segments.Insert(1, $"Property(t => t.{modelAttribute.Name})");

                     Output(segments);
                  }

                  if (modelAttribute.Indexed && !modelAttribute.IsIdentity)
                  {
                     segments.Clear();

                     segments.Add($"modelBuilder.Entity<{modelClass.FullName}>().HasIndex(t => t.{modelAttribute.Name})");

                     if (modelAttribute.IndexedUnique)
                        segments.Add("IsUnique()");

                     if (segments.Count > 1)
                        Output(segments);
                  }
               }

               if (!isDependent)
               {
                  // Navigation endpoints are distingished as Source and Target. They are also distinguished as Principal
                  // and Dependent. How do these map?
                  // In the case of one-to-one or zero-to-one-to-zero-to-one, it's model dependent and the user has to tell us
                  // In all other cases, we can tell by the cardinalities of the associations
                  // What matters is the Principal and Dependent classifications, so we look at those. 
                  // Source and Target are accidents of where the user started drawing the association.

                  // navigation properties

                  DefineUnidirectionalAssociations(modelClass
                                                 , visited
                                                 , segments
                                                 , foreignKeyColumns
                                                 , declaredShadowProperties);

                  DefineBidirectionalAssociations(modelClass
                                                , visited
                                                , segments
                                                , foreignKeyColumns
                                                , declaredShadowProperties);
               }
            }

            NL();

            Output("OnModelCreatedImpl(modelBuilder);");
            Output("}");
         }
      }
      #endregion Template
   }
}
