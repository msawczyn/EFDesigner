using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
// ReSharper disable RedundantNameQualifier

namespace Sawczyn.EFDesigner.EFModel.DslPackage.TextTemplates.EditingOnly
{
   [SuppressMessage("ReSharper", "UnusedMember.Local")]
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   partial class EditOnly
   {
      /**************************************************
       * EFCore-specific code generation methods
       */

      void GenerateEFCore(Manager manager, ModelRoot modelRoot)
      {
         // Entities
         string fileNameMarker = string.IsNullOrEmpty(modelRoot.FileNameMarker) ? string.Empty : $".{modelRoot.FileNameMarker}";

         foreach (ModelClass modelClass in modelRoot.Classes.Where(e => e.GenerateCode))
         {
            manager.StartNewFile(Path.Combine(modelClass.EffectiveOutputDirectory, $"{modelClass.Name}{fileNameMarker}.cs"));
            WriteClass(modelClass);
         }

         // Enums

         foreach (ModelEnum modelEnum in modelRoot.Enums.Where(e => e.GenerateCode))
         {
            manager.StartNewFile(Path.Combine(modelEnum.EffectiveOutputDirectory, $"{modelEnum.Name}{fileNameMarker}.cs"));
            WriteEnum(modelEnum);
         }

         // DatabaseInitializer not yet supported in EF Core
         // manager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}DatabaseInitializer{fileNameMarker}.cs"));
         // WriteDatabaseInitializerEFCore(modelRoot);

         // MigrationConfiguration not yet supported in EF Core
         // manager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}DbMigrationConfiguration{fileNameMarker}.cs"));
         // WriteMigrationConfigurationEFCore(modelRoot);

         manager.StartNewFile(Path.Combine(modelRoot.ContextOutputDirectory, $"{modelRoot.EntityContainerName}{fileNameMarker}.cs"));
         WriteDbContextEFCore(modelRoot);
      }

      List<string> GetAdditionalUsingStatementsEFCore(ModelRoot modelRoot)
      {
         List<string> result = new List<string>();
         List<string> attributeTypes = modelRoot.Classes.SelectMany(c => c.Attributes).Select(a => a.Type).Distinct().ToList();

         if (attributeTypes.Any(t => t.IndexOf("Geometry", StringComparison.Ordinal) > -1 || t.IndexOf("Geography", StringComparison.Ordinal) > -1))
            result.Add("using System.Data.Entity.Spatial;");

         return result;
      }

      // Revisit if/when supported in EFCore

      // void WriteDatabaseInitializerEFCore(ModelRoot modelRoot)
      // {
      //    Output("using System.Data.Entity;");
      //    NL();
      // 
      //    BeginNamespace(modelRoot.Namespace);
      // 
      //    if (modelRoot.DatabaseInitializerType == DatabaseInitializerKind.MigrateDatabaseToLatestVersion)
      //       Output($"public partial class {modelRoot.EntityContainerName}DatabaseInitializer : MigrateDatabaseToLatestVersion<{modelRoot.EntityContainerName}, {modelRoot.EntityContainerName}DbMigrationConfiguration>");
      //    else
      //       Output($"public partial class {modelRoot.EntityContainerName}DatabaseInitializer : {modelRoot.DatabaseInitializerType}<{modelRoot.EntityContainerName}>");
      // 
      //    Output("{");
      //    Output("}");
      //    EndNamespace(modelRoot.Namespace);
      // }
      // 
      // void WriteMigrationConfigurationEFCore(ModelRoot modelRoot)
      // {
      //    //if (modelRoot.DatabaseInitializerType != DatabaseInitializerKind.MigrateDatabaseToLatestVersion)
      //    //   return;
      // 
      //    Output("using System.Data.Entity.Migrations;");
      //    NL();
      // 
      //    BeginNamespace(modelRoot.Namespace);
      //    Output("public sealed partial class {0}DbMigrationConfiguration : DbMigrationsConfiguration<{0}>", modelRoot.EntityContainerName);
      // 
      //    Output("{");
      //    Output("partial void Init();");
      //    NL();
      // 
      //    Output("public {0}DbMigrationConfiguration()", modelRoot.EntityContainerName);
      //    Output("{");
      //    Output("AutomaticMigrationsEnabled = {0};", modelRoot.AutomaticMigrationsEnabled.ToString().ToLower());
      //    Output("AutomaticMigrationDataLossAllowed = false;");
      //    Output("Init();");
      //    Output("}");
      // 
      //    Output("}");
      //    EndNamespace(modelRoot.Namespace);
      // }

      void WriteDbContextEFCore(ModelRoot modelRoot)
      {
         List<string> segments = new List<string>();
         ModelClass[] classesWithTables = null;

         // Note: TablePerType and TablePerConcreteType not yet available, but it doesn't hurt for them to be here since they shouldn't make it past the designer's validations
         switch (modelRoot.InheritanceStrategy)
         {
            case CodeStrategy.TablePerType:
               classesWithTables = modelRoot.Classes.Where(mc => !mc.IsDependentType).OrderBy(x => x.Name).ToArray();
               break;

            case CodeStrategy.TablePerConcreteType:
               classesWithTables = modelRoot.Classes.Where(mc => !mc.IsDependentType && !mc.IsAbstract).OrderBy(x => x.Name).ToArray();
               break;

            case CodeStrategy.TablePerHierarchy:
               classesWithTables = modelRoot.Classes.Where(mc => !mc.IsDependentType && mc.Superclass == null).OrderBy(x => x.Name).ToArray();
               break;
         }

         Output("using System;");
         Output("using System.Collections.Generic;");
         Output("using System.Linq;");
         Output("using System.ComponentModel.DataAnnotations.Schema;");
         Output("using Microsoft.EntityFrameworkCore;");
         NL();

         BeginNamespace(modelRoot.Namespace);

         WriteDbContextComments(modelRoot);

         string baseClass = string.IsNullOrWhiteSpace(modelRoot.BaseClass) ? "Microsoft.EntityFrameworkCore.DbContext" : modelRoot.BaseClass;
         Output($"{modelRoot.EntityContainerAccess.ToString().ToLower()} partial class {modelRoot.EntityContainerName} : {baseClass}");
         Output("{");

         if (classesWithTables?.Any() == true)
            WriteDbSetsEFCore(modelRoot);

         WriteConstructorsEFCore(modelRoot);
         WriteOnConfiguringEFCore(modelRoot, segments);
         WriteOnModelCreateEFCore(modelRoot, segments, classesWithTables);

         Output("}");

         EndNamespace(modelRoot.Namespace);
      }

      private void WriteDbSetsEFCore(ModelRoot modelRoot)
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

         Output("#endregion DbSets");
         NL();
      }

