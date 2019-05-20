// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.Modeling;

using Newtonsoft.Json;

using ParsingModels;
// ReSharper disable UseObjectOrCollectionInitializer

namespace Sawczyn.EFDesigner.EFModel
{
   public class AssemblyProcessor : FileProcessor
   {
      private readonly Store Store;

      public static string AssemblyDirectory
      {
         get
         {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path);
         }
      }

      public AssemblyProcessor(Store store)
      {
         Store = store;
      }

      public bool Process(string filename)
      {
         if (filename == null)
            throw new ArgumentNullException(nameof(filename));

         string outputFilename = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
         
         if (TryParseAssembly(filename, "Parsers\\EF6Parser.exe", outputFilename, "Trying EF6") == 0 ||
             TryParseAssembly(filename, "Parsers\\EFCoreParser.exe", outputFilename, "Trying EFCore") == 0)
            return ProcessAssemblyData(outputFilename);

         return false;
      }

      private bool ProcessAssemblyData(string outputFilename)
      {
         try
         {
            using (StringReader sr = new StringReader(outputFilename))
            {
               string json = sr.ReadToEnd();
               ParsingModels.ModelRoot rootData = JsonConvert.DeserializeObject<ParsingModels.ModelRoot>(json);

               ProcessRootData(rootData);
            }
         }
         catch (Exception e)
         {
            ErrorDisplay.Show($"Error applying processed assembly: {e.Message}");

            return false;
         }

         return true;
      }

      private void ProcessRootData(ParsingModels.ModelRoot rootData)
      {
         ModelRoot modelRoot = Store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();

         modelRoot.Namespace = rootData.Namespace;

         ProcessClasses(modelRoot, rootData.Classes);
         ProcessEnumerations(modelRoot, rootData.Enumerations);
      }

      private void ProcessClasses(ModelRoot modelRoot, List<ParsingModels.ModelClass> classDataList)
      {
         foreach (ParsingModels.ModelClass data in classDataList)
         {
            ModelClass element = modelRoot.Classes.FirstOrDefault(x => x.FullName == data.FullName) ?? new ModelClass(Store);

            element.Name = data.Name;
            element.Namespace = data.Namespace;
            element.CustomAttributes = data.CustomAttributes;
            element.CustomInterfaces = data.CustomInterfaces;
            element.IsAbstract = data.IsAbstract;
            element.BaseClass = data.BaseClass;
            element.TableName = data.TableName;
            element.IsDependentType = data.IsDependentType;

            ProcessProperties(element, data.Properties);
            ProcessUnidirectionalAssociations(data.UnidirectionalAssociations);
            ProcessBidirectionalAssociations(data.BidirectionalAssociations);

            modelRoot.Classes.Add(element);
         }
      }

      private void ProcessProperties(ModelClass modelClass, List<ModelProperty> properties)
      {
         foreach (ModelProperty data in properties)
         {
            ModelAttribute element = new ModelAttribute(Store);

            element.Type = data.TypeName;
            element.Name = data.Name;
            element.CustomAttributes = data.CustomAttributes;
            element.Indexed = data.Indexed;
            element.Required = data.Required;
            element.MaxLength = data.MaxStringLength;
            element.MinLength = data.MinStringLength;
            element.IsIdentity = data.IsIdentity;

            modelClass.Attributes.Add(element);
         }
      }

      private void ProcessUnidirectionalAssociations(List<ModelUnidirectionalAssociation> unidirectionalAssociations)
      {
         foreach (ModelUnidirectionalAssociation data in unidirectionalAssociations)
         {
            ModelClass source = Store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(c => c.Name == data.SourceClassName && c.Namespace == data.SourceClassNamespace) ?? 
                                new ModelClass(Store, 
                                               new PropertyAssignment(ModelClass.NameDomainPropertyId, data.SourceClassName),
                                               new PropertyAssignment(ModelClass.NamespaceDomainPropertyId, data.SourceClassNamespace));

            ModelClass target = Store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(c => c.Name == data.TargetClassName && c.Namespace == data.TargetClassNamespace) ??
                                new ModelClass(Store,
                                               new PropertyAssignment(ModelClass.NameDomainPropertyId, data.TargetClassName),
                                               new PropertyAssignment(ModelClass.NamespaceDomainPropertyId, data.TargetClassNamespace));

            UnidirectionalAssociation element = new UnidirectionalAssociation(source, target);
            element.SourceMultiplicity = ConvertMultiplicity(data.SourceMultiplicity);
            element.TargetMultiplicity = ConvertMultiplicity(data.TargetMultiplicity);
            element.TargetPropertyName = data.TargetPropertyName;
            element.TargetSummary = data.TargetSummary;
            element.TargetDescription = data.TargetDescription;
         }
      }

