using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow
   {
      internal class HighlightCache
      {
         internal Store CachedShapeStore { get; }
         internal IHighlightFromModelExplorer CachedShape { get; private set; }
         internal Color CachedColor { get; }
         internal DashStyle CachedDashStyle { get; }
         internal float CachedThickness { get; }

         public HighlightCache(ModelElement modelElement, IHighlightFromModelExplorer shape)
         {
            CachedShapeStore = modelElement.Store;
            CachedShape = shape;
            CachedColor = shape.OutlineColor;
            CachedDashStyle = shape.OutlineDashStyle;
            CachedThickness = shape.OutlineThickness;
         }

         internal void SetShapeColors()
         {
            using (Transaction tx = CachedShapeStore.TransactionManager.BeginTransaction("Set model explorer highlight"))
            {
               CachedShape.OutlineColor = Color.DarkOrange;
               CachedShape.OutlineDashStyle = DashStyle.Solid;
               CachedShape.OutlineThickness = 0.09f;
               tx.Commit();
            }
         }

         internal void ResetShapeColors()
         {
            using (Transaction tx = CachedShapeStore.TransactionManager.BeginTransaction("Reset model explorer highlight"))
            {
               CachedShape.OutlineColor = CachedColor;
               CachedShape.OutlineDashStyle = CachedDashStyle;
               CachedShape.OutlineThickness = CachedThickness;
               CachedShape = null;
               tx.Commit();
            }
         }
      }

      private static HighlightCache highlightCache;

      protected override void OnSelectionChanged(System.EventArgs e)
      {
         base.OnSelectionChanged(e);

         // restore cached settings
         highlightCache?.ResetShapeColors();

         if (PrimarySelection is ModelElement modelElement && 
             PresentationViewsSubject.GetPresentation(modelElement).FirstOrDefault() is IHighlightFromModelExplorer selectedShape)
         {
            highlightCache = new HighlightCache(modelElement, selectedShape);
            highlightCache.SetShapeColors();
         }
      }

      /// <summary>
      /// Creates the model explorer to be hosted in the window.
      /// </summary>
      /// <returns>ModelExplorerTreeContainer</returns>
      protected override ModelExplorerTreeContainer CreateTreeContainer()
      {
         ModelExplorerTreeContainer result = base.CreateTreeContainer();
         return result;
      }

      public static void ClearHighlight()
      {
         highlightCache?.ResetShapeColors();
         highlightCache = null;
      }
   }
}
