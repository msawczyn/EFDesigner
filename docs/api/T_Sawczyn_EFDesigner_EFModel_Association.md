# Association Class
 

DomainRelationship Association Associations between Classes.


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sawczyn.EFDesigner.EFModel.Association<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_Sawczyn_EFDesigner_EFModel_BidirectionalAssociation">Sawczyn.EFDesigner.EFModel.BidirectionalAssociation</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_Sawczyn_EFDesigner_EFModel_UnidirectionalAssociation">Sawczyn.EFDesigner.EFModel.UnidirectionalAssociation</a><br />
**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.3.0.0

## Syntax

**C#**<br />
``` C#
public abstract class Association : <a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a>
```

**VB**<br />
``` VB
Public MustInherit Class Association
	Inherits <a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/CustomCode/Partials/Association.cs" title="View the source code">View Source</a><br />
The Association type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_CollectionClass">CollectionClass</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_ForeignKeyLocation">ForeignKeyLocation</a></td><td>
Gets or sets the value of ForeignKeyLocation domain property. Which class should hold the foreign key for this relationship</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139548" target="_blank">LinkedElements</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139912" target="_blank">Partition</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_Persistent">Persistent</a></td><td>
Gets or sets the value of Persistent domain property. If false, this is a transient association not stored in the database but instead created in code</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_Source">Source</a></td><td>
DomainRole Source</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceDeleteAction">SourceDeleteAction</a></td><td>
Gets or sets the value of SourceDeleteAction domain property. The action to take when an entity on this end is deleted.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceMultiplicity">SourceMultiplicity</a></td><td>
Gets or sets the value of SourceMultiplicity domain property. The allowed number of entities at this end of the association</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceMultiplicityDisplay">SourceMultiplicityDisplay</a></td><td>
Gets or sets the value of SourceMultiplicityDisplay domain property.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_SourceRole">SourceRole</a></td><td>
Gets or sets the value of SourceRole domain property. Which object(s) in this association is/are the principal object(s)?</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139916" target="_blank">Store</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_Summary">Summary</a></td><td>
Gets or sets the value of Summary domain property. Brief code documentation</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_Target">Target</a></td><td>
DomainRole Target</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetAutoProperty">TargetAutoProperty</a></td><td>
Gets or sets the value of TargetAutoProperty domain property. If false, generates a backing field with a partial method to hook getting and setting the property. If true, generates a simple auto property.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetCustomAttributes">TargetCustomAttributes</a></td><td>
Gets or sets the value of TargetCustomAttributes domain property. Any custom attributes to be generated for the target property. Will be passed through as entered.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetDeleteAction">TargetDeleteAction</a></td><td>
Gets or sets the value of TargetDeleteAction domain property. The action to take when an entity on this end is deleted.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetDescription">TargetDescription</a></td><td>
Gets or sets the value of TargetDescription domain property. Detailed code documentation for this end of the association</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetDisplayText">TargetDisplayText</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetMultiplicity">TargetMultiplicity</a></td><td>
Gets or sets the value of TargetMultiplicity domain property. The allowed number of entities at this end of the association</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetMultiplicityDisplay">TargetMultiplicityDisplay</a></td><td>
Gets or sets the value of TargetMultiplicityDisplay domain property.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetPropertyName">TargetPropertyName</a></td><td>
Gets or sets the value of TargetPropertyName domain property. Name of the entity property that returns the value at this end</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetRole">TargetRole</a></td><td>
Gets or sets the value of TargetRole domain property. Which object(s) in this association is/are the dependent object(s)?</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_Association_TargetSummary">TargetSummary</a></td><td>
Gets or sets the value of TargetSummary domain property. Short code documentation for this end of the association</td></tr></table>&nbsp;
<a href="#association-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb131174" target="_blank">GetDomainClass</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb130677" target="_blank">GetDomainRelationship</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162510" target="_blank">ElementLink</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetLinks">GetLinks</a></td><td>
Get any Association links between a given ModelClass and a ModelClass.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetLinksToSources">GetLinksToSources</a></td><td>
Get the list of Association links to a ModelClass.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetLinksToTargets">GetLinksToTargets</a></td><td>
Get the list of Association links to a ModelClass.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetSourceMultiplicityDisplayValue">GetSourceMultiplicityDisplayValue</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetSources">GetSources</a></td><td>
Gets a list of Sources.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetTargetMultiplicityDisplayValue">GetTargetMultiplicityDisplayValue</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Association_GetTargets">GetTargets</a></td><td>
Gets a list of Targets.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#association-class">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_Sawczyn_EFDesigner_EFModel_Association_TargetAutoPropertyDomainPropertyId">TargetAutoPropertyDomainPropertyId</a></td><td>
TargetAutoProperty domain property Id.</td></tr></table>&nbsp;
<a href="#association-class">Back to Top</a>

## See Also


#### Reference
<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />