// 

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sawczyn.EFDesigner.EFModel {
   internal class XMLDocumentation
   {
      public XMLDocumentation(SyntaxNode classDecl)
      {
         if (classDecl == null)
            throw new ArgumentNullException(nameof(classDecl));

         List<DocumentationCommentTriviaSyntax> xmlTrivia = classDecl.GetLeadingTrivia().Select(i => i.GetStructure())
                                                                     .OfType<DocumentationCommentTriviaSyntax>().ToList();

         foreach (DocumentationCommentTriviaSyntax xmlComment in xmlTrivia)
         {
            Summary = Extract(xmlComment, "summary");
            Description = Extract(xmlComment, "remarks");
         }
      }

      public string Summary { get; }
      public string Description { get; }

      private string Extract(DocumentationCommentTriviaSyntax xmlComment, string tagName)
      {
         string extracted = null;
         XmlElementSyntax summary = xmlComment.ChildNodes().OfType<XmlElementSyntax>()
                                              .FirstOrDefault(x => x.StartTag.Name.ToString() == tagName);

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

      private string Clean(XmlNodeSyntax xmlNodeSyntax) =>
         xmlNodeSyntax.ToString().Replace("\r", "").Replace("\n", "").Replace("///", "").Trim();
   }
}