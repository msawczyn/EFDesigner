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
         if (element is ModelAttribute)
         {
            ModelAttribute attribute = (ModelAttribute)element;

            if (attribute.IsIdentity)
               return Resources.Identity;

            switch (attribute.Persistent)
            {
               case true:
                  switch (attribute.SetterVisibility)
                  {
                     case SetterAccessModifier.Public:
                        return Resources.Public;
                     case SetterAccessModifier.Protected:
                        return Resources.Protected;
                     case SetterAccessModifier.Internal:
                        return Resources.Internal;
                  }

                  break;
               case false:
                  switch (attribute.SetterVisibility)
                  {
                     case SetterAccessModifier.Public:
                        return Resources.Calculated;
                     case SetterAccessModifier.Protected:
                        return Resources.CalculatedProtected;
                     case SetterAccessModifier.Internal:
                        return Resources.CalculatedInternal;
                  }

                  break;
            }

            return Resources.Spacer;
         }

         return null;
      }
   }
}
