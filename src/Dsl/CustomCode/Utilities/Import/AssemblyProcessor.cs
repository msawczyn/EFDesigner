using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.Modeling;

using Newtonsoft.Json;

using ParsingModels;

using Sawczyn.EFDesigner.EFModel.Extensions;

// ReSharper disable UseObjectOrCollectionInitializer

namespace Sawczyn.EFDesigner.EFModel
{
   public class AssemblyProcessor : IFileProcessor
   {
      private readonly Store Store;

      public AssemblyProcessor(Store store)
      {
         Store = store;
      }

      private bool DoProcessing(string outputFilename)
      {
         try
         {
            using (StreamReader sr = new StreamReader(outputFilename))
            {
               string json = sr.ReadToEnd();
               ParsingModels.ModelRoot rootData = JsonConvert.DeserializeObject<ParsingModels.ModelRoot>(json);

               ProcessRootData(rootData);
               return true;
            }
         }
         catch (Exception e)
         {
            ErrorDisplay.Show($"Error procesing assembly: {e.Message}");
         }
         finally
         {
            if (!string.IsNullOrEmpty(outputFilename))
               File.Delete(outputFilename);
         }

         return false;
      }

      public bool Process(string filename)
      {
         if (filename == null)
            throw new ArgumentNullException(nameof(filename));

         string outputFilename = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
         StatusDisplay.Show("Detecting .NET and EF versions");

         string[] paths =
         {
            @"Parsers\EF6Parser.exe"
          , @"Parsers\EFCore2Parser.exe"
          , @"Parsers\EFCore3Parser.exe"
         };

         foreach (string path in paths)
         {
            if (TryParseAssembly(filename, path, outputFilename) == 0)
               return DoProcessing(outputFilename);
         }

         ErrorDisplay.Show($"Error processing assembly. See {Path.ChangeExtension(outputFilename, "log")} for further information");

         return false;
      }

#region ModelRoot

      private void ProcessRootData(ParsingModels.ModelRoot rootData)
      {
         ModelRoot modelRoot = Store.ModelRoot();

         modelRoot.EntityContainerName = rootData.EntityContainerName;
         modelRoot.Namespace = rootData.Namespace;

         ProcessClasses(modelRoot, rootData.Classes);
         ProcessEnumerations(modelRoot, rootData.Enumerations);
      }

      #endregion

      #region Classes

      private void ProcessClasses(ModelRoot modelRoot, List<ParsingModels.ModelClass> classDataList)
      {
         RemoveDuplicateBidirectionalAssociations(classDataList);

         foreach (ParsingModels.ModelClass data in classDataList)
         {
            StatusDisplay.Show($"Processing {data.FullName}");

            ModelClass element = modelRoot.Classes.FirstOrDefault(x => x.FullName == data.FullName);

            if (element == null)
            {
               element = new ModelClass(Store,
                                        new PropertyAssignment(ModelClass.NameDomainPropertyId, data.Name),
                                        new PropertyAssignment(ModelClass.NamespaceDomainPropertyId, data.Namespace),
                                        new PropertyAssignment(ModelClass.CustomAttributesDomainPropertyId, data.CustomAttributes),
                                        new PropertyAssignment(ModelClass.CustomInterfacesDomainPropertyId, data.CustomInterfaces),
                                        new PropertyAssignment(ModelClass.IsAbstractDomainPropertyId, data.IsAbstract),
                                        new PropertyAssignment(ModelClass.BaseClassDomainPropertyId, data.BaseClass),
                                        new PropertyAssignment(ModelClass.TableNameDomainPropertyId, data.TableName),
                                        new PropertyAssignment(ModelClass.IsDependentTypeDomainPropertyId, data.IsDependentType));

               modelRoot.Classes.Add(element);
            }
            else
            {
               element.Name = data.Name;
               element.Namespace = data.Namespace;
               element.CustomAttributes = data.CustomAttributes;
               element.CustomInterfaces = data.CustomInterfaces;
               element.IsAbstract = data.IsAbstract;
               element.BaseClass = data.BaseClass;
               element.TableName = data.TableName;
               element.IsDependentType = data.IsDependentType;
            }

            ProcessProperties(element, data.Properties);
         }


         // classes are all created, so we can work the associations
         foreach (ParsingModels.ModelClass data in classDataList)
         {
            ProcessUnidirectionalAssociations(data);
            ProcessBidirectionalAssociations(data);
         }
      }

