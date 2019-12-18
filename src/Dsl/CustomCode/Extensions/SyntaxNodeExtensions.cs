using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel.Extensions
{
   public static class SyntaxNodeExtensions
   {
      public static bool HasAttribute([NotNull] this SyntaxNode node, string attributeName)
      {
         string fullname = attributeName.EndsWith("Attribute") ? attributeName : $"{attributeName}Attribute";
         string shortName = attributeName.EndsWith("Attribute") ? attributeName.Substring(0, attributeName.Length - 9) : attributeName;

         return node.DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .Any(x => x.Name.ToString() == shortName || x.Name.ToString() == fullname);
      }

      public static AttributeSyntax GetAttribute([NotNull] this SyntaxNode node, string attributeName)
      {
         string fullname = attributeName.EndsWith("Attribute") ? attributeName : $"{attributeName}Attribute";
         string shortName = attributeName.EndsWith("Attribute") ? attributeName.Substring(0, attributeName.Length - 9) : attributeName;

         return node.DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .FirstOrDefault(x => x.Name.ToString() == shortName || x.Name.ToString() == fullname);
      }

      public static IEnumerable<AttributeArgumentSyntax> GetAttributeArguments(this AttributeSyntax node)
      {
         return node.DescendantNodes().OfType<AttributeArgumentSyntax>();
      }

      public static string GetNamedArgumentValue(this AttributeSyntax node, string argumentName)
      {
         AttributeArgumentSyntax namedArgument =
            node.DescendantNodes()
                           .OfType<AttributeArgumentSyntax>()
                           .FirstOrDefault(aas => aas.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                     .Any(ins => ins.Identifier.Text == argumentName));
     
         SyntaxToken? valueToken = namedArgument.DescendantNodes().OfType<LiteralExpressionSyntax>().FirstOrDefault()?.Token;
         return valueToken?.Text.Trim('"');
      }
   }
}