      private void ProcessBidirectionalAssociations(List<ModelBidirectionalAssociation> bidirectionalAssociations)
      {
         foreach (ModelBidirectionalAssociation data in bidirectionalAssociations)
         {
            ModelClass source = Store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(c => c.Name == data.SourceClassName && c.Namespace == data.SourceClassNamespace) ?? 
                                new ModelClass(Store, 
                                               new PropertyAssignment(ModelClass.NameDomainPropertyId, data.SourceClassName),
                                               new PropertyAssignment(ModelClass.NamespaceDomainPropertyId, data.SourceClassNamespace));

            ModelClass target = Store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(c => c.Name == data.TargetClassName && c.Namespace == data.TargetClassNamespace) ??
                                new ModelClass(Store,
                                               new PropertyAssignment(ModelClass.NameDomainPropertyId, data.TargetClassName),
                                               new PropertyAssignment(ModelClass.NamespaceDomainPropertyId, data.TargetClassNamespace));

            BidirectionalAssociation element = new BidirectionalAssociation(source, target);
            element.SourceMultiplicity = ConvertMultiplicity(data.SourceMultiplicity);
            element.SourcePropertyName = data.SourcePropertyName;
            element.SourceSummary = data.SourceSummary;
            element.SourceDescription = data.SourceDescription;

            element.TargetMultiplicity = ConvertMultiplicity(data.TargetMultiplicity);
            element.TargetPropertyName = data.TargetPropertyName;
            element.TargetSummary = data.TargetSummary;
            element.TargetDescription = data.TargetDescription;
         }
      }

      private void ProcessEnumerations(ModelRoot modelRoot, List<ParsingModels.ModelEnum> enumDataList)
      {
         foreach (ParsingModels.ModelEnum data in enumDataList)
         {
            ModelEnum element = Store.ElementDirectory.AllElements.OfType<ModelEnum>().FirstOrDefault(e => e.Name == data.Name && e.Namespace == data.Namespace) ??
                                new ModelEnum(Store);

            element.Name = data.Name;
            element.Namespace = data.Namespace;
            element.CustomAttributes = data.CustomAttributes;
            //element.ValueType = data.ValueType;
            element.IsFlags = data.IsFlags;

            ProcessEnumerationValues(element, data.Values);

            modelRoot.Enums.Add(element);
         }
      }

      private void ProcessEnumerationValues(ModelEnum modelEnum, List<ParsingModels.ModelEnumValue> enumValueList)
      {
         modelEnum.Values.Clear();
         foreach (ParsingModels.ModelEnumValue data in enumValueList)
         {
            ModelEnumValue element = new ModelEnumValue(Store);
            element.Name = data.Name;
            element.Value = data.Value;
            element.CustomAttributes = data.CustomAttributes;
            element.DisplayText = data.DisplayText;

            modelEnum.Values.Add(element);
         }
      }

      private int TryParseAssembly(string filename, string parserAssembly, string outputFilename, string errorMessagePrefix)
      {
         int exitCode;

         ProcessStartInfo processStartInfo = new ProcessStartInfo(Path.Combine(AssemblyDirectory, parserAssembly))
                                             {
                                                Arguments = $"\"{filename.Trim('\"')}\" \"{outputFilename}\"", 
                                                CreateNoWindow = true, 
                                                ErrorDialog = false,
                                                UseShellExecute = false
                                             };

         using (Process process = System.Diagnostics.Process.Start(processStartInfo))
         {
            process.WaitForExit();
            exitCode = process.ExitCode;
         }

         string msgPrefix = string.IsNullOrEmpty(errorMessagePrefix)
                               ? $"{errorMessagePrefix}: "
                               : "";

         switch (exitCode)
         {
            case FileDropHelper.BAD_ARGUMENT_COUNT:
               ErrorDisplay.Show($"{msgPrefix}Internal error");

               break;

            case FileDropHelper.CANNOT_LOAD_ASSEMBLY:
               ErrorDisplay.Show($"{msgPrefix}Can't load assembly {filename}");

               break;

            case FileDropHelper.CANNOT_WRITE_OUTPUTFILE:
               ErrorDisplay.Show($"{msgPrefix}Can't write temporary file {outputFilename}");

               break;

            case FileDropHelper.CANNOT_CREATE_DBCONTEXT:
               ErrorDisplay.Show($"{msgPrefix}Can't create DbContext object");

               break;

            case FileDropHelper.CANNOT_FIND_APPROPRIATE_CONSTRUCTOR:
               ErrorDisplay.Show($"{msgPrefix}Can't find proper constructor in DbContext class. Class must have a constructor that takes one string parameter that's its connection string.");

               break;

            case FileDropHelper.AMBIGUOUS_REQUEST:
               ErrorDisplay.Show($"{msgPrefix}Found more than one DbContext class in the assembly. Don't know which one to process.");

               break;
         }

         return exitCode;
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

   }
}