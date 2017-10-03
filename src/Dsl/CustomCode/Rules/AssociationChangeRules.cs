using System.CodeDom.Compiler;
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
               errorMessage = ValidateAssociationIdentifier(element, element.Source, (string)e.NewValue);
               break;

            case "SourcePropertyName":
               errorMessage = ValidateAssociationIdentifier(element, element.Target, (string)e.NewValue);
               break;

            case "SourceMultiplicity":
               Multiplicity newSourceMultiplicity = (Multiplicity) e.NewValue;
               if (newSourceMultiplicity == Multiplicity.One && element.TargetMultiplicity == Multiplicity.One ||
                   newSourceMultiplicity == Multiplicity.ZeroOne && element.TargetMultiplicity == Multiplicity.ZeroOne)
               {
                  element.SourceRole = EndpointRole.NotSet;
                  element.TargetRole = EndpointRole.NotSet;
               }
               else
                  SetEndpointRoles(element);

               UpdateDisplayForCascadeDelete(element);
               break;

            case "TargetMultiplicity":
               Multiplicity newTargetMultiplicity = (Multiplicity)e.NewValue;
               if (element.SourceMultiplicity == Multiplicity.One && newTargetMultiplicity == Multiplicity.One ||
                   element.SourceMultiplicity == Multiplicity.ZeroOne && newTargetMultiplicity == Multiplicity.ZeroOne)
               {
                  element.SourceRole = EndpointRole.NotSet;
                  element.TargetRole = EndpointRole.NotSet;
               }
               else
                  SetEndpointRoles(element);

               UpdateDisplayForCascadeDelete(element);
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
               UpdateDisplayForCascadeDelete(element);

               break;

            case "TargetDeleteAction":
               DeleteAction targetDeleteAction = (DeleteAction)e.NewValue;
               UpdateDisplayForCascadeDelete(element);
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
         foreach (AssociationConnector connector in PresentationViewsSubject.GetPresentation(element).OfType<AssociationConnector>())
         {
            connector.Color = element.Persistent ? Color.Black : Color.DarkGray;
            connector.DashStyle = element.Persistent ? DashStyle.Solid : DashStyle.Dash;
         }
      }

      internal static void UpdateDisplayForCascadeDelete(Association element)
      {
         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         bool cascade = modelRoot.ShowCascadeDeletes && element.Persistent &&
                        (element.SourceMultiplicity == Multiplicity.One || element.TargetMultiplicity == Multiplicity.One ||
                        element.TargetDeleteAction == DeleteAction.Cascade || element.SourceDeleteAction == DeleteAction.Cascade);

         foreach (AssociationConnector connector in PresentationViewsSubject.GetPresentation(element).OfType<AssociationConnector>())
         {
            connector.Color = cascade ? Color.Red : Color.Black;
            connector.DashStyle = cascade ? DashStyle.Dash : DashStyle.Solid;
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

      private static string ValidateAssociationIdentifier(Association association, ModelClass targetedClass, string identifier)
      {
         if (string.IsNullOrWhiteSpace(identifier) || !CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
            return "Name must be a valid .NET identifier";

         ModelClass offendingModelClass = targetedClass.AllAttributes.FirstOrDefault(x => x.Name == identifier)?.ModelClass
                                          ?? targetedClass.AllNavigationProperties(association).FirstOrDefault(x => x.PropertyName == identifier)?.ClassType;

         return offendingModelClass != null ? $"Duplicate symbol {identifier} in {offendingModelClass.Name}" : null;
      }
   }
}