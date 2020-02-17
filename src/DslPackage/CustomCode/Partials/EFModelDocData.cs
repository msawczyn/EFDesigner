using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Sawczyn.EFDesigner.EFModel.Extensions;

using VSLangProj;

namespace Sawczyn.EFDesigner.EFModel
{
   [SuppressMessage("ReSharper", "RemoveRedundantBraces")]
   internal partial class EFModelDocData
   {
      private static DTE _dte;
      private static DTE2 _dte2;
      private IComponentModel _componentModel;
      private IVsOutputWindowPane _outputWindow;

      internal static DTE Dte => _dte ?? (_dte = Package.GetGlobalService(typeof(DTE)) as DTE);
      internal static DTE2 Dte2 => _dte2 ?? (_dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2);
      internal IComponentModel ComponentModel => _componentModel ?? (_componentModel = (IComponentModel)GetService(typeof(SComponentModel)));
      internal IVsOutputWindowPane OutputWindow => _outputWindow ?? (_outputWindow = (IVsOutputWindowPane)GetService(typeof(SVsGeneralOutputWindowPane)));

      internal static Project ActiveProject => Dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0
                                                 ? activeSolutionProjects.GetValue(0) as Project
                                                 : null;

      internal static void GenerateCode()
      {
         GenerateCode(null);
      }

      internal static void GenerateCode(string filepath)
      {
         ProjectItem modelProjectItem = Dte2.Solution.FindProjectItem(filepath ?? Dte2.ActiveDocument.FullName);

         if (Guid.Parse(modelProjectItem.Kind) == VSConstants.GUID_ItemType_PhysicalFile)
            modelProjectItem.Save();

         string templateFilename = Path.ChangeExtension(filepath ?? Dte2.ActiveDocument.FullName, "tt");

         ProjectItem templateProjectItem = Dte2.Solution.FindProjectItem(templateFilename);
#pragma warning disable IDE0019 // Use pattern matching
         VSProjectItem templateVsProjectItem = templateProjectItem?.Object as VSProjectItem;
#pragma warning restore IDE0019 // Use pattern matching

         if (templateVsProjectItem == null)
            Messages.AddError($"Tried to generate code but couldn't find {templateFilename} in the solution.");
         else
         {

            try
            {
               Dte.StatusBar.Text = $"Generating code from {templateFilename}";
               templateVsProjectItem.RunCustomTool();
               Dte.StatusBar.Text = $"Finished generating code from {templateFilename}";
            }
            catch (COMException)
            {
               string message = $"Encountered an error generating code from {templateFilename}. Please transform T4 template manually.";
               Dte.StatusBar.Text = message;
               Messages.AddError(message);
            }
         }
      }

      /// <summary>
      /// Called before the document is initially loaded with data.
      /// </summary>
      protected override void OnDocumentLoading(EventArgs e)
      {
         base.OnDocumentLoading(e);
         ValidationController?.ClearMessages();
      }

