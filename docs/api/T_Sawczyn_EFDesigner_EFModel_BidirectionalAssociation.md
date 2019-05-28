# BidirectionalAssociation Class
 

DomainRelationship BidirectionalAssociation


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_Sawczyn_EFDesigner_EFModel_Association">Sawczyn.EFDesigner.EFModel.Association</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sawczyn.EFDesigner.EFModel.BidirectionalAssociation<br />
**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.3.0.0

## Syntax

**C#**<br />
``` C#
public class BidirectionalAssociation : <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>
```

**VB**<br />
``` VB
Public Class BidirectionalAssociation
	Inherits <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/GeneratedCode/DomainRelationships.cs" title="View the source code">View Source</a><br />
The BidirectionalAssociation type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_BidirectionalSource">BidirectionalSource</a></td><td>
DomainRole BidirectionalSource</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_BidirectionalTarget">BidirectionalTarget</a></td><td>
DomainRole BidirectionalTarget</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_CollectionClass">CollectionClass</a></td><td> (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_ForeignKeyLocation">ForeignKeyLocation</a></td><td>
Gets or sets the value of ForeignKeyLocation domain property. Which class should hold the foreign key for this relationship
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139548" target="_blank">LinkedElements</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139912" target="_blank">Partition</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_Persistent">Persistent</a></td><td>
Gets or sets the value of Persistent domain property. If false, this is a transient association not stored in the database but instead created in code
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_Source">Source</a></td><td>
Gets the element playing BidirectionalSource domain role.
 (Overrides <a href="P_Sawczyn_EFDesigner_EFModel_Association_Source">Association.Source</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourceAutoProperty">SourceAutoProperty</a></td><td>
Gets or sets the value of SourceAutoProperty domain property. If false, generates a backing field with a partial method to hook getting and setting the property. If true, generates a simple auto property.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourceCustomAttributes">SourceCustomAttributes</a></td><td>
Gets or sets the value of SourceCustomAttributes domain property. Any custom attributes to be generated for the source property. Will be passed through as entered.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceDeleteAction">SourceDeleteAction</a></td><td>
Gets or sets the value of SourceDeleteAction domain property. The action to take when an entity on this end is deleted.
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourceDescription">SourceDescription</a></td><td>
Gets or sets the value of SourceDescription domain property. Detailed code documentation for this end of the association</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourceDisplayText">SourceDisplayText</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceMultiplicity">SourceMultiplicity</a></td><td>
Gets or sets the value of SourceMultiplicity domain property. The allowed number of entities at this end of the association
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceMultiplicityDisplay">SourceMultiplicityDisplay</a></td><td>
Gets or sets the value of SourceMultiplicityDisplay domain property.
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourcePropertyName">SourcePropertyName</a></td><td>
Gets or sets the value of SourcePropertyName domain property. Name of the entity property that returns the value at this end</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceRole">SourceRole</a></td><td>
Gets or sets the value of SourceRole domain property. Which object(s) in this association is/are the principal object(s)?
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourceSummary">SourceSummary</a></td><td>
Gets or sets the value of SourceSummary domain property. Short code documentation for this end of the association</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139916" target="_blank">Store</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_Summary">Summary</a></td><td>
Gets or sets the value of Summary domain property. Brief code documentation
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_Target">Target</a></td><td>
Gets the element playing BidirectionalTarget domain role.
 (Overrides <a href="P_Sawczyn_EFDesigner_EFModel_Association_Target">Association.Target</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetAutoProperty">TargetAutoProperty</a></td><td>
Gets or sets the value of TargetAutoProperty domain property. If false, generates a backing field with a partial method to hook getting and setting the property. If true, generates a simple auto property.
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetCustomAttributes">TargetCustomAttributes</a></td><td>
Gets or sets the value of TargetCustomAttributes domain property. Any custom attributes to be generated for the target property. Will be passed through as entered.
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetDeleteAction">TargetDeleteAction</a></td><td>
Gets or sets the value of TargetDeleteAction domain property. The action to take when an entity on this end is deleted.
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetDescription">TargetDescription</a></td><td>
Gets or sets the value of TargetDescription domain property. Detailed code documentation for this end of the association
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetDisplayText">TargetDisplayText</a></td><td> (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetMultiplicity">TargetMultiplicity</a></td><td>
Gets or sets the value of TargetMultiplicity domain property. The allowed number of entities at this end of the association
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetMultiplicityDisplay">TargetMultiplicityDisplay</a></td><td>
Gets or sets the value of TargetMultiplicityDisplay domain property.
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetPropertyName">TargetPropertyName</a></td><td>
Gets or sets the value of TargetPropertyName domain property. Name of the entity property that returns the value at this end
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetRole">TargetRole</a></td><td>
Gets or sets the value of TargetRole domain property. Which object(s) in this association is/are the dependent object(s)?
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetSummary">TargetSummary</a></td><td>
Gets or sets the value of TargetSummary domain property. Short code documentation for this end of the association
 (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr></table>&nbsp;
<a href="#bidirectionalassociation-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_GetBidirectionalSources">GetBidirectionalSources</a></td><td>
Gets a list of BidirectionalSources.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_GetBidirectionalTargets">GetBidirectionalTargets</a></td><td>
Gets a list of BidirectionalTargets.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb131174" target="_blank">GetDomainClass</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb130677" target="_blank">GetDomainRelationship</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_GetLinks">GetLinks</a></td><td>
Get any BidirectionalAssociation links between a given ModelClass and a ModelClass.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_GetLinksToBidirectionalSources">GetLinksToBidirectionalSources</a></td><td>
Get the list of BidirectionalAssociation links to a ModelClass.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_GetLinksToBidirectionalTargets">GetLinksToBidirectionalTargets</a></td><td>
Get the list of BidirectionalAssociation links to a ModelClass.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetSourceMultiplicityDisplayValue">GetSourceMultiplicityDisplayValue</a></td><td> (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetTargetMultiplicityDisplayValue">GetTargetMultiplicityDisplayValue</a></td><td> (Inherited from <a href="T_Sawczyn_EFDesigner_EFModel_Association">Association</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#bidirectionalassociation-class">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation_SourceAutoPropertyDomainPropertyId">SourceAutoPropertyDomainPropertyId</a></td><td>
SourceAutoProperty domain property Id.</td></tr></table>&nbsp;
<a href="#bidirectionalassociation-class">Back to Top</a>

## Extension Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_GetActiveDiagram">GetActiveDiagram</a></td><td> (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_GetCompartment">GetCompartment</a></td><td>
Gets the named compartment in this element
 (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_GetFirstShapeElement">GetFirstShapeElement</a></td><td> (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_InvalidateDiagrams">InvalidateDiagrams</a></td><td>
Causes all diagrams to redraw
 (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_LocateInDiagram">LocateInDiagram</a></td><td> (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr></table>&nbsp;
<a href="#bidirectionalassociation-class">Back to Top</a>

## See Also


#### Reference
<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />