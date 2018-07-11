using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Association), FireTime = TimeToFire.TopLevelCommit)]
   public class AssociationChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         Association element = (Association)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();

         switch (e.DomainProperty.Name)
         {
            case "Persistent":
               UpdateDisplayForPersistence(element);
               break;

            case "SourceDeleteAction":
               DeleteAction sourceDeleteAction = (DeleteAction)e.NewValue;
               UpdateDisplayForCascadeDelete(element, sourceDeleteAction);

               break;

            case "SourceMultiplicity":
               Multiplicity newSourceMultiplicity = (Multiplicity)e.NewValue;

               if (element.Target.IsDependentType && !element.Source.IsDependentType && newSourceMultiplicity == Multiplicity.ZeroMany)
               {
                  errorMessages.Add($"Can't have a 0..* association from {element.Source.Name} to dependent type {element.Target.Name}");

                  break;
               }

               if ((newSourceMultiplicity == Multiplicity.One && element.TargetMultiplicity == Multiplicity.One) ||
                   (newSourceMultiplicity == Multiplicity.ZeroOne && element.TargetMultiplicity == Multiplicity.ZeroOne))
               {
                  element.SourceRole = EndpointRole.NotSet;
                  element.TargetRole = EndpointRole.NotSet;
               }
               else
                  SetEndpointRoles(element);

               UpdateDisplayForCascadeDelete(element, null, null, newSourceMultiplicity);
               break;

            case "SourcePropertyName":
               string sourcePropertyNameErrorMessage = ValidateAssociationIdentifier(element, element.Target, (string)e.NewValue);
               if (EFModelDiagram.IsDropping && sourcePropertyNameErrorMessage != null)
                  element.Delete();
               else
                  errorMessages.Add(sourcePropertyNameErrorMessage);
               break;

            case "SourceRole":
               EndpointRole newSourceRole = (EndpointRole)e.NewValue;
               if (element.TargetRole == EndpointRole.NotSet && newSourceRole == EndpointRole.Dependent)
                  element.TargetRole = EndpointRole.Principal;
               else if (element.TargetRole == EndpointRole.NotSet && newSourceRole == EndpointRole.Principal)
                  element.TargetRole = EndpointRole.Dependent;

               break;

            case "TargetDeleteAction":
               DeleteAction targetDeleteAction = (DeleteAction)e.NewValue;
               UpdateDisplayForCascadeDelete(element, null, targetDeleteAction);
               break;

            case "TargetMultiplicity":
               Multiplicity newTargetMultiplicity = (Multiplicity)e.NewValue;

               if (element.Source.IsDependentType && !element.Target.IsDependentType && newTargetMultiplicity == Multiplicity.ZeroMany)
               {
                  errorMessages.Add($"Can't have a 0..* association from {element.Target.Name} to dependent type {element.Source.Name}");

                  break;
               }

               if ((element.SourceMultiplicity == Multiplicity.One && newTargetMultiplicity == Multiplicity.One) ||
                   (element.SourceMultiplicity == Multiplicity.ZeroOne && newTargetMultiplicity == Multiplicity.ZeroOne))
               {
                  element.SourceRole = EndpointRole.NotSet;
                  element.TargetRole = EndpointRole.NotSet;
               }
               else
                  SetEndpointRoles(element);

               UpdateDisplayForCascadeDelete(element, null, null, null, newTargetMultiplicity);
               break;

            case "TargetPropertyName":
               // if we're creating an association via drag/drop, it's possible the existing property name
               // is the same as the default property name. The default doesn't get created until the transaction is 
               // committed, so the drop's action will cause a name clash. Remove the clashing property, but
               // only if drag/drop.

               string targetPropertyNameErrorMessage = ValidateAssociationIdentifier(element, element.Source, (string)e.NewValue);
               if (EFModelDiagram.IsDropping && targetPropertyNameErrorMessage != null)
                  element.Delete();
               else
                  errorMessages.Add(targetPropertyNameErrorMessage);
               break;

            case "TargetRole":
               EndpointRole newTargetRole = (EndpointRole)e.NewValue;
               if (element.SourceRole == EndpointRole.NotSet && newTargetRole == EndpointRole.Dependent)
                  element.SourceRole = EndpointRole.Principal;
               else if (element.SourceRole == EndpointRole.NotSet && newTargetRole == EndpointRole.Principal)
                  element.SourceRole = EndpointRole.Dependent;

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();
         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(string.Join("; ", errorMessages));
         }
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
            connector.Color = persistent ? Color.Black : Color.DarkGray;

         foreach (AssociationConnector connector in changeStyle)
            connector.DashStyle = persistent ? DashStyle.Solid : DashStyle.Dash;
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
            connector.Color = cascade ? Color.Red : Color.Black;

         foreach (AssociationConnector connector in changeStyle)
            connector.DashStyle = cascade ? DashStyle.Dash : DashStyle.Solid;
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

      private static string ValidateAssociationIdentifier(Association association, ModelClass targetedClass, string identifier)
      {
         if (string.IsNullOrWhiteSpace(identifier) || !CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
            return $"{identifier} isn't a valid .NET identifier";

         ModelClass offendingModelClass = targetedClass.AllAttributes.FirstOrDefault(x => x.Name == identifier)?.ModelClass
                                          ?? targetedClass.AllNavigationProperties(association).FirstOrDefault(x => x.PropertyName == identifier)?.ClassType;

         return offendingModelClass != null ? $"Duplicate symbol {identifier} in {offendingModelClass.Name}" : null;
      }
   }
}