      private static void RemoveDuplicateBidirectionalAssociations(List<ParsingModels.ModelClass> classDataList)
      { 
         // Bidirectional associations get duplicates, and it's better to clean them here than to rely on each parser version
         List<ModelBidirectionalAssociation> allBidirectionalAssociations = classDataList.SelectMany(cls => cls.BidirectionalAssociations).ToList();

         for (int index = 0; index < allBidirectionalAssociations.Count; index++)
         {
            ModelBidirectionalAssociation keeper = allBidirectionalAssociations[index];

            ModelBidirectionalAssociation duplicate =
               allBidirectionalAssociations.Skip(index)
                                           .FirstOrDefault(a => a.SourcePropertyTypeName == keeper.TargetPropertyTypeName
                                                             && a.SourcePropertyName == keeper.TargetPropertyName
                                                             && a.TargetPropertyTypeName == keeper.SourcePropertyTypeName
                                                             && a.TargetPropertyName == keeper.SourcePropertyName);

            if (duplicate != null)
            {
               // discard the one on the target
               ParsingModels.ModelClass duplicateOwner = classDataList.Single(c => c.FullName == duplicate.TargetClassFullName);
               duplicateOwner.BidirectionalAssociations.Remove(duplicate);
               allBidirectionalAssociations.Remove(duplicate);
            }
         }
      }