      private void WriteConstructorsEFCore(ModelRoot modelRoot)
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

      private void WriteOnConfiguringEFCore(ModelRoot modelRoot, List<string> segments)
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

            if (modelRoot.ChopMethodChains)
               OutputChopped(segments);
            else
               Output(string.Join(".", segments) + ";");

            NL();
         }

         Output("CustomInit(optionsBuilder);");
         Output("}");
         NL();
      }

      private void WriteOnModelCreateEFCore(ModelRoot modelRoot, List<string> segments, ModelClass[] classesWithTables)
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

         Output($"modelBuilder.HasDefaultSchema(\"{modelRoot.DatabaseSchema}\");");

         List<Association> visited = new List<Association>();
         List<string> foreignKeyColumns = new List<string>();

         foreach (ModelClass modelClass in modelRoot.Classes.OrderBy(x => x.Name))
         {
            segments.Clear();
            foreignKeyColumns.Clear();
            NL();

            // class level
            segments.Add($"modelBuilder.{(modelClass.IsDependentType ? "Owned" : "Entity")}<{modelClass.FullName}>()");

            foreach (ModelAttribute transient in modelClass.Attributes.Where(x => !x.Persistent))
               segments.Add($"Ignore(t => t.{transient.Name})");

            if (!modelClass.IsDependentType)
            {
               // note: this must come before the 'ToTable' call or there's a runtime error
               if (modelRoot.InheritanceStrategy == CodeStrategy.TablePerConcreteType && modelClass.Superclass != null)
                  segments.Add("Map(x => x.MapInheritedProperties())");

               if (classesWithTables.Contains(modelClass))
               {
                  segments.Add(modelClass.DatabaseSchema == modelClass.ModelRoot.DatabaseSchema
                                    ? $"ToTable(\"{modelClass.TableName}\")"
                                    : $"ToTable(\"{modelClass.TableName}\", \"{modelClass.DatabaseSchema}\")");

                  // primary key code segments must be output last, since HasKey returns a different type
                  List<ModelAttribute> identityAttributes = modelClass.IdentityAttributes.ToList();

                  if (identityAttributes.Count == 1)
                     segments.Add($"HasKey(t => t.{identityAttributes[0].Name})");
                  else if (identityAttributes.Count > 1)
                     segments.Add($"HasKey(t => new {{ t.{string.Join(", t.", identityAttributes.Select(ia => ia.Name))} }})");
               }
            }

            if (segments.Count > 1 || modelClass.IsDependentType)
            {
               if (modelRoot.ChopMethodChains)
                  OutputChopped(segments);
               else
                  Output(string.Join(".", segments) + ";");
            }

            if (modelClass.IsDependentType)
               continue;

            // attribute level
            foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.Persistent && !SpatialTypes.Contains(x.Type)))
            {
               segments.Clear();

               if ((modelAttribute.MaxLength ?? 0) > 0)
                  segments.Add($"HasMaxLength({modelAttribute.MaxLength.Value})");

               if (modelAttribute.Required)
                  segments.Add("IsRequired()");

               if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
                  segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

               if (!modelAttribute.AutoProperty)
               {
                  segments.Add($"HasField(\"_{modelAttribute.Name}\")");
                  segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{(modelAttribute.PersistencePoint == PersistencePointType.Field ? "Field" : "Property")})");
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

               if (segments.Any())
               {
                  segments.Insert(0, $"modelBuilder.{(modelClass.IsDependentType ? "Owned" : "Entity")}<{modelClass.FullName}>()");
                  segments.Insert(1, $"Property(t => t.{modelAttribute.Name})");

                  if (modelRoot.ChopMethodChains)
                     OutputChopped(segments);
                  else
                     Output(string.Join(".", segments) + ";");
               }

               if (modelAttribute.Indexed && !modelAttribute.IsIdentity)
               {
                  segments.Clear();

                  segments.Add($"modelBuilder.Entity<{modelClass.FullName}>().HasIndex(t => t.{modelAttribute.Name})");

                  if (modelAttribute.IndexedUnique)
                     segments.Add("IsUnique()");

                  if (modelRoot.ChopMethodChains)
                     OutputChopped(segments);
                  else
                     Output(string.Join(".", segments) + ";");
               }
            }

            bool hasDefinedConcurrencyToken = modelClass.AllAttributes.Any(x => x.IsConcurrencyToken);

            if (!hasDefinedConcurrencyToken && modelClass.EffectiveConcurrency == ConcurrencyOverride.Optimistic)
               Output($@"modelBuilder.Entity<{modelClass.FullName}>().Property<byte[]>(""Timestamp"").IsConcurrencyToken();");

            // Navigation endpoints are distingished as Source and Target. They are also distinguished as Principal
            // and Dependent. How do these map?
            // In the case of one-to-one or zero-to-one-to-zero-to-one, it's model dependent and the user has to tell us
            // In all other cases, we can tell by the cardinalities of the associations
            // What matters is the Principal and Dependent classifications, so we look at those. 
            // Source and Target are accidents of where the user started drawing the association.

            // navigation properties
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
               bool required = false;

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     // TODO: Implement many-to-many
                     if (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                        segments.Add($"HasMany(x => x.{association.TargetPropertyName})");
                     else
                        continue;

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne(x => x.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne(x => x.{association.TargetPropertyName})");

                     break;

                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add($"HasMany(x => x.{association.TargetPropertyName})");
                     //   break;
               }

               string columnPrefix = association.SourceRole == EndpointRole.Dependent
                                    ? ""
                                    : association.Target.Name + "_";

               switch (association.SourceMultiplicity) // realized by shadow property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     // TODO: Implement many-to-many
                     if (association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        segments.Add("WithMany()");
                        segments.Add($"HasForeignKey(\"{columnPrefix}{association.TargetPropertyName}_Id\")");
                     }
                     else
                        continue;

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add("WithOne()");

                     segments.Add(association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany
                                       ? $"HasForeignKey<{association.Source.FullName}>(\"{columnPrefix}{association.TargetPropertyName}_Id\")"
                                       : $"HasForeignKey(\"{columnPrefix}{association.TargetPropertyName}_Id\")");

                     required = true;

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add("WithOne()");

                     segments.Add(association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany
                                       ? $"HasForeignKey<{association.Source.FullName}>(\"{columnPrefix}{association.TargetPropertyName}_Id\")"
                                       : $"HasForeignKey(\"{columnPrefix}{association.TargetPropertyName}_Id\")");

                     break;

                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add("HasMany()");
                     //   break;
               }

               if (required)
                  segments.Add("IsRequired()");

               if (association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal)
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

               if (modelRoot.ChopMethodChains)
                  OutputChopped(segments);
               else
                  Output(string.Join(".", segments) + ";");
            }

            foreach (UnidirectionalAssociation association in Association.GetLinksToTargets(modelClass)
                                                                           .OfType<UnidirectionalAssociation>()
                                                                           .Where(x => x.Persistent && x.Target.IsDependentType))
            {
               if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne || association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                  Output($"modelBuilder.Entity<{modelClass.FullName}>().OwnsOne(x => x.{association.TargetPropertyName});");
               else
                  Output($"// Dependent 1-many association seen ({association.TargetPropertyName}). Code generation still unsupported in designer.");
            }

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (BidirectionalAssociation association in Association.GetLinksToSources(modelClass)
                                                                        .OfType<BidirectionalAssociation>()
                                                                        .Where(x => x.Persistent))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               // TODO: fix cascade delete
               bool required = false;

               segments.Clear();
               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.SourceMultiplicity) // realized by property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     // TODO: Implement many-to-many
                     if (association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                        segments.Add($"HasMany(x => x.{association.SourcePropertyName})");
                     else
                        continue;

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne(x => x.{association.SourcePropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne(x => x.{association.SourcePropertyName})");

                     break;

                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add($"HasMany(x => x.{association.SourcePropertyName})");
                     //   break;
               }

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     // TODO: Implement many-to-many
                     if (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                        segments.Add($"WithMany(x => x.{association.TargetPropertyName})");
                     else
                        continue;

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"WithOne(x => x.{association.TargetPropertyName})");
                     required = true;

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"WithOne(x => x.{association.TargetPropertyName})");

                     break;

                     //case Sawczyn.EFDesigner.EFModel.Multiplicity.OneMany:
                     //   segments.Add($"HasMany(x => x.{association.TargetPropertyName})");
                     //   break;
               }

               string foreignKeySegment = CreateForeignKeySegmentEFCore(association, foreignKeyColumns);

               if (foreignKeySegment != null)
                  segments.Add(foreignKeySegment);

               if (required)
                  segments.Add("IsRequired()");

               if (association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal)
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

               if (modelRoot.ChopMethodChains)
                  OutputChopped(segments);
               else
                  Output(string.Join(".", segments) + ";");
            }
         }

         NL();

         Output("OnModelCreatedImpl(modelBuilder);");
         Output("}");
      }

      string CreateForeignKeySegmentEFCore(Association association, List<string> foreignKeyColumns)
      {
         // foreign key definitions always go in the table representing the Dependent end of the association
         // if there is no dependent end (i.e., many-to-many), there are no foreign keys
         ModelClass principal;
         ModelClass dependent;

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

         string columnName;

         if (string.IsNullOrWhiteSpace(association.FKPropertyName))
         {
            // shadow properties
            columnName = string.Join(", "
                                    , principal.IdentityAttributes
                                                .Select(a => CreateShadowPropertyName(association, foreignKeyColumns, a))
                                                .Select(s => $@"""{s.Trim()}"""));
         }
         else
         {
            // defined properties
            foreignKeyColumns.AddRange(association.FKPropertyName.Split(','));
            columnName = string.Join(", ", association.FKPropertyName.Split(',').Select(s => $@"""{s.Trim()}"""));
         }

         return association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany
               && association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany
                     ? $"HasForeignKey<{dependent.FullName}>({columnName})"
                     : $"HasForeignKey({columnName})";
      }
   }
}