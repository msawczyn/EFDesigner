using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Sawczyn.EFDesigner.EFModel.Annotations;

// ReSharper disable RedundantNameQualifier

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   [UsedImplicitly]
   public partial class GeneratedTextTransformation
   {
      #region Template

      // EFDesigner v4.1.2.0
      // Copyright (c) 2017-2022 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore5ModelGenerator : EFCore3ModelGenerator
      {
         public EFCore5ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         protected override void ConfigureModelClasses(List<string> segments, ModelClass[] classesWithTables, List<string> foreignKeyColumns, List<Association> visited)
         {
            foreach (ModelClass modelClass in modelRoot.Classes.Where(x => !x.IsAssociationClass).OrderBy(x => x.Name))
               ConfigureModelClass(segments, classesWithTables, foreignKeyColumns, visited, modelClass);
         }

         protected override void ConfigureModelClass(List<string> segments, ModelClass[] classesWithTables, List<string> foreignKeyColumns, List<Association> visited, ModelClass modelClass)
         {
            segments.Clear();
            foreignKeyColumns.Clear();
            NL();

            if (modelClass.IsDependentType)
            {
               segments.Add($"modelBuilder.Owned<{modelClass.FullName}>()");
               Output(segments);

               return;
            }

            segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

            ConfigureTransientProperties(segments, modelClass);

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

         protected override void ConfigureTable(List<string> segments, ModelClass modelClass)
         {
            string tableName = string.IsNullOrEmpty(modelClass.TableName)
                                  ? modelClass.Name
                                  : modelClass.TableName;

            string viewName = string.IsNullOrEmpty(modelClass.ViewName)
                                 ? modelClass.Name
                                 : modelClass.ViewName;

            string schema = string.IsNullOrEmpty(modelClass.DatabaseSchema) || modelClass.DatabaseSchema == modelClass.ModelRoot.DatabaseSchema
                               ? string.Empty
                               : $", \"{modelClass.DatabaseSchema}\"";

            List<string> modifiers = new List<string>();

            if (modelClass.ExcludeFromMigrations)
               modifiers.Add("t.ExcludeFromMigrations();");

            if (modelClass.UseTemporalTables
             && !modelClass.IsDatabaseView
             && (!modelClass.Subclasses.Any() || modelClass.ModelRoot.InheritanceStrategy == CodeStrategy.TablePerHierarchy)
             && modelClass.Superclass == null)
               modifiers.Add("t.IsTemporal();");

            string buildActions = modifiers.Any()
                                     ? $", t => {{ {string.Join(" ", modifiers)} }}"
                                     : string.Empty;

            segments.Add(modelClass.IsDatabaseView
                            ? $"ToView(\"{viewName}\"{schema}{buildActions})"
                            : $"ToTable(\"{tableName}\"{schema}{buildActions})");

            if (modelClass.Superclass != null)
               segments.Add($"HasBaseType<{modelClass.Superclass.FullName}>()");

            // primary key code segments must be output last, since HasKey returns a different type
            List<ModelAttribute> identityAttributes = modelClass.IdentityAttributes.ToList();

            if (identityAttributes.Count == 1)
               segments.Add($"HasKey(t => t.{identityAttributes[0].Name})");
            else if (identityAttributes.Count > 1)
               segments.Add($"HasKey(t => new {{ t.{string.Join(", t.", identityAttributes.Select(ia => ia.Name))} }})");
         }

         protected override List<string> GatherModelAttributeSegments(ModelAttribute modelAttribute)
         {
            List<string> segments = base.GatherModelAttributeSegments(modelAttribute);

            if (!string.IsNullOrEmpty(modelAttribute.InitialValue))
            {
               if (modelAttribute.InitialValue.Contains(".")) // enum
               {
                  string enumName = modelAttribute.InitialValue.Split('.').First();
                  string enumValue = modelAttribute.InitialValue.Split('.').Last();
                  string enumFQN = modelAttribute.ModelClass.ModelRoot.Enums.FirstOrDefault(e => e.Name == enumName)?.FullName ?? enumName;
                  segments.Add($"HasDefaultValue({enumFQN.Trim()}.{enumValue.Trim()})");
               }
               else
               {
                  switch (modelAttribute.Type)
                  {
                     case "String":
                        segments.Add($"HasDefaultValue(\"{modelAttribute.InitialValue.Trim(' ', '"')}\")");

                        break;

                     case "Char":
                        segments.Add($"HasDefaultValue('{modelAttribute.InitialValue.Trim(' ', '\'')}')");

                        break;

                     case "DateTime":
                        if (modelAttribute.InitialValue == "DateTime.UtcNow")
                           segments.Add("HasDefaultValueSql(\"CURRENT_TIMESTAMP\")");

                        break;

                     default:
                        segments.Add($"HasDefaultValue({modelAttribute.InitialValue})");

                        break;
                  }
               }
            }

            if (!string.IsNullOrEmpty(modelAttribute.DatabaseCollation)
             && modelAttribute.DatabaseCollation != modelRoot.DatabaseCollationDefault
             && modelAttribute.Type == "String")
               segments.Add($"UseCollation(\"{modelAttribute.DatabaseCollation.Trim('"')}\")");

            int index = segments.IndexOf("IsRequired()");

            if (index >= 0)
            {
               segments.RemoveAt(index);
               segments.Add("IsRequired()");
            }

            return segments;
         }

         protected override void WriteOnModelCreate(List<string> segments, ModelClass[] classesWithTables)
         {
            Output("partial void OnModelCreatingImpl(ModelBuilder modelBuilder);");
            Output("partial void OnModelCreatedImpl(ModelBuilder modelBuilder);");
            NL();

            Output("/// <summary>");
            Output("///     Override this method to further configure the model that was discovered by convention from the entity types");
            Output("///     exposed in <see cref=\"T:Microsoft.EntityFrameworkCore.DbSet`1\" /> properties on your derived context. The resulting model may be cached");
            Output("///     and re-used for subsequent instances of your derived context.");
            Output("/// </summary>");
            Output("/// <remarks>");
            Output("///     If a model is explicitly set on the options for this context (via <see cref=\"M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)\" />)");
            Output("///     then this method will not be run.");
            Output("/// </remarks>");
            Output("/// <param name=\"modelBuilder\">");
            Output("///     The builder being used to construct the model for this context. Databases (and other extensions) typically");
            Output("///     define extension methods on this object that allow you to configure aspects of the model that are specific");
            Output("///     to a given database.");
            Output("/// </param>");
            Output("protected override void OnModelCreating(ModelBuilder modelBuilder)");
            Output("{");
            Output("base.OnModelCreating(modelBuilder);");
            Output("OnModelCreatingImpl(modelBuilder);");
            NL();

            if (!string.IsNullOrEmpty(modelRoot.DatabaseSchema))
               Output($"modelBuilder.HasDefaultSchema(\"{modelRoot.DatabaseSchema.Trim('"')}\");");

            if (modelRoot.DatabaseCollationDefault.ToLowerInvariant() != "default")
               Output($"modelBuilder.UseCollation(\"{modelRoot.DatabaseCollationDefault.Trim('"')}\");");

            List<Association> visited = new List<Association>();
            List<string> foreignKeyColumns = new List<string>();

            ConfigureModelClasses(segments, classesWithTables, foreignKeyColumns, visited);

            NL();

            Output("OnModelCreatedImpl(modelBuilder);");
            Output("}");
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected override void ConfigureBidirectionalAssociations(ModelClass modelClass
                                                                  , List<Association> visited
                                                                  , List<string> foreignKeyColumns
                                                                  , List<string> declaredShadowProperties)
         {
            WriteBidirectionalNonDependentAssociations(modelClass, visited, foreignKeyColumns);
            WriteBidirectionalDependentAssociations(modelClass, $"modelBuilder.Entity<{modelClass.FullName}>()", visited);
         }

         protected override void WriteBidirectionalDependentAssociations(ModelClass sourceInstance, string baseSegment, List<Association> visited)
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

               string separator = sourceInstance.ModelRoot.ShadowKeyNamePattern == ShadowKeyPattern.TableColumn
                                     ? string.Empty
                                     : "_";

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

                        segments.Add(baseSegment);
                        segments.Add($"Navigation(p => p.{association.TargetPropertyName}).IsRequired()");
                        Output(segments);

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

         protected override void WriteBidirectionalNonDependentAssociations(ModelClass modelClass, List<Association> visited, List<string> foreignKeyColumns)
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
                     {
                        segments.Add($"HasMany<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");
                        required = association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One;

                        break;
                     }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     {
                        segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                        break;
                     }
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"WithMany(p => p.{association.SourcePropertyName})");

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        ModelClass associationClass = modelClass.Store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(m => m.DescribedAssociationElementId == association.Id);

                        if (associationClass == null)
                           segments.AddRange(WriteStandardBidirectionalAssociation(association, foreignKeyColumns, required));
                        else
                        {
                           OutputNoTerminator(segments);
                           WriteBidirectionalAssociationWithAssociationClass(modelClass, associationClass, association);
                        }
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"WithOne(p => p.{association.SourcePropertyName})");

                     break;
               }

               if (segments.Any()) Output(segments);

               if (association.TargetAutoInclude)
                  Output($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName}).AutoInclude();");
               else if (association.SourceAutoInclude)
                  Output($"modelBuilder.Entity<{association.Target.FullName}>().Navigation(e => e.{association.SourcePropertyName}).AutoInclude();");

               if (!association.TargetAutoProperty)
               {
                  segments.Add($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName})");
                  segments.Add($"HasField(\"{association.TargetBackingFieldName}\")");

                  segments.Add(modelClass.ModelRoot.IsEFCore6Plus
                                  ? $"UsePropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})"
                                  : $"Metadata.SetPropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})");

                  Output(segments);
               }

               if (!association.SourceAutoProperty)
               {
                  segments.Add($"modelBuilder.Entity<{association.Target.FullName}>().Navigation(e => e.{association.SourcePropertyName})");
                  segments.Add($"HasField(\"{association.SourceBackingFieldName}\")");

                  segments.Add(modelClass.ModelRoot.IsEFCore6Plus
                                  ? $"UsePropertyAccessMode(PropertyAccessMode.{association.SourcePropertyAccessMode})"
                                  : $"Metadata.SetPropertyAccessMode(PropertyAccessMode.{association.SourcePropertyAccessMode})");

                  Output(segments);
               }
            }
         }

         private IEnumerable<string> WriteStandardBidirectionalAssociation(BidirectionalAssociation association, List<string> foreignKeyColumns, bool required)
         {
            List<string> segments = new List<string>();

            string tableMap = string.IsNullOrEmpty(association.JoinTableName)
                                 ? $"{association.Target.Name}_{association.SourcePropertyName}_x_{association.Source.Name}_{association.TargetPropertyName}"
                                 : association.JoinTableName;

            segments.Add($"UsingEntity(x => x.ToTable(\"{tableMap}\"))");

            string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

            if (!string.IsNullOrEmpty(foreignKeySegment))
               segments.Add(foreignKeySegment);

            WriteSourceDeleteBehavior(association, segments);
            WriteTargetDeleteBehavior(association, segments);

            if (required
             && (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
              || association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
               segments.Add("IsRequired()");

            return segments;
         }

         private void WriteBidirectionalAssociationWithAssociationClass(ModelClass modelClass, ModelClass associationClass, BidirectionalAssociation association)
         {
            string indent = associationClass.ModelRoot.UseTabs
                               ? "\t"
                               : "   ";
            BidirectionalAssociation associationToSource = (BidirectionalAssociation)associationClass.AllNavigationProperties().First(n => n.AssociationObject.Source == association.Source).AssociationObject;
            BidirectionalAssociation associationToTarget = (BidirectionalAssociation)associationClass.AllNavigationProperties().First(n => n.AssociationObject.Source == association.Target).AssociationObject;

            if (modelClass.ModelRoot.ChopMethodChains) 
               PushIndent("            "); 
            else 
               PushIndent(indent);

            Output($".UsingEntity<{associationClass.FullName}>(");
            PushIndent(indent);
            Output("j => j");
            PushIndent(indent);
            Output($".HasOne(x => x.{associationToTarget.SourcePropertyName})");
            Output($".WithMany(x => x.{associationToTarget.TargetPropertyName})");
            Output($".HasForeignKey(x => x.{associationClass.Attributes.First(a => a.IsForeignKeyFor == associationToTarget.Id).Name}),");
            PopIndent();
            Output("j => j");
            PushIndent(indent);
            Output($".HasOne(x => x.{associationToSource.SourcePropertyName})");
            Output($".WithMany(x => x.{associationToSource.TargetPropertyName})");
            Output($".HasForeignKey(x => x.{associationClass.Attributes.First(a => a.IsForeignKeyFor == associationToSource.Id).Name}),");
            PopIndent();
            Output("j =>");
            Output("{");

            #region transient properties

            foreach (ModelAttribute transient in associationClass.Attributes.Where(x => !x.Persistent))
               Output($"j.Ignore(t => t.{transient.Name});");

            #endregion

            #region table definition

            string tableName = string.IsNullOrEmpty(associationClass.TableName)
                                  ? associationClass.Name
                                  : associationClass.TableName;

            string schema = string.IsNullOrEmpty(associationClass.DatabaseSchema) || associationClass.DatabaseSchema == associationClass.ModelRoot.DatabaseSchema
                               ? string.Empty
                               : $", \"{associationClass.DatabaseSchema}\"";

            List<string> modifiers = new List<string>();

            if (associationClass.UseTemporalTables)
               modifiers.Add(" t.IsTemporal();");

            string buildActions = modifiers.Any()
                                     ? $", t => {{ {string.Join(" ", modifiers)} }}"
                                     : string.Empty;

            Output($"j.ToTable(\"{tableName}\"{schema}{buildActions});");

            List<ModelAttribute> identityAttributes = associationClass.IdentityAttributes.ToList();

            if (identityAttributes.Count == 1)
               Output($"j.HasKey(t => t.{identityAttributes[0].Name});");
            else if (identityAttributes.Count > 1)
               Output($"j.HasKey(t => new {{ t.{string.Join(", t.", identityAttributes.Select(ia => ia.Name))} }});");

            #endregion

#region model attributes

            foreach (ModelAttribute modelAttribute in associationClass.Attributes.Where(x => x.Persistent && !x.IsIdentity))
            {
               List<string> buffer = new List<string>();
               buffer.AddRange(GatherModelAttributeSegments(modelAttribute));

               if (buffer.Any())
                  Output($"j.Property(t => t.{modelAttribute.Name}).{string.Join(".", buffer)};");

               if (modelAttribute.Indexed)
               {
                  buffer.Clear();
                  buffer.Add($"HasIndex(t => t.{modelAttribute.Name})");

                  if (modelAttribute.IndexedUnique)
                     buffer.Add("IsUnique()");

                  Output($"j.{string.Join(".", buffer)};");
               }
               }

#endregion

            PopIndent();
            Output("});");
            PopIndent();
            PopIndent();
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected override void ConfigureUnidirectionalAssociations(ModelClass modelClass
                                                                   , List<Association> visited
                                                                   , List<string> foreignKeyColumns
                                                                   , List<string> declaredShadowProperties)
         {
            WriteUnidirectionalNonDependentAssociations(modelClass, visited, foreignKeyColumns);
            WriteUnidirectionalDependentAssociations(modelClass, $"modelBuilder.Entity<{modelClass.FullName}>()", visited);
         }

         protected override void WriteUnidirectionalDependentAssociations(ModelClass sourceInstance, string baseSegment, List<Association> visited)
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

               string separator = sourceInstance.ModelRoot.ShadowKeyNamePattern == ShadowKeyPattern.TableColumn
                                     ? string.Empty
                                     : "_";

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

                        segments.Add(baseSegment);
                        segments.Add($"Navigation(p => p.{association.TargetPropertyName}).IsRequired()");
                        Output(segments);

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

         protected override void WriteUnidirectionalNonDependentAssociations(ModelClass modelClass, List<Association> visited, List<string> foreignKeyColumns)
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
                     required = (association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                     break;
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add("WithMany()");
                     required = (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One);

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName)
                                             ? $"{association.Target.Name}_x_{association.Source.Name}_{association.TargetPropertyName}"
                                             : association.JoinTableName;

                        segments.Add($"UsingEntity(x => x.ToTable(\"{tableMap}\"))");
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add("WithOne()");

                     break;
               }

               string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

               if (!string.IsNullOrEmpty(foreignKeySegment))
                  segments.Add(foreignKeySegment);

               if (association.Dependent == association.Target)
               {
                  if (association.SourceDeleteAction == DeleteAction.None)
                     segments.Add("OnDelete(DeleteBehavior.NoAction)");
                  else if (association.SourceDeleteAction == DeleteAction.Cascade)
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");
               }
               else if (association.Dependent == association.Source)
               {
                  if (association.TargetDeleteAction == DeleteAction.None)
                     segments.Add("OnDelete(DeleteBehavior.NoAction)");
                  else if (association.TargetDeleteAction == DeleteAction.Cascade)
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");
               }

               if (required)
                  segments.Add("IsRequired()");

               Output(segments);

               if (association.TargetAutoInclude)
                  Output($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName}).AutoInclude();");

               if (!association.TargetAutoProperty)
               {
                  segments.Add($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName})");

                  segments.Add(modelClass.ModelRoot.IsEFCore6Plus
                                  ? $"UsePropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})"
                                  : $"Metadata.SetPropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})");

                  Output(segments);
               }
            }
         }
      }
      #endregion Template
   }
}



