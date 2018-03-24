using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
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
         ModelRoot modelRoot = store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         if (current.IsSerializing)
            return;

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
            case "Persistent":
               UpdateDisplayForPersistence(element);
               break;

            case "TargetPropertyName":
               errorMessage = ValidateAssociationIdentifier(element, element.Source, element.Target, (string)e.NewValue);
               break;

            case "SourcePropertyName":
               errorMessage = ValidateAssociationIdentifier(element, element.Target, element.Source, (string)e.NewValue);
               break;

            case "SourceMultiplicity":
               Multiplicity newSourceMultiplicity = (Multiplicity) e.NewValue;

               //TODO: EFCore limitation as of 2.0. Review for each new release.
               if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore && 
                   newSourceMultiplicity == Multiplicity.ZeroMany && 
                   element.TargetMultiplicity == Multiplicity.ZeroMany)
               {
                  errorMessage = "Many-to-many relationships not yet supported for EntityFramework Core";
               }
               else
               {
                  if ((newSourceMultiplicity == Multiplicity.One && element.TargetMultiplicity == Multiplicity.One) || 
                      (newSourceMultiplicity == Multiplicity.ZeroOne && element.TargetMultiplicity == Multiplicity.ZeroOne))
                  {
                     element.SourceRole = EndpointRole.NotSet;
                     element.TargetRole = EndpointRole.NotSet;
                  }
                  else
                     SetEndpointRoles(element);

                  UpdateDisplayForCascadeDelete(element, null, null, newSourceMultiplicity);
               }
               break;

            case "TargetMultiplicity":
               Multiplicity newTargetMultiplicity = (Multiplicity)e.NewValue;

               //TODO: EFCore limitation as of 2.0. Review for each new release.
               if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore && 
                   newTargetMultiplicity == Multiplicity.ZeroMany && 
                   element.SourceMultiplicity == Multiplicity.ZeroMany)
               {
                  errorMessage = "Many-to-many relationships not yet supported for EntityFramework Core";
               }
               else
               {
                  if ((element.SourceMultiplicity == Multiplicity.One && newTargetMultiplicity == Multiplicity.One) || 
                      (element.SourceMultiplicity == Multiplicity.ZeroOne && newTargetMultiplicity == Multiplicity.ZeroOne))
                  {
                     element.SourceRole = EndpointRole.NotSet;
                     element.TargetRole = EndpointRole.NotSet;
                  }
                  else
                     SetEndpointRoles(element);

                  UpdateDisplayForCascadeDelete(element, null, null, null, newTargetMultiplicity);
               }
               break;

            case "SourceRole":
               EndpointRole newSourceRole = (EndpointRole)e.NewValue;
               if (element.TargetRole == EndpointRole.NotSet && newSourceRole == EndpointRole.Dependent)
                  element.TargetRole = EndpointRole.Principal;
               else if (element.TargetRole == EndpointRole.NotSet && newSourceRole == EndpointRole.Principal)
                  element.TargetRole = EndpointRole.Dependent;

               break;

            case "TargetRole":
               EndpointRole newTargetRole = (EndpointRole) e.NewValue;
               if (element.SourceRole == EndpointRole.NotSet && newTargetRole == EndpointRole.Dependent)
                  element.SourceRole = EndpointRole.Principal;
               else if (element.SourceRole == EndpointRole.NotSet && newTargetRole == EndpointRole.Principal)
                  element.SourceRole = EndpointRole.Dependent;
               
               break;

            case "SourceDeleteAction":
               DeleteAction sourceDeleteAction = (DeleteAction) e.NewValue;
               UpdateDisplayForCascadeDelete(element, sourceDeleteAction);

               break;

            case "TargetDeleteAction":
               DeleteAction targetDeleteAction = (DeleteAction)e.NewValue;
               UpdateDisplayForCascadeDelete(element, null, targetDeleteAction);
               break;
         }

         if (errorMessage != null)
         {
            current.Rollback();
            MessageBox.Show(errorMessage);
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

      private static string ValidateAssociationIdentifier(Association association, ModelClass targetedClass, ModelClass enclosingClass, string identifier)
      {
         if (string.IsNullOrWhiteSpace(identifier) || !CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
            return "Name must be a valid .NET identifier";

         ModelClass offendingModelClass = targetedClass.AllAttributes.FirstOrDefault(x => x.Name == identifier)?.ModelClass
                                          ?? targetedClass.AllNavigationProperties(association).FirstOrDefault(x => x.PropertyName == identifier)?.ClassType;

         return offendingModelClass != null ? $"Duplicate symbol {identifier} in {offendingModelClass.Name}" : null;
      }
   }
}