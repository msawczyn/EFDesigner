










using System;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
// ReSharper disable LoopCanBePartlyConvertedToQuery
// ReSharper disable RedundantNameQualifier


namespace Sawczyn.EFDesigner.EFModel
{

// Provides mechanism to synchronize a new diagram with current elements available from the IMS.
internal static class EFModelSynchronizationHelper // HACK : MEXEDGE
{
   internal static void FixUp(global::Sawczyn.EFDesigner.EFModel.EFModelDiagram diagram)
   {


      // 300010
      Guid[] l_300010_domainClassesIds = {
       };

      using (Transaction transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddShapeRulePriority:Core"))
      {
         foreach (Guid domainClassId in l_300010_domainClassesIds)
         {
            ReadOnlyCollection<ModelElement> modelElements = diagram.Store.ElementDirectory.FindElements(domainClassId);
            foreach (ModelElement modelElement in modelElements.Where(diagram.ShouldSupport))
               FixUpAllDiagrams.FixUp(diagram, modelElement);    
         }
         transaction.Commit();
      }


      // 300020
      Guid[] l_300020_domainClassesIds = {
       
  global::Sawczyn.EFDesigner.EFModel.Comment.DomainClassId,
       
  global::Sawczyn.EFDesigner.EFModel.ModelEnum.DomainClassId,
       
  global::Sawczyn.EFDesigner.EFModel.ModelClass.DomainClassId,
       };

      using (Transaction transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddShapeRulePriority:Core"))
      {
         foreach (Guid domainClassId in l_300020_domainClassesIds)
         {
            ReadOnlyCollection<ModelElement> modelElements = diagram.Store.ElementDirectory.FindElements(domainClassId);
            foreach (ModelElement modelElement in modelElements.Where(diagram.ShouldSupport))
               FixUpAllDiagrams.FixUp(diagram, modelElement);    
         }
         transaction.Commit();
      }


      // 300021
      Guid[] l_300021_domainClassesIds = {
       };

      using (Transaction transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddShapeRulePriority:Core"))
      {
         foreach (Guid domainClassId in l_300021_domainClassesIds)
         {
            ReadOnlyCollection<ModelElement> modelElements = diagram.Store.ElementDirectory.FindElements(domainClassId);
            foreach (ModelElement modelElement in modelElements.Where(diagram.ShouldSupport))
               FixUpAllDiagrams.FixUp(diagram, modelElement);    
         }
         transaction.Commit();
      }


      // 300100
      Guid[] l_300100_domainClassesIds = {
       
  global::Sawczyn.EFDesigner.EFModel.CommentReferencesSubjects.DomainClassId,
       
  global::Sawczyn.EFDesigner.EFModel.UnidirectionalAssociation.DomainClassId,
       
  global::Sawczyn.EFDesigner.EFModel.Generalization.DomainClassId,
       
  global::Sawczyn.EFDesigner.EFModel.BidirectionalAssociation.DomainClassId,
       };

      using (Transaction transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddConnectionRulePriority:AutoLayout"))
      {
         diagram.AutoLayoutShapeElements(diagram.NestedChildShapes,
         Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.VGRoutingStyle.VGRouteTreeNS,
            Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.PlacementValueStyle.VGPlaceSN, true);
         transaction.Commit();
      }

      using(Transaction transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddConnectionRulePriority:Core"))
      {
         foreach (Guid domainClassId in l_300100_domainClassesIds)
         {
            ReadOnlyCollection<ModelElement> modelElements = diagram.Store.ElementDirectory.FindElements(domainClassId);

            foreach (ModelElement modelElement in modelElements.Where(diagram.ShouldSupport))
               FixUpAllDiagrams.FixUp(diagram, modelElement);    
         }
         transaction.Commit();
      }

      diagram.OnSynchronized();

   }

}

}
