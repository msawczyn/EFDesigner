using System.ComponentModel.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelClipboardCommandSet
   {
      protected override void ProcessOnStatusCutCommand(MenuCommand command)
      {
         OnStatusDeleteCommandLogic.ForEditor(command, CurrentDocumentSelection, base.ProcessOnStatusCutCommand);
      }

      protected override void ProcessOnStatusCopyCommand(MenuCommand command)
      {
         OnStatusCopyCommandLogic.ForEditor(command, CurrentDocumentSelection, base.ProcessOnStatusCopyCommand);
      }

      protected override void ProcessOnStatusPasteCommand(MenuCommand command)
      {
         OnStatusPasteCommandLogic.ForEditor(command, CurrentDocumentSelection, base.ProcessOnStatusPasteCommand);
      }
   }
}