using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
// ReSharper disable RedundantNameQualifier

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.3
      // Copyright (c) 2017-2021 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore5ModelGenerator : EFCore3ModelGenerator
      {
         public EFCore5ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         protected override void ConfigureTable(List<string> segments, ModelClass modelClass)
         {
            string tableName = string.IsNullOrEmpty(modelClass.TableName) ? modelClass.Name : modelClass.TableName;
            string schema = string.IsNullOrEmpty(modelClass.DatabaseSchema) || modelClass.DatabaseSchema == modelClass.ModelRoot.DatabaseSchema ? string.Empty : ", \"{modelClass.DatabaseSchema}\"";
            string buildAction = modelClass.ExcludeFromMigrations ? ", t => t.ExcludeFromMigrations()" : string.Empty;

            segments.Add($"ToTable(\"{tableName}\"{schema}{buildAction})");

            // primary key code segments must be output last, since HasKey returns a different type
            List<ModelAttribute> identityAttributes = modelClass.IdentityAttributes.ToList();

            if (identityAttributes.Count == 1)
               segments.Add($"HasKey(t => t.{identityAttributes[0].Name})");
            else if (identityAttributes.Count > 1)
               segments.Add($"HasKey(t => new {{ t.{string.Join(", t.", identityAttributes.Select(ia => ia.Name))} }})");
         }

         protected override void WriteRequiredNavigationsInConstructorBody(ModelClass modelClass, ref int lineCount)
         {
            // don't do this for EFCore5+. Adding these prevents EFCore from assigning the object when deserializing
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
                  string enumFQN = modelAttribute.ModelClass.ModelRoot.Enums.FirstOrDefault(e => e.Name == enumName).FullName;
                  segments.Add($"HasDefaultValue({enumFQN}.{enumValue.Trim()})");
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

            return segments;
         }

         protected override void WriteOnModelCreate(List<string> segments, ModelClass[] classesWithTables)
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
               bool sourceRequired = false;
               bool targetRequired = false;

               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     {
                        segments.Add($"HasMany<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                        break;
                     }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     {
                        segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");
                        targetRequired = true;

                        break;
                     }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     {
                        segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                        break;
                     }
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     {
                        segments.Add($"WithMany(p => p.{association.SourcePropertyName})");

                        if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                        {
                           string tableMap = string.IsNullOrEmpty(association.JoinTableName)
                                                ? $"{association.Target.Name}_{association.SourcePropertyName}_x_{association.Source.Name}_{association.TargetPropertyName}"
                                                : association.JoinTableName;

                           segments.Add($"UsingEntity(x => x.ToTable(\"{tableMap.Trim('"')}\"))");
                        }

                        break;
                     }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     {
                        segments.Add($"WithOne(p => p.{association.SourcePropertyName})");
                        sourceRequired = true;

                        break;
                     }

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"WithOne(p => p.{association.SourcePropertyName})");

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

                  if (targetRequired)
                     segments.Add("IsRequired()");
               }
               else if (association.Dependent == association.Source)
               {
                  if (association.TargetDeleteAction == DeleteAction.None)
                     segments.Add("OnDelete(DeleteBehavior.NoAction)");
                  else if (association.TargetDeleteAction == DeleteAction.Cascade)
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");

                  if (sourceRequired)
                     segments.Add("IsRequired()");
               }

               Output(segments);

               if (association.Principal == association.Target && targetRequired)
                  Output($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName}).IsRequired();");
               else if (association.Principal == association.Source && sourceRequired)
                  Output($"modelBuilder.Entity<{association.Target.FullName}>().Navigation(e => e.{association.SourcePropertyName}).IsRequired();");

               if (association.TargetAutoInclude)
                  Output($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName}).AutoInclude();");
               else if (association.SourceAutoInclude)
                  Output($"modelBuilder.Entity<{association.Target.FullName}>().Navigation(e => e.{association.SourcePropertyName}).AutoInclude();");
            }
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
               bool sourceRequired = false;
               bool targetRequired = false;

               segments.Add($"modelBuilder.Entity<{modelClass.FullName}>()");

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne<{association.Target.FullName}>(p => p.{association.TargetPropertyName})");
                     targetRequired = true;

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
                     sourceRequired = true;

                     break;

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

                  if (targetRequired)
                     segments.Add("IsRequired()");
               }
               else if (association.Dependent == association.Source)
               {
                  if (association.TargetDeleteAction == DeleteAction.None)
                     segments.Add("OnDelete(DeleteBehavior.NoAction)");
                  else if (association.TargetDeleteAction == DeleteAction.Cascade)
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");

                  if (sourceRequired)
                     segments.Add("IsRequired()");
               }

               Output(segments);

               if (association.Principal == association.Target && targetRequired)
                  Output($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName}).IsRequired();");

               if (association.TargetAutoInclude)
                  Output($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName}).AutoInclude();");

               if (!association.TargetAutoProperty)
               {
                  segments.Add($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName})");
                  segments.Add($"HasField(\"{association.TargetBackingFieldName}\")");
                  segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})");
                  Output(segments);
               }
            }
         }
      }
      #endregion Template
   }
}
