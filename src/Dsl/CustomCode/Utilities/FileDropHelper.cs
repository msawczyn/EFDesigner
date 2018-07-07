using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{

   internal class FileDropHelper
   {
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
               List<ClassDeclarationSyntax> classDeclarations = new List<ClassDeclarationSyntax>();

               using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped class"))
               {
                  // find namespace-less classes
                  foreach (ClassDeclarationSyntax classDecl in root.Members.OfType<ClassDeclarationSyntax>())
                  {
                     if (classDecl.BaseList != null && classDecl.BaseList.Types.FirstOrDefault()?.ToString() == "DbContext")
                        ProcessContext(store, classDecl);
                     else
                     {
                        classDeclarations.Add(classDecl);
                        ProcessClass(store, classDecl);
                     }
                  }

                  // same with enums
                  foreach (EnumDeclarationSyntax enumDecl in root.Members.OfType<EnumDeclarationSyntax>())
                     ProcessEnum(store, enumDecl);

                  // find classes and enums in namespaces
                  foreach (NamespaceDeclarationSyntax namespaceDecl in root.Members.OfType<NamespaceDeclarationSyntax>())
                  {
                     foreach (ClassDeclarationSyntax classDecl in root.Members.OfType<ClassDeclarationSyntax>())
                     {

                        if (classDecl.BaseList != null && classDecl.BaseList.Types.FirstOrDefault()?.ToString() == "DbContext")
                           ProcessContext(store, classDecl);
                        else
                        {
                           classDeclarations.Add(classDecl);
                           ProcessClass(store, classDecl, namespaceDecl);
                        }
                     }

                     foreach (EnumDeclarationSyntax enumDecl in root.Members.OfType<EnumDeclarationSyntax>())
                        ProcessEnum(store, enumDecl, namespaceDecl);
                  }

                  // process last so we know what all the classes and enums are first
                  foreach (ClassDeclarationSyntax classDecl in classDeclarations)
                     ProcessProperties(store, classDecl);

                  tx.Commit();
               }
            }
         }
         catch
         {
            ErrorDisplay.Show("No class or enum found in " + filename);
         }
      }

      private static void ProcessProperties(Store store, ClassDeclarationSyntax classDecl)
      {
         ModelRoot modelRoot = store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();

         string className = classDecl.Identifier.Text;
         ModelClass modelClass = store.ElementDirectory.AllElements.OfType<ModelClass>().FirstOrDefault(c => c.Name == className);
         modelClass.Attributes.Clear();

         foreach (PropertyDeclarationSyntax propertyDecl in classDecl.Members.OfType<PropertyDeclarationSyntax>())
         {
            string source = Regex.Replace(propertyDecl.ToString(), @"\[[^]]+\]", "", RegexOptions.Multiline).Replace("\r", "").Replace("\n", "");

            try
            {
               #region Parse
               ParseResult parseResult = ModelAttribute.Parse(modelRoot, source);

               if (parseResult == null)
               {
                  WarningDisplay.Show($"Could not parse '{source}'. The line will be discarded.");

                  continue;
               }

               string message = null;

               if (string.IsNullOrEmpty(parseResult.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(parseResult.Name))
                  message = $"Could not add '{parseResult.Name}' to {className}: '{parseResult.Name}' is not a valid .NET identifier";
               else if (modelClass.AllAttributes.Any(x => x.Name == parseResult.Name))
                  message = $"Could not add {parseResult.Name} to {className}: {parseResult.Name} already in use";
               else if (modelClass.AllNavigationProperties().Any(p => p.PropertyName == parseResult.Name))
                  message = $"Could not add {parseResult.Name} to {className}: {parseResult.Name} already in use";

               if (message != null)
               {
                  WarningDisplay.Show(message);

                  continue;
               }
               #endregion Parse


               ModelAttribute modelAttribute = new ModelAttribute(store, new PropertyAssignment(ModelAttribute.NameDomainPropertyId, parseResult.Name), new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, parseResult.Type ?? "String"), new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, parseResult.Required ?? true), new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, parseResult.MaxLength ?? 0), new PropertyAssignment(ModelAttribute.InitialValueDomainPropertyId, parseResult.InitialValue), new PropertyAssignment(ModelAttribute.IsIdentityDomainPropertyId, parseResult.IsIdentity), new PropertyAssignment(ModelAttribute.SetterVisibilityDomainPropertyId, parseResult.SetterVisibility ?? SetterAccessModifier.Public));
               modelClass.Attributes.Add(modelAttribute);
            }
            catch
            {
               WarningDisplay.Show($"Could not parse '{source}'. The line will be discarded.");
            }
         }

      }

      private static void ProcessContext(Store store, ClassDeclarationSyntax ctx, NamespaceDeclarationSyntax namespaceDecl = null)
      {

      }

      private static void ProcessEnum(Store store, EnumDeclarationSyntax enumDecl, NamespaceDeclarationSyntax namespaceDecl = null)
      {
      }

      private static void ProcessClass(Store store, ClassDeclarationSyntax classDecl, NamespaceDeclarationSyntax namespaceDecl = null)
      {
         ModelRoot modelRoot = store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();
         string className = classDecl.Identifier.Text;

         #region Sanity checks

         // can't add duplicate class names
         if (store.ElementDirectory.AllElements.OfType<ModelClass>().Any(c => c.Name == className))
         {
            ErrorDisplay.Show($"'{className}' already exists in model.");

            return;
         }

         if (classDecl.TypeParameterList != null)
         {
            ErrorDisplay.Show($"Can't add generic class '{className}'.");

            return;
         }

         #endregion

         ModelClass modelClass = new ModelClass(store, new PropertyAssignment(ModelClass.NameDomainPropertyId, className))
         {
            Namespace = namespaceDecl.Name?.ToString()
                                  ,
            IsAbstract = classDecl.DescendantNodes().Any(n => n.Kind() == SyntaxKind.AbstractKeyword)
         };

         #region Base classes and interfaces

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

               // if we see the base class in the model, it's a superclass. Otherwise, it's a custom interface
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

         #endregion Base classes and interfaces

         #region Comments

         List<DocumentationCommentTriviaSyntax> xmlTrivia = classDecl.GetLeadingTrivia().Select(i => i.GetStructure()).OfType<DocumentationCommentTriviaSyntax>().ToList();

         foreach (DocumentationCommentTriviaSyntax xmlComment in xmlTrivia)
         {
            XmlElementSyntax summary = xmlComment.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == "summary");

            if (summary != null)
            {
               modelClass.Summary = string.Empty;

               for (int index = 0; index < summary.Content.Count; index++)
               {
                  XmlNodeSyntax xmlNodeSyntax = summary.Content[index];

                  modelClass.Summary += (index == 0
                                          ? xmlNodeSyntax.ToString()
                                          : $"\n<p>{xmlNodeSyntax.ToString()}</p>");
               }
            }

            XmlElementSyntax remarks = xmlComment.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(x => x.StartTag.Name.ToString() == "remarks");

            if (remarks != null)
            {
               modelClass.Description = string.Empty;

               for (int index = 0; index < remarks.Content.Count; index++)
               {
                  XmlNodeSyntax xmlNodeSyntax = remarks.Content[index];

                  modelClass.Description += (index == 0
                                              ? xmlNodeSyntax.ToString()
                                              : $"\n<p>{xmlNodeSyntax.ToString()}</p>");
               }
            }
         }

         #endregion Comments

         modelRoot.Types.Add(modelClass);
      }
   }

}
