using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.CustomCode.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{

   internal class FileDropHelper
   {
      private static Func<IEnumerable<string>> FileSelectionHandler;

      public static void RegisterFileSelectionHandler(Func<IEnumerable<string>> fileSelectionHandler)
      {
         // called from EFModelDocData.OnDocumentLoaded
         FileSelectionHandler = fileSelectionHandler;
      }
      
      public static IEnumerable<string> SelectedFilePaths => FileSelectionHandler == null
                                                                ? new string[0] 
                                                                : FileSelectionHandler().ToArray();

      public static void HandleDrop(Store store, string filename)
      {
         if (string.IsNullOrEmpty(filename))
            return;

         // read the file
         string fileContents = File.ReadAllText(filename);

         try
         {
            // parse the contents
            SyntaxTree tree = CSharpSyntaxTree.ParseText(fileContents);

            if (tree.GetRoot() is CompilationUnitSyntax root)
            {
               List<ClassDeclarationSyntax> classDecls = root.DescendantNodes()
                                                             .OfType<ClassDeclarationSyntax>()
                                                             .Where(classDecl => classDecl.BaseList == null || 
                                                                                 classDecl.BaseList.Types.FirstOrDefault()?.ToString() != "DbContext")
                                                             .ToList();
               List<EnumDeclarationSyntax> enumDecls = root.DescendantNodes().OfType<EnumDeclarationSyntax>().ToList();

               if (!classDecls.Any() && !enumDecls.Any())
               {
                  WarningDisplay.Show("Couldn't find any classes or enums to add to the model");

                  return;
               }
               
               List<ClassDeclarationSyntax> classDeclarations = new List<ClassDeclarationSyntax>();

               using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped class"))
               {

                  foreach (ClassDeclarationSyntax classDecl in classDecls)
                  {
                     classDeclarations.Add(classDecl);
                     ProcessClass(store, classDecl);
                  }

                  foreach (EnumDeclarationSyntax enumDecl in enumDecls)
                     ProcessEnum(store, enumDecl);

                  // process last so all classes and enums are already in the model
                  foreach (ClassDeclarationSyntax classDecl in classDeclarations)
                     ProcessProperties(store, classDecl);

                  tx.Commit();
               }
            }
         }
         catch
         {
            ErrorDisplay.Show("Error interpretting " + filename);
         }
      }

      private static void ProcessProperties(Store store, ClassDeclarationSyntax classDecl)
      {
         string className = classDecl.Identifier.Text;
         ModelRoot modelRoot = store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();
         ModelClass modelClass = store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(c => c.Name == className);
         modelClass.Attributes.Clear();

         foreach (PropertyDeclarationSyntax propertyDecl in classDecl.DescendantNodes().OfType<PropertyDeclarationSyntax>())
         {
            string propertyName = propertyDecl.Identifier.ToString();
            string propertyType = propertyDecl.Type.ToString();
            ModelClass target = modelRoot.Types.OfType<ModelClass>().FirstOrDefault(t => t.Name == propertyType);

            // is the property type a generic?
            // assume it's a list
            // TODO: this isn't a good idea. Fix later
            if (propertyDecl.ChildNodes().OfType<GenericNameSyntax>().Any())
            {
               GenericNameSyntax genericDecl = propertyDecl.ChildNodes().OfType<GenericNameSyntax>().FirstOrDefault();
               List<string> contentTypes = genericDecl.DescendantNodes().OfType<IdentifierNameSyntax>().Select(i => i.Identifier.ToString()).ToList();

               // there can only be one generic argument
               if (contentTypes.Count != 1)
               {
                  WarningDisplay.Show($"Found {className}.{propertyName}, but its type isn't anything expected. Ignoring...");
                  continue;
               }

               propertyType = contentTypes[0];
               target = modelRoot.Types.OfType<ModelClass>().FirstOrDefault(t => t.Name == propertyType);

               if (target == null)
               {
                  target = new ModelClass(store, new PropertyAssignment(ModelClass.NameDomainPropertyId, propertyType));
                  modelRoot.Types.Add(target);
               }

               ProcessAssociation(modelClass, target, propertyDecl, true);

               continue;
            }

            // is the property type an existing ModelClass?
            if (target != null)
            {
               ProcessAssociation(modelClass, target, propertyDecl);

               continue;
            }

            // is the property type something we don't know about?
            if (!ModelAttribute.IsValidCLRType(propertyType))
            {
               // assume it's a class and create the class
               target = new ModelClass(store, new PropertyAssignment(ModelClass.NameDomainPropertyId, propertyType));
               modelRoot.Types.Add(target);

               ProcessAssociation(modelClass, target, propertyDecl);

               continue;
            }

            // if we'ref here, it's just a property
            try
            {
               // ReSharper disable once UseObjectOrCollectionInitializer
               ModelAttribute modelAttribute = new ModelAttribute(store, new PropertyAssignment(ModelAttribute.NameDomainPropertyId, propertyName));
               modelAttribute.Type = ModelAttribute.ToCLRType(propertyDecl.Type.ToString()).Trim('?');
               modelAttribute.Required = propertyDecl.HasAttribute("RequiredAttribute") || !propertyDecl.DescendantNodes().OfType<NullableTypeSyntax>().Any();
               modelAttribute.Indexed = propertyDecl.HasAttribute("IndexedAttribute");
               modelAttribute.IsIdentity = propertyDecl.HasAttribute("KeyAttribute");
               modelAttribute.Virtual = propertyDecl.DescendantTokens().Any(t => t.IsKind(SyntaxKind.VirtualKeyword));

               if (modelAttribute.Type.ToLower() == "string")
               {
                  AttributeSyntax maxLengthAttribute = propertyDecl.GetAttribute("MaxLengthAttribute");
                  AttributeArgumentSyntax maxLength = maxLengthAttribute?.GetAttributeArguments()?.FirstOrDefault();

                  if (maxLength != null)
                     modelAttribute.MaxLength = int.TryParse(maxLength.Expression.ToString(), out int maxLengthValue)
                                                   ? maxLengthValue
                                                   : 0;

                  AttributeSyntax minLengthAttribute = propertyDecl.GetAttribute("MinLengthAttribute");
                  AttributeArgumentSyntax minLength = minLengthAttribute?.GetAttributeArguments()?.FirstOrDefault();

                  if (minLength != null)
                     modelAttribute.MinLength = int.TryParse(minLength.Expression.ToString(), out int minLengthValue)
                                                   ? minLengthValue
                                                   : 0;
               }

               AccessorDeclarationSyntax getAccessor = (AccessorDeclarationSyntax)propertyDecl.DescendantNodes()
                                                                                              .FirstOrDefault(node => node.IsKind(SyntaxKind.GetAccessorDeclaration));
               AccessorDeclarationSyntax setAccessor = (AccessorDeclarationSyntax)propertyDecl.DescendantNodes()
                                                                                              .FirstOrDefault(node => node.IsKind(SyntaxKind.SetAccessorDeclaration));

               modelAttribute.AutoProperty = !getAccessor.DescendantNodes().Any(node => node.IsKind(SyntaxKind.Block)) && 
                                             !setAccessor.DescendantNodes().Any(node => node.IsKind(SyntaxKind.Block));

               modelAttribute.SetterVisibility = setAccessor.Modifiers.Any(m => m.ToString() == "protected")
                                                    ? SetterAccessModifier.Protected
                                                    : setAccessor.Modifiers.Any(m => m.ToString() == "internal")
                                                       ? SetterAccessModifier.Internal
                                                       : SetterAccessModifier.Public;

               XMLDocumentation xmlDocumentation = ProcessXMLDocumentation(propertyDecl);
               modelAttribute.Summary = xmlDocumentation.Summary;
               modelAttribute.Description = xmlDocumentation.Description;

               modelClass.Attributes.Add(modelAttribute);
            }
            catch
            {
               WarningDisplay.Show($"Could not parse '{className}.{propertyDecl.Identifier}'. The property will be discarded.");
            }
         }

      }

      private static void ProcessAssociation(ModelClass source, ModelClass target, PropertyDeclarationSyntax propertyDecl, bool toMany = false)
      {
         string propertyName = propertyDecl.Identifier.ToString();

         // since we don't have enough information from the code, we'll create unidirectional associations
         // cardinality 1 on the source end, 0..1 or 0..* on the target, depending on the parameter
         XMLDocumentation xmlDocumentation = ProcessXMLDocumentation(propertyDecl);
         
         // ReSharper disable once UseObjectOrCollectionInitializer
         UnidirectionalAssociation association = new UnidirectionalAssociation(source, target);
         association.SourceMultiplicity = Multiplicity.One;

         association.TargetMultiplicity = toMany
                                             ? Multiplicity.ZeroMany
                                             : Multiplicity.ZeroOne;

         association.TargetPropertyName = propertyName;
         association.TargetSummary = xmlDocumentation.Summary;
         association.TargetDescription = xmlDocumentation.Description;
      }

      private static void ProcessEnum(Store store, EnumDeclarationSyntax enumDecl, NamespaceDeclarationSyntax namespaceDecl = null)
      {
         ModelRoot modelRoot = store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();
         string enumName = enumDecl.Identifier.Text;

         if (namespaceDecl == null && enumDecl.Parent is NamespaceDeclarationSyntax enumDeclParent)
            namespaceDecl = enumDeclParent;

         string namespaceName = namespaceDecl?.Name?.ToString() ?? modelRoot.Namespace;

         if (store.ElementDirectory.AllElements.OfType<ModelClass>().Any(c => c.Name == enumName) || 
             store.ElementDirectory.AllElements.OfType<ModelEnum>().Any(c => c.Name == enumName))
         {
            ErrorDisplay.Show($"'{enumName}' already exists in model.");

            return;
         }

         ModelEnum modelEnum = new ModelEnum(store, new PropertyAssignment(ModelEnum.NameDomainPropertyId, enumName))
                               {
                                  Namespace = namespaceName
                                , IsFlags = enumDecl.HasAttribute("Flags")
                               };

         SimpleBaseTypeSyntax baseTypeSyntax = enumDecl.DescendantNodes().OfType<SimpleBaseTypeSyntax>().FirstOrDefault();

         if (baseTypeSyntax != null)
         {
            switch (baseTypeSyntax.Type.ToString())
            {
               case "Int16":
               case "short":
                  modelEnum.ValueType = EnumValueType.Int16;

                  break;
               case "Int32":
               case "int":
                  modelEnum.ValueType = EnumValueType.Int32;

                  break;
               case "Int64":
               case "long":
                  modelEnum.ValueType = EnumValueType.Int64;

                  break;
               default:
                  WarningDisplay.Show($"Could not resolve value type for '{enumName}'. The enum will default to an Int32 value type.");

                  break;
            }
         }

         XMLDocumentation xmlDocumentation;

         foreach (EnumMemberDeclarationSyntax enumValueDecl in enumDecl.DescendantNodes().OfType<EnumMemberDeclarationSyntax>())
         {
            ModelEnumValue enumValue = new ModelEnumValue(store, new PropertyAssignment(ModelEnumValue.NameDomainPropertyId, enumValueDecl.Identifier.ToString()));
            EqualsValueClauseSyntax valueDecl = enumValueDecl.DescendantNodes().OfType<EqualsValueClauseSyntax>().FirstOrDefault();

            if (valueDecl != null)
               enumValue.Value = valueDecl.Value.ToString();

            xmlDocumentation = ProcessXMLDocumentation(enumValueDecl);
            enumValue.Summary = xmlDocumentation.Summary;
            enumValue.Description = xmlDocumentation.Description;

            modelEnum.Values.Add(enumValue);
         }

         xmlDocumentation = ProcessXMLDocumentation(enumDecl);
         modelEnum.Summary = xmlDocumentation.Summary;
         modelEnum.Description = xmlDocumentation.Description;

         modelRoot.Enums.Add(modelEnum);
      }

      private static void ProcessClass(Store store, ClassDeclarationSyntax classDecl, NamespaceDeclarationSyntax namespaceDecl = null)
      {
         ModelRoot modelRoot = store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();
         string className = classDecl.Identifier.Text;

         if (namespaceDecl == null && classDecl.Parent is NamespaceDeclarationSyntax classDeclParent)
            namespaceDecl = classDeclParent;

         if (store.ElementDirectory.AllElements.OfType<ModelEnum>().Any(c => c.Name == className))
         {
            ErrorDisplay.Show($"'{className}' already exists in model as an Enum.");

            return;
         }

         if (classDecl.TypeParameterList != null)
         {
            ErrorDisplay.Show($"Can't add generic class '{className}'.");

            return;
         }

         ModelClass modelClass = store.ElementDirectory
                                      .AllElements
                                      .OfType<ModelClass>()
                                      .FirstOrDefault(c => c.Name == className);

         if (modelClass == null)
         {
            modelClass = new ModelClass(store, new PropertyAssignment(ModelClass.NameDomainPropertyId, className))
                         {
                            Namespace = namespaceDecl?.Name?.ToString() ?? modelRoot.Namespace
                          , IsAbstract = classDecl.DescendantNodes().Any(n => n.Kind() == SyntaxKind.AbstractKeyword)
                         };
            modelRoot.Types.Add(modelClass);
         }         
         
         // Base classes and interfaces
         if (classDecl.BaseList != null)
         {
            List<string> customInterfaces = new List<string>();

            foreach (BaseTypeSyntax type in classDecl.BaseList.Types)
            {
               string baseName = type.ToString();

               if (baseName == "INotifyPropertyChanged")
               {
                  modelClass.ImplementNotify = true;

                  continue;
               }

               // if we see the base class in the model, it's a superclass. Otherwise, it's a custom interface (maybe?)
               // TODO: Come up with something better than that
               ModelClass superClass = modelRoot.Types.OfType<ModelClass>().FirstOrDefault(c => c.Name == baseName);

               if (superClass != null)
                  modelClass.Superclass = superClass;
               else
                  customInterfaces.Add(baseName);
            }

            modelClass.CustomInterfaces = customInterfaces.Any()
                                             ? string.Join(",", customInterfaces)
                                             : null;
         }

         XMLDocumentation xmlDocumentation = ProcessXMLDocumentation(classDecl);
         modelClass.Summary = xmlDocumentation.Summary;
         modelClass.Description = xmlDocumentation.Description;
      }

      private class XMLDocumentation
      {
         public string Summary { get; set; }
         public string Description { get; set; }
      }

      private static XMLDocumentation ProcessXMLDocumentation(SyntaxNode classDecl)
      {
         string Extract(DocumentationCommentTriviaSyntax xmlComment, string tagName)
         {
            string extracted = null;
            XmlElementSyntax summary = xmlComment.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == tagName);

            if (summary != null)
            {
               extracted = string.Empty;

               for (int index = 0; index < summary.Content.Count; index++)
               {
                  XmlNodeSyntax xmlNodeSyntax = summary.Content[index];

                  extracted += (index == 0
                                ? Clean(xmlNodeSyntax)
                                : $"\n<p>{Clean(xmlNodeSyntax)}</p>");
               }
            }

            return extracted;
         }

         string Clean(XmlNodeSyntax xmlNodeSyntax) => xmlNodeSyntax.ToString().Replace("\r","").Replace("\n","").Replace("///","").Trim();

         XMLDocumentation result = new XMLDocumentation();
         List<DocumentationCommentTriviaSyntax> xmlTrivia = classDecl.GetLeadingTrivia()
                                                                     .Select(i => i.GetStructure())
                                                                     .OfType<DocumentationCommentTriviaSyntax>()
                                                                     .ToList();

         foreach (DocumentationCommentTriviaSyntax xmlComment in xmlTrivia)
         {
            result.Summary = Extract(xmlComment, "summary");
            result.Description = Extract(xmlComment, "remarks");
         }

         return result;
      }

   }

}