      /// <summary>
      /// Called when user double clicks on a class shape
      /// </summary>
      /// <param name="modelClass"></param>
      internal static bool OpenFileFor(ModelClass modelClass)
      {
         Project activeProject = ActiveProject;

         if (activeProject != null)
         {
            string projectDirectory = Path.GetDirectoryName(activeProject.FullName);
            Debug.Assert(projectDirectory != null, nameof(projectDirectory) + " != null");
            string filename = Path.Combine(projectDirectory, modelClass.GetRelativeFileName());
            if (File.Exists(filename))
            {
               Dte.ItemOperations.OpenFile(filename);
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Called when user double clicks on a enum shape
      /// </summary>
      /// <param name="modelEnum"></param>
      internal static bool OpenFileFor(ModelEnum modelEnum)
      {
         Project activeProject = ActiveProject;

         if (activeProject != null)
         {
            string projectDirectory = Path.GetDirectoryName(activeProject.FullName);
            Debug.Assert(projectDirectory != null, nameof(projectDirectory) + " != null");
            string filename = Path.Combine(projectDirectory, modelEnum.GetRelativeFileName());
            if (File.Exists(filename))
            {
               Dte.ItemOperations.OpenFile(filename);
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Called on both document load and reload.
      /// </summary>
      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();
         ErrorDisplay.RegisterDisplayHandler(ShowError);
         WarningDisplay.RegisterDisplayHandler(ShowWarning);
         QuestionDisplay.RegisterDisplayHandler(ShowBooleanQuestionBox);
         StatusDisplay.RegisterDisplayHandler(ShowStatus);
         ChoiceDisplay.RegisterDisplayHandler(GetChoice);
         ModelDisplay.RegisterLayoutDiagramAction(Commands.LayoutDiagram);

         ClassShape.OpenCodeFile = OpenFileFor;
         ClassShape.ExecCodeGeneration = GenerateCode;
         EnumShape.OpenCodeFile = OpenFileFor;
         EnumShape.ExecCodeGeneration = GenerateCode;
         ModelRoot.ExecuteValidator = ValidateAll;
         ModelDiagramData.DisplayDiagram = DisplayDiagram;

         if (!(RootElement is ModelRoot modelRoot)) return;

         ModelRoot.CanLoadNugetPackages = false;

         // set to the project's namespace if no namespace set
         if (string.IsNullOrEmpty(modelRoot.Namespace))
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("SetDefaultNamespace"))
            {
               modelRoot.Namespace = ActiveProject.Properties.Item("DefaultNamespace")?.Value as string;
               tx.Commit();
            }
         }

         ReadOnlyCollection<Association> associations = modelRoot.Store.ElementDirectory.FindElements<Association>();

         if (associations.Any())
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("StyleConnectors"))
            {
               // style association connectors if needed
               foreach (Association association in associations)
               {
                  PresentationHelper.UpdateAssociationDisplay(association);

                  // for older diagrams that didn't calculate this initially
                  AssociationChangeRules.SetEndpointRoles(association);
               }

               tx.Commit();
            }
         }

         List<GeneralizationConnector> generalizationConnectors = modelRoot.Store
                                                                           .ElementDirectory
                                                                           .FindElements<GeneralizationConnector>()
                                                                           .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();
         List<AssociationConnector> associationConnectors = modelRoot.Store
                                                                     .ElementDirectory
                                                                     .FindElements<AssociationConnector>()
                                                                     .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();

         if (generalizationConnectors.Any() || associationConnectors.Any())
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("HideConnectors"))
            {
               // hide any connectors that may have been hidden due to hidden shapes
               foreach (GeneralizationConnector connector in generalizationConnectors)
                  connector.Hide();

               foreach (AssociationConnector connector in associationConnectors)
                  connector.Hide();

               tx.Commit();
            }
         }

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("SetClassVisuals"))
         {
            foreach (ModelClass modelClass in modelRoot.Store.ElementDirectory.FindElements<ModelClass>())
               PresentationHelper.UpdateClassDisplay(modelClass);
            tx.Commit();
         }

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("ValidateOnChanges"))
         {
            // validate classes that show warnings, so that we can change glyphs accordingly
            List<DomainClassInfo> classesWithWarnings = Store.ElementDirectory
                                                             .AllElements
                                                             .OfType<IDisplaysWarning>()
                                                             .OfType<ModelElement>()
                                                             .Select(e => e.GetDomainClass())
                                                             .Distinct()
                                                             .ToList();

            EventManagerDirectory events = Store.EventManagerDirectory;

            foreach (DomainClassInfo classInfo in classesWithWarnings)
            {
               events.ElementPropertyChanged.Add(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValidateModelElement));
               events.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(ValidateModelElement));   
            }

            tx.Commit();
         }

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("Diagrams"))
         {
            List<EFModelDiagram> diagrams = Store.ElementDirectory.FindElements<EFModelDiagram>().ToList();
            modelRoot.Diagrams.Clear();
            List<ModelDiagramData> matched = new List<ModelDiagramData>();

            foreach (EFModelDiagram efModelDiagram in diagrams)
            {
               ModelDiagramData diagramDataObject = modelRoot.Diagrams.FirstOrDefault(d => d.Name == efModelDiagram.Name);

               if (diagramDataObject == null)
               {
                  diagramDataObject = new ModelDiagramData(Store, new PropertyAssignment(ModelDiagramData.NameDomainPropertyId, efModelDiagram.Name));
                  modelRoot.Diagrams.Add(diagramDataObject);
               }

               matched.Add(diagramDataObject);
               diagramDataObject.SetDiagram(efModelDiagram);
            }

            for (int index  = 0; index < modelRoot.Diagrams.Count; index++)
            {
               ModelDiagramData diagramDataObject = modelRoot.Diagrams[index];

               if (matched.All(d => d.Name != diagramDataObject.Name))
                  modelRoot.Diagrams.RemoveAt(index--);
            }

            tx.Commit();
         }

         SetDocDataDirty(0);
      }

      private void DisplayDiagram(string diagramName)
      {
         OpenView(Constants.LogicalView, new Mexedge.VisualStudio.Modeling.ViewContext(diagramName, typeof(EFModelDiagram), RootElement));
      }

      private void ValidateAll()
      {
         ValidationCategories allCategories = ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Custom | ValidationCategories.Load;
         Store.Get<IDisplaysWarning>().ToList().ForEach(e => e.ResetWarning());
         ValidationController?.ClearMessages();
         ValidationController?.Validate(Store.ElementDirectory.AllElements, allCategories);
      }

      private void ValidateModelElement(object sender, ElementPropertyChangedEventArgs e)
      {
         ModelElement modelElement = e.ModelElement;
         ValidateModelElement(modelElement);
      }

      private void ValidateModelElement(object sender, ElementAddedEventArgs e)
      {
         ModelElement modelElement = e.ModelElement;
         ValidateModelElement(modelElement);
      }

      private void ValidateModelElement(ModelElement modelElement)
      {
         if (!ModelRoot.BatchUpdating && modelElement is IDisplaysWarning displaysWarningElement)
         {
            StatusDisplay.Show($"Validating {modelElement.GetDisplayText()}");
            displaysWarningElement.ResetWarning();

            ValidationCategories allCategories = ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Custom | ValidationCategories.Load;
            ValidationController.Validate(modelElement, allCategories);
            
            displaysWarningElement.RedrawItem();
         }
      }

      private DialogResult ShowQuestionBox(string question)
      {
         return PackageUtility.ShowMessageBox(ServiceProvider, 
                                              question, 
                                              OLEMSGBUTTON.OLEMSGBUTTON_YESNO, 
                                              OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, 
                                              OLEMSGICON.OLEMSGICON_QUERY);
      }

      private bool ShowBooleanQuestionBox(string question)
      {
         return ShowQuestionBox(question) == DialogResult.Yes;
      }

      // ReSharper disable once UnusedMember.Local
      private void ShowMessage(string message)
      {
         Messages.AddMessage(message);
      }

      private void ShowWarning(string message)
      {
         Messages.AddWarning(message);
      }

      private void ShowStatus(string message)
      {
         Messages.AddStatus(message);
      }

      private string GetChoice(string title, IEnumerable<string> choices)
      {
         return Messages.GetChoice(title, choices);
      }

      public override IEnumerable<ModelElement> GetAllElementsForValidation()
      {
         List<ModelElement> elements = base.GetAllElementsForValidation().ToList();
         elements.OfType<IDisplaysWarning>().ToList().ForEach(e => e.ResetWarning());

         return elements;
      }

      private void ShowError(string message)
      {
         Messages.AddError(message);
         PackageUtility.ShowMessageBox(ServiceProvider, 
                                       message, 
                                       OLEMSGBUTTON.OLEMSGBUTTON_OK, 
                                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, 
                                       OLEMSGICON.OLEMSGICON_CRITICAL);
      }

      /// <summary>
      /// Validate the model before the file is saved.
      /// </summary>
      protected override bool CanSave(bool allowUserInterface)
      {
         if (allowUserInterface)
            ValidationController?.ClearMessages();
         return base.CanSave(allowUserInterface);
      }

      protected override void OnDocumentSaved(EventArgs e)
      {
         base.OnDocumentSaved(e);

         if (RootElement is ModelRoot modelRoot)
         {
            if (modelRoot.TransformOnSave)
               GenerateCode(((DocumentSavedEventArgs)e).NewFileName);

            // add other post-save processing as needed
         }
      }

      public void Merge(UnidirectionalAssociation[] selected)
      {
         if (!(RootElement is ModelRoot modelRoot)) return;

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("Merge associations"))
         {
            selected[0].Delete();
            selected[1].Delete();

            // ReSharper disable once UnusedVariable
            BidirectionalAssociation element = new BidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(BidirectionalAssociation.BidirectionalSourceDomainRoleId, selected[0].Source),
                                                      new RoleAssignment(BidirectionalAssociation.BidirectionalTargetDomainRoleId, selected[1].Source)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(BidirectionalAssociation.SourcePropertyNameDomainPropertyId, selected[1].TargetPropertyName), 
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, selected[0].TargetPropertyName), 
                                                     
                                                      new PropertyAssignment(BidirectionalAssociation.SourceCustomAttributesDomainPropertyId, selected[1].TargetCustomAttributes), 
                                                      new PropertyAssignment(Association.TargetCustomAttributesDomainPropertyId, selected[0].TargetCustomAttributes), 

                                                      new PropertyAssignment(BidirectionalAssociation.SourceDisplayTextDomainPropertyId, selected[1].TargetDisplayText), 
                                                      new PropertyAssignment(Association.TargetDisplayTextDomainPropertyId, selected[0].TargetDisplayText), 

                                                      new PropertyAssignment(BidirectionalAssociation.SourceSummaryDomainPropertyId, selected[1].TargetSummary), 
                                                      new PropertyAssignment(BidirectionalAssociation.SourceDescriptionDomainPropertyId, selected[1].TargetDescription),

                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, selected[0].TargetSummary), 
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, selected[0].TargetDescription), 

                                                      new PropertyAssignment(Association.SourceDeleteActionDomainPropertyId, selected[1].TargetDeleteAction), 
                                                      new PropertyAssignment(Association.TargetDeleteActionDomainPropertyId, selected[0].TargetDeleteAction), 

                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, selected[1].TargetRole), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, selected[0].TargetRole), 

                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, selected[1].TargetMultiplicity), 
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, selected[0].TargetMultiplicity), 
                                                   });
            tx.Commit();
         }
      }

      public void Split(BidirectionalAssociation selected)
      {
         if (!(RootElement is ModelRoot modelRoot)) return;

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("Split associations"))
         {
            selected.Delete();

            // ReSharper disable once UnusedVariable
            UnidirectionalAssociation element1 = new UnidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(Association.SourceDomainRoleId, selected.Source),
                                                      new RoleAssignment(Association.TargetDomainRoleId, selected.Target)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, selected.TargetPropertyName), 
                                                     
                                                      new PropertyAssignment(Association.TargetCustomAttributesDomainPropertyId, selected.TargetCustomAttributes), 

                                                      new PropertyAssignment(Association.TargetDisplayTextDomainPropertyId, selected.TargetDisplayText), 

                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, selected.TargetSummary), 
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, selected.TargetDescription), 

                                                      new PropertyAssignment(Association.SourceDeleteActionDomainPropertyId, selected.SourceDeleteAction), 
                                                      new PropertyAssignment(Association.TargetDeleteActionDomainPropertyId, selected.TargetDeleteAction), 

                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, selected.SourceRole), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, selected.TargetRole), 

                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, selected.SourceMultiplicity), 
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, selected.TargetMultiplicity), 
                                                   });

            // ReSharper disable once UnusedVariable
            UnidirectionalAssociation element2 = new UnidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(Association.SourceDomainRoleId, selected.Target),
                                                      new RoleAssignment(Association.TargetDomainRoleId, selected.Source)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, selected.SourcePropertyName), 
                                                     
                                                      new PropertyAssignment(Association.TargetCustomAttributesDomainPropertyId, selected.SourceCustomAttributes), 

                                                      new PropertyAssignment(Association.TargetDisplayTextDomainPropertyId, selected.SourceDisplayText), 

                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, selected.SourceSummary), 
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, selected.SourceDescription), 

                                                      new PropertyAssignment(Association.SourceDeleteActionDomainPropertyId, selected.TargetDeleteAction), 
                                                      new PropertyAssignment(Association.TargetDeleteActionDomainPropertyId, selected.SourceDeleteAction), 

                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, selected.TargetRole), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, selected.SourceRole), 

                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, selected.TargetMultiplicity), 
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, selected.SourceMultiplicity), 
                                                   });

            tx.Commit();
         }
      }

      protected override void CleanupOldDiagramFiles()
      {
         string diagramsFileName = FileName + this.DiagramExtension;
         if (diagramsFileName.EndsWith("x"))
         {
            string oldDiagramFileName = diagramsFileName.TrimEnd('x');

            if (File.Exists(oldDiagramFileName))
               Dte2.Solution.FindProjectItem(oldDiagramFileName)?.Delete();
         }
      }
   }
}
