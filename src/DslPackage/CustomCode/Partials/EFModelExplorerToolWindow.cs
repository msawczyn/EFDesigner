using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow
   {
      private class HighlightCache
      {
         private IHighlightFromModelExplorer CachedShape { get; }
         private Store CachedShapeStore => (CachedShape as ModelElement)?.Store;
         private bool? Visible => CachedShape?.Visible;

         private Color CachedColor { get; }
         private DashStyle CachedDashStyle { get; }
         private float CachedThickness { get; }

         public HighlightCache(IHighlightFromModelExplorer shape)
         {
            CachedShape = shape;

            if (shape != null)
            {
               CachedColor = shape.OutlineColor;
               CachedDashStyle = shape.OutlineDashStyle;
               CachedThickness = shape.OutlineThickness;
            }
         }

         internal void SetShapeColors()
         {
            if (Visible == true && CachedShapeStore?.TransactionManager != null && CachedShape != null)
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
            if (Visible == true && CachedShapeStore?.TransactionManager != null && CachedShape != null)
            {
               using (Transaction tx = CachedShapeStore.TransactionManager.BeginTransaction("Reset model explorer highlight"))
               {
                  CachedShape.OutlineColor = CachedColor;
                  CachedShape.OutlineDashStyle = CachedDashStyle;
                  CachedShape.OutlineThickness = CachedThickness;
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

            try
            {
               highlightCache = new HighlightCache(selectedShape);
               highlightCache.SetShapeColors();
            }
            catch
            {
               highlightCache = null;
            }
         }
      }

      public static void ClearHighlight()
      {
         try
         {
            highlightCache?.ResetShapeColors();
         }
         catch
         {
            // ignore
         }
         highlightCache = null;
      }
   }
}
