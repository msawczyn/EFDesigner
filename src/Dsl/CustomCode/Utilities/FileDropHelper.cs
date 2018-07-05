using System.CodeDom.Compiler;
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
               // find namespace-less classes
               foreach (ClassDeclarationSyntax cls in root.Members.OfType<ClassDeclarationSyntax>())
                  ProcessClass(store, cls);

               // same with enums
               foreach (EnumDeclarationSyntax en in root.Members.OfType<EnumDeclarationSyntax>())
                  ProcessEnum(store, en);


               // find classes and enums in namespaces
               foreach (NamespaceDeclarationSyntax ns in root.Members.OfType<NamespaceDeclarationSyntax>())
               {
                  foreach (ClassDeclarationSyntax cls in ns.Members.OfType<ClassDeclarationSyntax>())
                     ProcessClass(store, cls, ns);
                  foreach (EnumDeclarationSyntax en in ns.Members.OfType<EnumDeclarationSyntax>())
                     ProcessEnum(store, en, ns);
               }
            }
         }
         catch
         {
            ErrorDisplay.Show("No class or enum found in " + filename);
         }
      }

      private static void ProcessEnum(Store store, EnumDeclarationSyntax en, NamespaceDeclarationSyntax ns = null)
      {
      }

      private static void ProcessClass(Store store, ClassDeclarationSyntax cls, NamespaceDeclarationSyntax ns = null)
      {
         ModelRoot modelRoot = store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();

         {
            string className = cls.Identifier.Text;

            //// can't add duplicate class names
            //if (store.ElementDirectory.AllElements.OfType<ModelClass>().Any(c => c.Name == className))
            //{
            //   ErrorDisplay.Show($"'{className}' already exists in model.");
            //   return;
            //}

            if (cls.TypeParameterList != null)
            {
               ErrorDisplay.Show($"Can't add generic class '{className}'.");
               return;
            }

            using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped class"))
            {
               ModelClass newClass = new ModelClass(store, new PropertyAssignment(ModelClass.NameDomainPropertyId, className));
               modelRoot.Types.Add(newClass);

               if (ns != null)
               {
                  string @namespace = ns.Members.OfType<QualifiedNameSyntax>().FirstOrDefault()?.ToString();

                  if (!string.IsNullOrEmpty(@namespace))
                     newClass.Namespace = @namespace;
               }

               if (cls.DescendantNodes().Any(n => n.Kind() == SyntaxKind.AbstractKeyword))
                  newClass.IsAbstract = true;

               BaseTypeSyntax baseType = cls.BaseList?.Types.FirstOrDefault();

               if (baseType != null)
               {
                  string baseName = baseType.ToString();
                  newClass.Superclass = modelRoot.Types.OfType<ModelClass>().FirstOrDefault(c => c.Name == baseName);
               }

               // TODO: Continue here - separate base class from interfaces?

               newClass.Attributes.Clear();
               foreach (string source in cls.Members
                                            .OfType<PropertyDeclarationSyntax>()
                                            .Select(prop => prop.ToString())
                                            .Select(source => Regex.Replace(source, @"\[[^]]+\]", "", RegexOptions.Multiline)
                                                                   .Replace("\r", "")
                                                                   .Replace("\n", "")))
               {
                  try
                  {
                     ParseResult parseResult = ModelAttribute.Parse(modelRoot, source);

                     if (parseResult == null)
                     {
                        WarningDisplay.Show($"Could not parse '{source}'. The line will be discarded.");

                        continue;
                     }

                     string message = null;

                     if (string.IsNullOrEmpty(parseResult.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(parseResult.Name))
                        message = $"Could not add '{parseResult.Name}' to {className}: '{parseResult.Name}' is not a valid .NET identifier";
                     else if (newClass.AllAttributes.Any(x => x.Name == parseResult.Name))
                        message = $"Could not add {parseResult.Name} to {className}: {parseResult.Name} already in use";
                     else if (newClass.AllNavigationProperties().Any(p => p.PropertyName == parseResult.Name))
                        message = $"Could not add {parseResult.Name} to {className}: {parseResult.Name} already in use";

                     if (message != null)
                     {
                        WarningDisplay.Show(message);
                        continue;
                     }

                     ModelAttribute modelAttribute = new ModelAttribute(store, new PropertyAssignment(ModelAttribute.NameDomainPropertyId, parseResult.Name), new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, parseResult.Type ?? "String"), new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, parseResult.Required ?? true), new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, parseResult.MaxLength ?? 0), new PropertyAssignment(ModelAttribute.InitialValueDomainPropertyId, parseResult.InitialValue), new PropertyAssignment(ModelAttribute.IsIdentityDomainPropertyId, parseResult.IsIdentity), new PropertyAssignment(ModelAttribute.SetterVisibilityDomainPropertyId, parseResult.SetterVisibility ?? SetterAccessModifier.Public));
                     newClass.Attributes.Add(modelAttribute);
                  }
                  catch
                  {
                     WarningDisplay.Show($"Could not parse '{source}'. The line will be discarded.");
                  }
               }
            
               tx.Commit();
            }
         }
      }
   }
}
