using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.0.4
      // Copyright (c) 2017-2020 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore5ModelGenerator : EFCore3ModelGenerator
      {
         public EFCore5ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected override void ConfigureBidirectionalAssociations(ModelClass modelClass
                                                                  , List<Association> visited
                                                                  , List<string> foreignKeyColumns
                                                                  , List<string> declaredShadowProperties)
         {
            MakeNonDependentAssociations();
            MakeDependentAssociations(modelClass, $"modelBuilder.Entity<{modelClass.FullName}>()");

            void MakeNonDependentAssociations()
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
                           segments.Add("OnDelete(DeleteBehavior.NoAction)");

                           break;

                        case DeleteAction.Cascade:
                           segments.Add("OnDelete(DeleteBehavior.Cascade)");

                           break;
                     }
                  }

                  if (required
                   && (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                    || association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
                     segments.Add("IsRequired()");

                  Output(segments);

                  if (!association.TargetAutoProperty)
                  {
                     segments.Add($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName})");
                     segments.Add($"HasField(\"{association.TargetBackingFieldName}\")");
                     segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})");
                     Output(segments);
                  }

                  if (!association.SourceAutoProperty)
                  {
                     segments.Add($"modelBuilder.Entity<{association.Target.FullName}>().Navigation(e => e.{association.SourcePropertyName})");
                     segments.Add($"HasField(\"{association.SourceBackingFieldName}\")");
                     segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{association.SourcePropertyAccessMode})");
                     Output(segments);
                  }
               }
            }

            void MakeDependentAssociations(ModelClass sourceInstance, string baseSegment)
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
                        segments.Add($"HasForeignKey(\"{association.SourcePropertyName}Id\")");
                        Output(segments);

                        segments.Add(baseSegment);
                        segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                        segments.Add("Property<int>(\"Id\")");

                        Output(segments);

                        segments.Add(baseSegment);
                        segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                        segments.Add("HasKey(\"Id\")");

                        Output(segments);

                        MakeDependentAssociations(association.Target, $"{baseSegment}.OwnsMany(p => p.{association.TargetPropertyName})");

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

                        MakeDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})");

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

                        MakeDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})");

                        break;
                     }
                  }
               }
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected override void ConfigureUnidirectionalAssociations(ModelClass modelClass
                                                                   , List<Association> visited
                                                                   , List<string> foreignKeyColumns
                                                                   , List<string> declaredShadowProperties)
         {
            MakeNonDependentAssociations();
            MakeDependentAssociations(modelClass, $"modelBuilder.Entity<{modelClass.FullName}>()");

            void MakeNonDependentAssociations()
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
                           segments.Add("OnDelete(DeleteBehavior.NoAction)");

                           break;

                        case DeleteAction.Cascade:
                           segments.Add("OnDelete(DeleteBehavior.Cascade)");

                           break;
                     }
                  }

                  if (required
                   && (association.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                    || association.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
                     segments.Add("IsRequired()");

                  Output(segments);

                  if (!association.TargetAutoProperty)
                  {
                     segments.Add($"modelBuilder.Entity<{association.Source.FullName}>().Navigation(e => e.{association.TargetPropertyName})");
                     segments.Add($"HasField(\"{association.TargetBackingFieldName}\")");
                     segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{association.TargetPropertyAccessMode})");
                     Output(segments);
                  }
               }
            }

            void MakeDependentAssociations(ModelClass sourceInstance, string baseSegment)
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

                  switch (association.TargetMultiplicity) // realized by property on source
                  {
                     case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     {
                        segments.Add(baseSegment);
                        segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                        segments.Add($"WithOwner(\"{association.Source.Name}_{association.TargetPropertyName}\")");
                        segments.Add($"HasForeignKey(\"{association.Source.Name}_{association.TargetPropertyName}Id\")");
                        Output(segments);

                        segments.Add(baseSegment);
                        segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                        segments.Add("Property<int>(\"Id\")");

                        Output(segments);

                        segments.Add(baseSegment);
                        segments.Add($"OwnsMany(p => p.{association.TargetPropertyName})");
                        segments.Add("HasKey(\"Id\")");

                        Output(segments);

                        MakeDependentAssociations(association.Target, $"{baseSegment}.OwnsMany(p => p.{association.TargetPropertyName})");

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

                        MakeDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})");

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

                        MakeDependentAssociations(association.Target, $"{baseSegment}.OwnsOne(p => p.{association.TargetPropertyName})");

                        break;
                     }
                  }
               }
            }
         }
      }

      #endregion Template
   }
}
