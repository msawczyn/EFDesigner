# DesignElement Class
 

DomainClass DesignElement Description for Sawczyn.EFDesigner.EFModel.DesignElement


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a><br />&nbsp;&nbsp;&nbsp;&nbsp;Sawczyn.EFDesigner.EFModel.DesignElement<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_Sawczyn_EFDesigner_EFModel_ModelClass">Sawczyn.EFDesigner.EFModel.ModelClass</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_Sawczyn_EFDesigner_EFModel_ModelEnum">Sawczyn.EFDesigner.EFModel.ModelEnum</a><br />
**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.3.0.0

## Syntax

**C#**<br />
``` C#
public abstract class DesignElement : <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>
```

**VB**<br />
``` VB
Public MustInherit Class DesignElement
	Inherits <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/GeneratedCode/DomainClasses.cs" title="View the source code">View Source</a><br />
The DesignElement type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_DesignElement_Comments">Comments</a></td><td>
Gets a list of Comments. Description for Sawczyn.EFDesigner.EFModel.CommentReferencesSubjects.DesignElement</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139912" target="_blank">Partition</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139916" target="_blank">Store</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr></table>&nbsp;
<a href="#designelement-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb131174" target="_blank">GetDomainClass</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#designelement-class">Back to Top</a>

## Extension Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_GetActiveDiagram">GetActiveDiagram</a></td><td> (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_GetCompartment">GetCompartment</a></td><td>
Gets the named compartment in this element
 (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_GetFirstShapeElement">GetFirstShapeElement</a></td><td> (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_InvalidateDiagrams">InvalidateDiagrams</a></td><td>
Causes all diagrams to redraw
 (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr><tr><td>![Public Extension Method](media/pubextension.gif "Public Extension Method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions_LocateInDiagram">LocateInDiagram</a></td><td> (Defined by <a href="T_Sawczyn_EFDesigner_EFModel_Extensions_ModelElementExtensions">ModelElementExtensions</a>.)</td></tr></table>&nbsp;
<a href="#designelement-class">Back to Top</a>

## See Also


#### Reference
<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />