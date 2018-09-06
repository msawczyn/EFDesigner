using System;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ClassShape : IHighlightFromModelExplorer
   {
      /// <summary>
      /// Exposes NodeShape Collapse() function to DSL's context menu
      /// </summary>
      public void CollapseShape()
      {
         SetIsExpandedValue(false);
      }

      /// <summary>
      /// Exposes NodeShape Expand() function to DSL's context menu
      /// </summary>
      public void ExpandShape()
      {
         SetIsExpandedValue(true);
      }

      protected override CompartmentMapping[] GetCompartmentMappings(Type melType)
      {
         CompartmentMapping[] mappings = base.GetCompartmentMappings(melType);

         foreach (ElementListCompartmentMapping mapping in mappings.OfType<ElementListCompartmentMapping>()
                                                                   .Where(m => m.CompartmentId == "AttributesCompartment"))
            mapping.ImageGetter = GetPropertyImage;

         return mappings;
      }

      private Image GetPropertyImage(ModelElement element)
      {
         Bitmap result = Resources.Spacer;

         if (element is ModelAttribute attribute)
         {
            if (attribute.IsIdentity)
               result = Resources.Identity;

            switch (attribute.Persistent)
            {
               case true:
                  switch (attribute.SetterVisibility)
                  {
                     case SetterAccessModifier.Public:
                        result = Resources.Public;
                        break;
                     case SetterAccessModifier.Protected:
                        result = Resources.Protected;
                        break;
                     case SetterAccessModifier.Internal:
                        result = Resources.Internal;
                        break;
                  }

                  break;
               case false:
                  switch (attribute.SetterVisibility)
                  {
                     case SetterAccessModifier.Public:
                        result = Resources.Calculated;
                        break;
                     case SetterAccessModifier.Protected:
                        result = Resources.CalculatedProtected;
                        break;
                     case SetterAccessModifier.Internal:
                        result = Resources.CalculatedInternal;
                        break;
                  }

                  break;
            }
         }

         // TODO: if element isn't valid, change glyph background color to WARNING yellow
         return result;
      }
   }
}