      private void ProcessProperties(ModelClass modelClass, List<ModelProperty> properties)
      {
         foreach (ModelProperty data in properties)
         {
            ModelAttribute element = modelClass.Attributes.FirstOrDefault(x => x.Name == data.Name);

            if (element == null)
            {
               // we've never seen this one before. Add it.
               element = new ModelAttribute(Store,
                                            new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, data.TypeName),
                                            new PropertyAssignment(ModelAttribute.NameDomainPropertyId, data.Name),
                                            new PropertyAssignment(ModelAttribute.CustomAttributesDomainPropertyId, data.CustomAttributes),
                                            new PropertyAssignment(ModelAttribute.IndexedDomainPropertyId, data.Indexed),
                                            new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, data.Required),
                                            new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, data.MaxStringLength),
                                            new PropertyAssignment(ModelAttribute.MinLengthDomainPropertyId, data.MinStringLength),
                                            new PropertyAssignment(ModelAttribute.IsIdentityDomainPropertyId, data.IsIdentity),
                                            new PropertyAssignment(ModelAttribute.IdentityTypeDomainPropertyId, data.IsIdentity 
                                                                                                                   ? data.IsIdentityGenerated ? IdentityType.AutoGenerated : IdentityType.Manual 
                                                                                                                   : IdentityType.None));
               modelClass.Attributes.Add(element);
            }
            else
            {
               // somehow, we have seen this before. Update it.
               element.Type = data.TypeName;
               element.Name = data.Name;
               element.CustomAttributes = data.CustomAttributes;
               element.Indexed = data.Indexed;
               element.Required = data.Required;
               element.MaxLength = data.MaxStringLength;
               element.MinLength = data.MinStringLength;
               element.IsIdentity = data.IsIdentity;

               element.IdentityType = data.IsIdentity
                                         ? data.IsIdentityGenerated ? IdentityType.AutoGenerated : IdentityType.Manual
                                         : IdentityType.None;
            }
         }
      }

      private void ProcessUnidirectionalAssociations(ParsingModels.ModelClass modelClass)
      {
         List<ModelUnidirectionalAssociation> unidirectionalAssociations = modelClass.UnidirectionalAssociations;

         foreach (ModelUnidirectionalAssociation data in unidirectionalAssociations)
         {
            UnidirectionalAssociation existing = Store.Get<UnidirectionalAssociation>()
                                                      .FirstOrDefault(x => x.Target.Name == data.TargetClassName
                                                                        && x.Source.Name == data.SourceClassName
                                                                        && x.Source.Name == modelClass.Name // just to be sure
                                                                        && x.TargetPropertyName == data.TargetPropertyName);

            if (existing != null)
            {
               if (string.IsNullOrWhiteSpace(existing.FKPropertyName) && !string.IsNullOrWhiteSpace(data.ForeignKey))
               {
                  existing.FKPropertyName = data.ForeignKey;
                  existing.Source.ModelRoot.ExposeForeignKeys = true;
               }

               continue;
            }

            ModelClass source = Store.Get<ModelClass>().FirstOrDefault(c => c.FullName == data.SourceClassFullName);
            ModelClass target = Store.Get<ModelClass>().FirstOrDefault(c => c.FullName == data.TargetClassFullName);

            if (source == null || target == null || source.FullName != modelClass.FullName)
               continue;

            // ReSharper disable once UnusedVariable
            UnidirectionalAssociation element = new UnidirectionalAssociation(Store,
                                                    new[]
                                                    {
                                                       new RoleAssignment(UnidirectionalAssociation.UnidirectionalSourceDomainRoleId, source),
                                                       new RoleAssignment(UnidirectionalAssociation.UnidirectionalTargetDomainRoleId, target)
                                                    },
                                                    new[]
                                                    {
                                                       new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, ConvertMultiplicity(data.SourceMultiplicity)),
                                                       new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, ConvertMultiplicity(data.TargetMultiplicity)),
                                                       new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, data.TargetPropertyName),
                                                       new PropertyAssignment(Association.TargetSummaryDomainPropertyId, data.TargetSummary),
                                                       new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, data.TargetDescription),
                                                       new PropertyAssignment(Association.FKPropertyNameDomainPropertyId, data.ForeignKey),
                                                       new PropertyAssignment(Association.SourceRoleDomainPropertyId, ConvertRole(data.SourceRole)), 
                                                       new PropertyAssignment(Association.TargetRoleDomainPropertyId, ConvertRole(data.TargetRole)), 
                                                    });

            AssociationChangeRules.SetEndpointRoles(element);
            element.EnsureForeignKeyAttributes();
         }
      }

      private void ProcessBidirectionalAssociations(ParsingModels.ModelClass modelClass)
      {
         List<ModelBidirectionalAssociation> bidirectionalAssociations = modelClass.BidirectionalAssociations;

         foreach (ModelBidirectionalAssociation data in bidirectionalAssociations)
         {
            BidirectionalAssociation existing = Store.Get<BidirectionalAssociation>()
                                                     .FirstOrDefault(x => x.Target.Name == data.TargetClassName
                                                                       && x.Source.Name == data.SourceClassName
                                                                       && x.Source.Name == modelClass.Name // just to be sure
                                                                       && x.TargetPropertyName == data.TargetPropertyName
                                                                       && x.SourcePropertyName == data.SourcePropertyName)
                                             ?? Store.Get<BidirectionalAssociation>()
                                                     .FirstOrDefault(x => x.Source.Name == data.TargetClassName
                                                                       && x.Target.Name == data.SourceClassName
                                                                       && x.Target.Name == modelClass.Name // just to be sure
                                                                       && x.SourcePropertyName == data.TargetPropertyName
                                                                       && x.TargetPropertyName == data.SourcePropertyName);

            if (existing != null)
            {
               if (string.IsNullOrWhiteSpace(existing.FKPropertyName) && !string.IsNullOrWhiteSpace(data.ForeignKey))
               {
                  existing.FKPropertyName = data.ForeignKey;
                  existing.Source.ModelRoot.ExposeForeignKeys = true;
               }

               continue;
            }

            ModelClass source = Store.Get<ModelClass>().FirstOrDefault(c => c.Name == data.SourceClassName);
            ModelClass target = Store.Get<ModelClass>().FirstOrDefault(c => c.Name == data.TargetClassName);

            if (source == null || target == null || source.FullName != modelClass.FullName)
               continue;

            // ReSharper disable once UnusedVariable
            BidirectionalAssociation element = new BidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(BidirectionalAssociation.BidirectionalSourceDomainRoleId, source),
                                                      new RoleAssignment(BidirectionalAssociation.BidirectionalTargetDomainRoleId, target)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, ConvertMultiplicity(data.SourceMultiplicity)),
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, ConvertMultiplicity(data.TargetMultiplicity)),
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, data.TargetPropertyName),
                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, data.TargetSummary),
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, data.TargetDescription),
                                                      new PropertyAssignment(Association.FKPropertyNameDomainPropertyId, data.ForeignKey),
                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, ConvertRole(data.SourceRole)), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, ConvertRole(data.TargetRole)), 
                                                      new PropertyAssignment(BidirectionalAssociation.SourcePropertyNameDomainPropertyId, data.SourcePropertyName),
                                                      new PropertyAssignment(BidirectionalAssociation.SourceSummaryDomainPropertyId, data.SourceSummary),
                                                      new PropertyAssignment(BidirectionalAssociation.SourceDescriptionDomainPropertyId, data.SourceDescription),
                                                   });

            AssociationChangeRules.SetEndpointRoles(element);
            element.EnsureForeignKeyAttributes();
         }
      }

      #endregion

      #region Enumerations

      private void ProcessEnumerations(ModelRoot modelRoot, List<ParsingModels.ModelEnum> enumDataList)
      {
         foreach (ParsingModels.ModelEnum data in enumDataList)
         {
            StatusDisplay.Show($"Processing {data.FullName}");
            ModelEnum element = modelRoot.Enums.FirstOrDefault(e => e.FullName == data.FullName);

            if (element == null)
            {
               element = new ModelEnum(Store,
                                       new PropertyAssignment(ModelEnum.NameDomainPropertyId, data.Name),
                                       new PropertyAssignment(ModelEnum.NamespaceDomainPropertyId, data.Namespace),
                                       new PropertyAssignment(ModelEnum.CustomAttributesDomainPropertyId, data.CustomAttributes),
                                       new PropertyAssignment(ModelEnum.IsFlagsDomainPropertyId, data.IsFlags));
               modelRoot.Enums.Add(element);
            }
            else
            {
               element.Name = data.Name;
               element.Namespace = data.Namespace;
               element.CustomAttributes = data.CustomAttributes;

               // TODO - deal with ValueType
               //element.ValueType = data.ValueType;
               element.IsFlags = data.IsFlags;
            }

            ProcessEnumerationValues(element, data.Values);
         }
      }

      private void ProcessEnumerationValues(ModelEnum modelEnum, List<ParsingModels.ModelEnumValue> enumValueList)
      {
         foreach (ParsingModels.ModelEnumValue data in enumValueList)
         {
            ModelEnumValue element = modelEnum.Values.FirstOrDefault(x => x.Name == data.Name);

            if (element == null)
            {
               element = new ModelEnumValue(Store,
                                            new PropertyAssignment(ModelEnumValue.NameDomainPropertyId, data.Name),
                                            new PropertyAssignment(ModelEnumValue.ValueDomainPropertyId, data.Value),
                                            new PropertyAssignment(ModelEnumValue.CustomAttributesDomainPropertyId, data.CustomAttributes),
                                            new PropertyAssignment(ModelEnumValue.DisplayTextDomainPropertyId, data.DisplayText));
               modelEnum.Values.Add(element);
            }
            else
            {
               element.Name = data.Name;
               element.Value = data.Value;
               element.CustomAttributes = data.CustomAttributes;
               element.DisplayText = data.DisplayText;
            }
         }
      }

      #endregion

      private int TryParseAssembly(string filename, string parserAssembly, string outputFilename)
      {
         string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), parserAssembly);
         ProcessStartInfo processStartInfo = new ProcessStartInfo(path)
         {
            Arguments = $"\"{filename.Trim('\"')}\" \"{outputFilename}\"",
            CreateNoWindow = true,
            ErrorDialog = false,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = true
         };

         using (Process process = System.Diagnostics.Process.Start(processStartInfo))
         {
            process.WaitForExit();
            return process.ExitCode;
         }
      }

      private Multiplicity ConvertMultiplicity(ParsingModels.Multiplicity data)
      {
         switch (data)
         {
            case ParsingModels.Multiplicity.ZeroMany:
               return Multiplicity.ZeroMany;

            case ParsingModels.Multiplicity.One:
               return Multiplicity.One;

            case ParsingModels.Multiplicity.ZeroOne:
               return Multiplicity.ZeroOne;
         }

         return Multiplicity.ZeroOne;
      }

      private EndpointRole ConvertRole(AssociationRole data)
      {
         switch (data)
         {
            case AssociationRole.Dependent:
               return EndpointRole.Dependent;

            case AssociationRole.Principal:
               return EndpointRole.Principal;

            case AssociationRole.NotApplicable:
               return EndpointRole.NotApplicable;
         }

         return EndpointRole.NotSet;
      }

   }
}