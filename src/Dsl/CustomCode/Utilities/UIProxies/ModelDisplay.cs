using Sawczyn.EFDesigner.EFModel;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls the various methods to register handlers at the UI level.
   /// </summary>
   public static class ModelDisplay
   {
      // executes autolayout on the diagram passed in
      public delegate void LayoutDiagramAction(EFModelDiagram diagram);

      private static LayoutDiagramAction LayoutDiagramMethod;

      public static void LayoutDiagram(EFModelDiagram diagram)
      {
         if (LayoutDiagramMethod != null)
         {
            try
            {
               LayoutDiagramMethod(diagram);
            }
            catch
            {
               // swallow the exception
            }
         }
      }

      public static void RegisterLayoutDiagramAction(LayoutDiagramAction method)
      {
         LayoutDiagramMethod = method;
      }

   }
}