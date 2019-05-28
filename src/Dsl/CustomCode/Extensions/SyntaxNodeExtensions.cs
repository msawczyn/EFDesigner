using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel.Extensions
{
   public static class SyntaxNodeExtensions
   {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
      public static bool HasAttribute([NotNull] this SyntaxNode node, string attributeName)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
      {
         string name = attributeName.EndsWith("Attribute")
                          ? attributeName
                          : $"{attributeName}Attribute";

         return node.DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .Any(x => x.Name.ToString() == name);
      }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
#pragma warning disable CS3002 // Return type is not CLS-compliant
      public static AttributeSyntax GetAttribute(this SyntaxNode node, string attributeName)
#pragma warning restore CS3002 // Return type is not CLS-compliant
#pragma warning restore CS3001 // Argument type is not CLS-compliant
      {
         string name = attributeName.EndsWith("Attribute")
                          ? attributeName
                          : $"{attributeName}Attribute";

         return node.DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .FirstOrDefault(x => x.Name.ToString() == name);
      }

#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning disable CS3001 // Argument type is not CLS-compliant
      public static IEnumerable<AttributeArgumentSyntax> GetAttributeArguments(this AttributeSyntax node)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
#pragma warning restore CS3002 // Return type is not CLS-compliant
      {
         return node.DescendantNodes().OfType<AttributeArgumentSyntax>();
      }
   }
}
