using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(BidirectionalAssociation), FireTime = TimeToFire.TopLevelCommit)]
   public class BidirectionalAssociationChangeRules : UnidirectionalAssociationChangeRules
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         BidirectionalAssociation element = (BidirectionalAssociation)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         switch (e.DomainProperty.Name)
         {
            case "SourceCustomAttributes":

               if (!string.IsNullOrWhiteSpace(element.SourceCustomAttributes))
                  element.SourceCustomAttributes = $"[{element.SourceCustomAttributes.Trim('[', ']')}]";

               break;
         }
      }

      internal static void SetEndpointRoles(Association element)
      {
         switch (element.TargetMultiplicity)
         {
            case Multiplicity.ZeroMany:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.NotApplicable;
                     element.TargetRole = EndpointRole.NotApplicable;

                     break;
                  case Multiplicity.One:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     break;
                  case Multiplicity.ZeroOne:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     break;
               }

               break;
            case Multiplicity.One:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     break;
                  case Multiplicity.One:

                     break;
                  case Multiplicity.ZeroOne:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     break;
               }

               break;
            case Multiplicity.ZeroOne:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     break;
                  case Multiplicity.One:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     break;
                  case Multiplicity.ZeroOne:

                     break;
               }

               break;
         }
      }

      internal static void UpdateDisplayForCascadeDelete(Association element,
                                                         DeleteAction? sourceDeleteAction = null,
                                                         DeleteAction? targetDeleteAction = null,
                                                         Multiplicity? sourceMultiplicity = null,
                                                         Multiplicity? targetMultiplicity = null)
      {
         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         sourceDeleteAction = sourceDeleteAction ?? element.SourceDeleteAction;
         targetDeleteAction = targetDeleteAction ?? element.TargetDeleteAction;
         sourceMultiplicity = sourceMultiplicity ?? element.SourceMultiplicity;
         targetMultiplicity = targetMultiplicity ?? element.TargetMultiplicity;

         bool cascade = modelRoot.ShowCascadeDeletes &&
                        element.Persistent &&
                        (sourceMultiplicity == Multiplicity.One ||
                         targetMultiplicity == Multiplicity.One ||
                         targetDeleteAction == DeleteAction.Cascade ||
                         sourceDeleteAction == DeleteAction.Cascade);

         List<AssociationConnector> changeColor =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.Color != Color.Red) ||
                                                        (!cascade && connector.Color != Color.Black))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.DashStyle != DashStyle.Dash) ||
                                                        (!cascade && connector.DashStyle != DashStyle.Solid))
                                    .ToList();

         foreach (AssociationConnector connector in changeColor)
            connector.Color = cascade
                                 ? Color.Red
                                 : Color.Black;

         foreach (AssociationConnector connector in changeStyle)
            connector.DashStyle = cascade
                                     ? DashStyle.Dash
                                     : DashStyle.Solid;
      }

      internal static void UpdateDisplayForPersistence(Association element)
      {
         // don't change unless necessary so as to not set the model's dirty flag without need
         bool persistent = element.Persistent;

         List<AssociationConnector> changeColors =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && connector.Color != Color.Black) ||
                                                        (!persistent && connector.Color != Color.DarkGray))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && connector.DashStyle != DashStyle.Solid) ||
                                                        (!persistent && connector.DashStyle != DashStyle.Dash))
                                    .ToList();

         foreach (AssociationConnector connector in changeColors)
            connector.Color = persistent
                                 ? Color.Black
                                 : Color.DarkGray;

         foreach (AssociationConnector connector in changeStyle)
            connector.DashStyle = persistent
                                     ? DashStyle.Solid
                                     : DashStyle.Dash;
      }

      private static string ValidateAssociationIdentifier(Association association, ModelClass targetedClass, string identifier)
      {
         if (string.IsNullOrWhiteSpace(identifier) || !CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
            return $"{identifier} isn't a valid .NET identifier";

         ModelClass offendingModelClass = targetedClass.AllAttributes.FirstOrDefault(x => x.Name == identifier)?.ModelClass ?? targetedClass.AllNavigationProperties(association).FirstOrDefault(x => x.PropertyName == identifier)?.ClassType;

         return offendingModelClass != null
                   ? $"Duplicate symbol {identifier} in {offendingModelClass.Name}"
                   : null;
      }
   }
}
