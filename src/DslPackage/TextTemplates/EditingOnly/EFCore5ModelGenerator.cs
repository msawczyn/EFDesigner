﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.0.1
      // Copyright (c) 2017-2020 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore5ModelGenerator : EFCoreModelGenerator
      {
         public EFCore5ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         protected override void WriteModelClasses(List<string> segments, ModelClass[] classesWithTables, List<string> foreignKeyColumns, List<Association> visited)
         {
            foreach (ModelClass modelClass in modelRoot.Classes.OrderBy(x => x.Name))
            {
               segments.Clear();
               foreignKeyColumns.Clear();
               NL();

               if (modelClass.IsPropertyBag)
                  WritePropertyBagClassBuilder(segments, classesWithTables, modelClass, visited, foreignKeyColumns);
               else
                  WriteStandardClassBuilder(segments, classesWithTables, modelClass, visited, foreignKeyColumns);
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected void WritePropertyBagClassBuilder(List<string> segments, ModelClass[] classesWithTables, ModelClass modelClass, List<Association> visited, List<string> foreignKeyColumns)
         {
            // class level
            string declaration = $"modelBuilder.SharedTypeEntity<Dictionary<string, object>>(\"{modelClass.Name}\")";
            segments.Add(declaration);

            foreach (ModelAttribute transient in modelClass.Attributes.Where(x => !x.Persistent))
               segments.Add($"Ignore(t => t.{transient.Name})");

            // note: this must come before the 'ToTable' call or there's a runtime error
            if (modelRoot.InheritanceStrategy == CodeStrategy.TablePerConcreteType && modelClass.Superclass != null)
               segments.Add("Map(x => x.MapInheritedProperties())");

            if (classesWithTables.Contains(modelClass))
            {
               segments.Add(string.IsNullOrEmpty(modelClass.DatabaseSchema) || modelClass.DatabaseSchema == modelClass.ModelRoot.DatabaseSchema
                               ? $"ToTable(\"{modelClass.TableName}\")"
                               : $"ToTable(\"{modelClass.TableName}\", \"{modelClass.DatabaseSchema}\")");

               // primary key code segments must be output last, since HasKey returns a different type
               List<ModelAttribute> identityAttributes = modelClass.IdentityAttributes.ToList();

               if (identityAttributes.Count == 1)
                  segments.Add($"HasKey(\"{identityAttributes[0].Name}\")");
               else if (identityAttributes.Count > 1)
                  segments.Add($"HasKey(\"{string.Join(", t.", identityAttributes.Select(ia => ia.Name))}\")");
            }

            if (segments.Count > 1)
               Output(segments);

            // attribute level
            WriteModelAttributes(segments, modelClass);

            bool hasDefinedConcurrencyToken = modelClass.AllAttributes.Any(x => x.IsConcurrencyToken);

            if (!hasDefinedConcurrencyToken && modelClass.EffectiveConcurrency == ConcurrencyOverride.Optimistic)
               Output($"{declaration}.IndexerProperty<byte[]>(\"Timestamp\").IsConcurrencyToken();");

            // Navigation endpoints are distingished as Source and Target. They are also distinguished as Principal
            // and Dependent. How do these map? Short answer: they don't. Source and Target are accidents of where the user started drawing the association.

            // What matters is the Principal and Dependent classifications, so we look at those. 
            // In the case of one-to-one or zero-to-one-to-zero-to-one, it's model dependent and the user has to tell us
            // In all other cases, we can tell by the cardinalities of the associations

            // navigation properties
            List<string> declaredShadowProperties = new List<string>();
            DefineUnidirectionalAssociations(modelClass, visited, segments, foreignKeyColumns, declaredShadowProperties);
            DefineBidirectionalAssociations(modelClass, visited, segments, foreignKeyColumns, declaredShadowProperties);
         }

         protected override void WriteModelAttributes(List<string> segments, ModelClass modelClass)
         {
            string declaration = modelClass.IsPropertyBag
                                    ? $"modelBuilder.SharedTypeEntity<Dictionary<string, object>>(\"{modelClass.Name}\")"
                                    : $"modelBuilder.{(modelClass.IsDependentType ? "Owned" : "Entity")}<{modelClass.FullName}>()";

            foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.Persistent && !SpatialTypes.Contains(x.Type)))
            {
               string attributeDeclaration = modelClass.IsPropertyBag 
                                                ? $"IndexerProperty<{modelAttribute.CLRType}>(\"{modelAttribute.Name}\")" 
                                                : $"Property(t => t.{modelAttribute.Name})";
               segments.Clear();

               if (modelClass.IsPropertyBag)
               {
                  segments.Add(declaration);
                  segments.Add(attributeDeclaration);
               }

               if ((modelAttribute.MaxLength ?? 0) > 0)
                  // ReSharper disable once PossibleInvalidOperationException
                  segments.Add($"HasMaxLength({modelAttribute.MaxLength.Value})");

               if (modelAttribute.Required)
                  segments.Add("IsRequired()");

               if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
                  segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

               if (!modelAttribute.AutoProperty)
               {
                  segments.Add($"HasField(\"{modelAttribute.BackingFieldName}\")");
                  segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{modelAttribute.PropertyAccessMode})");
               }

               if (!string.IsNullOrEmpty(modelAttribute.ColumnType) && modelAttribute.ColumnType.ToLowerInvariant() != "default")
               {
                  if (modelAttribute.ColumnType.ToLowerInvariant() == "varchar"
                   || modelAttribute.ColumnType.ToLowerInvariant() == "nvarchar"
                   || modelAttribute.ColumnType.ToLowerInvariant() == "char")
                     segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}({(modelAttribute.MaxLength > 0 ? modelAttribute.MaxLength.ToString() : "max")})\")");
                  else
                     segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}\")");
               }

               if (!string.IsNullOrEmpty(modelAttribute.InitialValue))
               {
                  string initialValue = modelAttribute.InitialValue;

                  // using switch statements since more exceptions will undoubtedly be created in the future
                  switch (modelAttribute.Type)
                  {
                     case "DateTime":
                        switch (modelAttribute.InitialValue)
                        {
                           case "DateTime.Now":
                              segments.Add("HasDefaultValue(DateTime.Now)");

                              //segments.Add("HasDefaultValueSql(\"getdate()\")");
                              break;

                           case "DateTime.UtcNow":
                              segments.Add("HasDefaultValue(DateTime.UtcNow)");

                              //segments.Add("HasDefaultValueSql(\"getutcdate()\")");
                              break;

                           default:
                              if (!initialValue.StartsWith("\""))
                                 initialValue = "\"" + initialValue;

                              if (!initialValue.EndsWith("\""))
                                 initialValue = initialValue + "\"";

                              segments.Add($"HasDefaultValue(DateTime.Parse({initialValue}))");

                              break;
                        }

                        break;

                     case "String":
                        if (!initialValue.StartsWith("\""))
                           initialValue = "\"" + initialValue;

                        if (!initialValue.EndsWith("\""))
                           initialValue = initialValue + "\"";

                        segments.Add($"HasDefaultValue({initialValue})");

                        break;

                     default:
                        segments.Add($"HasDefaultValue({modelAttribute.InitialValue})");

                        break;
                  }
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
                  if (!modelClass.IsPropertyBag)
                  {
                     segments.Insert(0, declaration);
                     segments.Insert(1, attributeDeclaration);
                  }

                  Output(segments);
               }

               if (modelAttribute.Indexed && !modelAttribute.IsIdentity)
               {
                  segments.Clear();

                  segments.Add(modelClass.IsPropertyBag 
                                  ? $"{declaration}.HasIndex(\"{modelAttribute.Name}\")" 
                                  : $"{declaration}.HasIndex(t => t.{modelAttribute.Name})");

                  if (modelAttribute.IndexedUnique)
                     segments.Add("IsUnique()");

                  Output(segments);
               }
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected override void DefineBidirectionalAssociations(ModelClass modelClass
                                             , List<Association> visited
                                             , List<string> segments
                                             , List<string> foreignKeyColumns
                                             , List<string> declaredShadowProperties)
         {
            string declaration = modelClass.IsPropertyBag
                                    ? $"modelBuilder.SharedTypeEntity<Dictionary<string, object>>(\"{modelClass.Name}\")"
                                    : $"modelBuilder.Entity<{modelClass.FullName}>()";

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (BidirectionalAssociation association in Association.GetLinksToTargets(modelClass)
                                                                        .OfType<BidirectionalAssociation>()
                                                                        .Where(x => x.Persistent && !x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               bool required = false;

               segments.Clear();
               segments.Add(declaration);

               string targetProperty = modelClass.IsPropertyBag 
                                             ? $"\"{association.TargetPropertyName}\"" 
                                             : $"x => x.{association.TargetPropertyName}";

               string sourceProperty = modelClass.IsPropertyBag 
                                          ? $"\"{association.SourcePropertyName}\"" 
                                          : $"x => x.{association.SourcePropertyName}";

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany({targetProperty})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne({targetProperty})");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne({targetProperty})");

                     break;
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"WithMany({sourceProperty})");

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName) ? $"{association.TargetPropertyName}_x_{association.TargetPropertyName}" : association.JoinTableName;
                        segments.Add($"UsingEntity(x => x.ToTable(\"{tableMap}\"))");
                     }

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"WithOne({sourceProperty})");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"WithOne({sourceProperty})");

                     break;
               }

               string foreignKeySegment = CreateForeignKeySegment(association, foreignKeyColumns);

               if (!string.IsNullOrEmpty(foreignKeySegment))
                  segments.Add(foreignKeySegment);

               if (required)
                  segments.Add("IsRequired()");

               if ((association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal) && !association.LinksDependentType)
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

               Output(segments);

               if (association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One && association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                  Output($"{declaration}.Navigation({targetProperty}).IsRequired();");
            }
         }

         [SuppressMessage("ReSharper", "RedundantNameQualifier")]
         protected override void DefineUnidirectionalAssociations(ModelClass modelClass
                                                  , List<Association> visited
                                                  , List<string> segments
                                                  , List<string> foreignKeyColumns
                                                  , List<string> declaredShadowProperties)
         {
            string declaration = modelClass.IsPropertyBag
                                    ? $"modelBuilder.SharedTypeEntity<Dictionary<string, object>>(\"{modelClass.Name}\")"
                                    : $"modelBuilder.Entity<{modelClass.FullName}>()";

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (UnidirectionalAssociation association in Association.GetLinksToTargets(modelClass)
                                                                         .OfType<UnidirectionalAssociation>()
                                                                         .Where(x => x.Persistent && !x.Target.IsDependentType))
            {
               if (visited.Contains(association))
                  continue;

               visited.Add(association);

               bool required = false;

               string targetProperty = modelClass.IsPropertyBag 
                                          ? $"\"{association.TargetPropertyName}\"" 
                                          : $"x => x.{association.TargetPropertyName}";

               segments.Clear();
               segments.Add(declaration);

               switch (association.TargetMultiplicity) // realized by property on source
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add($"HasMany({targetProperty})");

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.One:
                     segments.Add($"HasOne({targetProperty})");
                     required = (modelClass == association.Principal);

                     break;

                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroOne:
                     segments.Add($"HasOne({targetProperty})");

                     break;
               }

               switch (association.SourceMultiplicity) // realized by property on target, but no property on target
               {
                  case Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany:
                     segments.Add("WithMany()");

                     if (association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.ZeroMany)
                     {
                        string tableMap = string.IsNullOrEmpty(association.JoinTableName) ? $"{association.TargetPropertyName}_x_{association.TargetPropertyName}" : association.JoinTableName;
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

               if ((association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal) && !association.LinksDependentType)
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

               if (required)
                  segments.Add("IsRequired()");

               Output(segments);

               if (association.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One && association.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                  Output($"{declaration}.Navigation(x => x.{association.TargetPropertyName}).IsRequired();");
            }
         }
      }

      #endregion Template
   }
}