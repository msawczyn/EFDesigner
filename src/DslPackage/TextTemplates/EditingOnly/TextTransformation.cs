using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TextTemplating;

namespace Sawczyn.EFDesigner.EFModel.DslPackage.TextTemplates.EditingOnly
{
   [SuppressMessage("ReSharper", "UnusedMember.Local")]
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   partial class EditOnly
   {
      private ITextTemplatingEngineHost Host { get; set; }

      private string PopIndent() { return string.Empty; }

      private void PushIndent(string indent) { }

      private void WriteLine(string textToAppend) { }

      private void WriteLine(string format, params object[] args) { }
   }
}