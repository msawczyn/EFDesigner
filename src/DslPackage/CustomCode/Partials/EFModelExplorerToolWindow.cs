using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow
   {
      internal class HighlightCache
      {
         internal IHighlightFromModelExplorer CachedShape { get; private set; }
         internal Store CachedShapeStore => (CachedShape as ModelElement)?.Store;
         internal bool? Visible => CachedShape?.Visible;

         internal Color CachedColor { get; }
         internal DashStyle CachedDashStyle { get; }
         internal float CachedThickness { get; }

         public HighlightCache(IHighlightFromModelExplorer shape)
         {
            CachedShape = shape;
            CachedColor = shape.OutlineColor;
            CachedDashStyle = shape.OutlineDashStyle;
            CachedThickness = shape.OutlineThickness;
         }

         internal void SetShapeColors()
         {
            if (Visible == true)
            {
               using (Transaction tx = CachedShapeStore.TransactionManager.BeginTransaction("Set model explorer highlight"))
               {
                  CachedShape.OutlineColor = Color.DarkOrange;
                  CachedShape.OutlineDashStyle = DashStyle.Solid;
                  CachedShape.OutlineThickness = 0.09f;
                  tx.Commit();
               }
            }
         }

         internal void ResetShapeColors()
         {
            if (Visible == true)
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
      }

      private static HighlightCache highlightCache;

      protected override void OnSelectionChanged(System.EventArgs e)
      {
         base.OnSelectionChanged(e);

         // restore cached settings
         highlightCache?.ResetShapeColors();
         highlightCache = null;

         if (PrimarySelection is ModelElement modelElement && 
             PresentationViewsSubject.GetPresentation(modelElement).FirstOrDefault() is IHighlightFromModelExplorer selectedShape)
         {
            highlightCache = new HighlightCache(selectedShape);
            highlightCache.SetShapeColors();
         }
      }

      public static void ClearHighlight()
      {
         highlightCache?.ResetShapeColors();
         highlightCache = null;
      }
   }
}
