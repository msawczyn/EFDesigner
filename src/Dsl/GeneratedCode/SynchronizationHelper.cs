using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using MexModeling = global::Mexedge.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
	// Provides mechanism to synchronize a new diagram with current elements available from the IMS.
	internal static class EFModelSynchronizationHelper // HACK : MEXEDGE
	{
	    internal static void FixUp(global::Sawczyn.EFDesigner.EFModel.EFModelDiagram diagram)
	    {
	        // 300010
	        var l_300010_domainClassesIds = new Guid[] {
				     };
	        using(var transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddShapeRulePriority:Core"))
	        {
	            foreach(var l_domainClassId in l_300010_domainClassesIds)
	            {
	                var l_modelElements = diagram.Store.ElementDirectory.FindElements(l_domainClassId);
					foreach (var modelElement in l_modelElements)
		            {
	                    if(diagram.ShouldSupport(modelElement))
	                    {
	                        FixUpAllDiagrams.FixUp(diagram, modelElement);    
	                    }					
		            }
	            }
	            transaction.Commit();
	        }
	        // 300020
	        var l_300020_domainClassesIds = new Guid[] {
				        global::Sawczyn.EFDesigner.EFModel.Comment.DomainClassId,
				        global::Sawczyn.EFDesigner.EFModel.ModelEnum.DomainClassId,
				        global::Sawczyn.EFDesigner.EFModel.ModelClass.DomainClassId,
				     };
	        using(var transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddShapeRulePriority:Core"))
	        {
	            foreach(var l_domainClassId in l_300020_domainClassesIds)
	            {
	                var l_modelElements = diagram.Store.ElementDirectory.FindElements(l_domainClassId);
					foreach (var modelElement in l_modelElements)
		            {
	                    if(diagram.ShouldSupport(modelElement))
	                    {
	                        FixUpAllDiagrams.FixUp(diagram, modelElement);    
	                    }					
		            }
	            }
	            transaction.Commit();
	        }
	        // 300021
	        var l_300021_domainClassesIds = new Guid[] {
				     };
	        using(var transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddShapeRulePriority:Core"))
	        {
	            foreach(var l_domainClassId in l_300021_domainClassesIds)
	            {
	                var l_modelElements = diagram.Store.ElementDirectory.FindElements(l_domainClassId);
					foreach (var modelElement in l_modelElements)
		            {
	                    if(diagram.ShouldSupport(modelElement))
	                    {
	                        FixUpAllDiagrams.FixUp(diagram, modelElement);    
	                    }					
		            }
	            }
	            transaction.Commit();
	        }
	        // 300100
	        var l_300100_domainClassesIds = new Guid[] {
				        global::Sawczyn.EFDesigner.EFModel.CommentReferencesSubjects.DomainClassId,
				        global::Sawczyn.EFDesigner.EFModel.UnidirectionalAssociation.DomainClassId,
				        global::Sawczyn.EFDesigner.EFModel.Generalization.DomainClassId,
				        global::Sawczyn.EFDesigner.EFModel.BidirectionalAssociation.DomainClassId,
				     };
	        using (var transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddConnectionRulePriority:AutoLayout"))
	        {
	            diagram.AutoLayoutShapeElements(diagram.NestedChildShapes,
	            Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.VGRoutingStyle.VGRouteTreeNS,
	                Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.PlacementValueStyle.VGPlaceSN, true);
	            transaction.Commit();
	        }
	
	        using(var transaction = diagram.Store.TransactionManager.BeginTransaction("FixUp:AddConnectionRulePriority:Core"))
	        {
	            foreach(var l_domainClassId in l_300100_domainClassesIds)
	            {
	                var l_modelElements = diagram.Store.ElementDirectory.FindElements(l_domainClassId);
	
	                foreach (var modelElement in l_modelElements)
		            {
	                    if(diagram.ShouldSupport(modelElement))
	                    {
	                        FixUpAllDiagrams.FixUp(diagram, modelElement);    
	                    }					
		            }
	            }
	            transaction.Commit();
	        }
	
	        diagram.OnSynchronized();
	    }
	}
}
