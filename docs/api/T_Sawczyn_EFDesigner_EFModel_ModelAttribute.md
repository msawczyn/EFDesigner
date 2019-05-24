# ModelAttribute Class
 

DomainClass ModelAttribute An attribute of a class.


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a><br />&nbsp;&nbsp;&nbsp;&nbsp;Sawczyn.EFDesigner.EFModel.ModelAttribute<br />
**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.3.0.0

## Syntax

**C#**<br />
``` C#
public class ModelAttribute : <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>
```

**VB**<br />
``` VB
Public Class ModelAttribute
	Inherits <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/CustomCode/Partials/ModelAttribute.cs" title="View the source code">View Source</a><br />
The ModelAttribute type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_AutoProperty">AutoProperty</a></td><td>
Gets or sets the value of AutoProperty domain property. If false, generates a backing field with a partial method to hook getting and setting the property. If true, generates a simple auto property.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_CLRType">CLRType</a></td><td>
Converts a C# primitive type to a CLR type.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_ColumnName">ColumnName</a></td><td>
Gets or sets the value of ColumnName domain property. The name for the table column backing this property</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_ColumnType">ColumnType</a></td><td>
Gets or sets the value of ColumnType domain property. The data type for the table column backing this property</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_CustomAttributes">CustomAttributes</a></td><td>
Gets or sets the value of CustomAttributes domain property. Any custom attributes to be generated for this element. Will be passed through as entered.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Description">Description</a></td><td>
Gets or sets the value of Description domain property. Detailed code documentation</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_DisplayText">DisplayText</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_FQPrimitiveType">FQPrimitiveType</a></td><td>
Converts the attribute's CLR type to a C# primitive type.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_IdentityType">IdentityType</a></td><td>
Gets or sets the value of IdentityType domain property. If this property is an identity, how the value is generated</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Indexed">Indexed</a></td><td>
Gets or sets the value of Indexed domain property. Should this attribute create an index in the database?</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_IndexedUnique">IndexedUnique</a></td><td>
Gets or sets the value of IndexedUnique domain property. If indexed, is the index a unique index?</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_InitialValue">InitialValue</a></td><td>
Gets or sets the value of InitialValue domain property. Value for attribute when first created</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_IsConcurrencyToken">IsConcurrencyToken</a></td><td>
Gets or sets the value of IsConcurrencyToken domain property. If true, this property serves as the concurrency value for the class</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_IsIdentity">IsIdentity</a></td><td>
Gets or sets the value of IsIdentity domain property. Does this attribute represent the identity of the object?</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_MaxLength">MaxLength</a></td><td>
Gets or sets the value of MaxLength domain property. Maximum length of the string, 0 for no max length</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_MinLength">MinLength</a></td><td>
Gets or sets the value of MinLength domain property. Minimum length of the string, 0 for no minimum length</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_ModelClass">ModelClass</a></td><td>
Gets or sets ModelClass.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Name">Name</a></td><td>
Gets or sets the value of Name domain property. The name of the property. Must be a valid C# symbol.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_ParentModelElement">ParentModelElement</a></td><td>
Gets the parent model element (ModelClass).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139912" target="_blank">Partition</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Persistent">Persistent</a></td><td>
Gets or sets the value of Persistent domain property. Attribute will be stored in persistent storage</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_PrimitiveType">PrimitiveType</a></td><td>
From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity. Converts the attribute's CLR type to a C# primitive type.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_ReadOnly">ReadOnly</a></td><td>
Gets or sets the value of ReadOnly domain property. If true, no setter will be generated. Only valid for transient public properties.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Required">Required</a></td><td>
Gets or sets the value of Required domain property. If true, cannot be null (or the default value for the property type, if not nullable)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_SetterVisibility">SetterVisibility</a></td><td>
Gets or sets the value of SetterVisibility domain property. Visibility for property setter; getter is public</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139916" target="_blank">Store</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_StringType">StringType</a></td><td>
Gets or sets the value of StringType domain property. If not empty, will create a standard validation annotation for this attribute.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Summary">Summary</a></td><td>
Gets or sets the value of Summary domain property. Brief code documentation</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_TableOverride">TableOverride</a></td><td>
Gets or sets the value of TableOverride domain property. Unused - Alternate table to store this attribute. If empty, the class's table is used.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Type">Type</a></td><td>
Gets or sets the value of Type domain property. Type of this attribute</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelAttribute_Virtual">Virtual</a></td><td>
Gets or sets the value of Virtual domain property. If true, property will be generated with the 'virtual' keyword</td></tr></table>&nbsp;
<a href="#modelattribute-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb131174" target="_blank">GetDomainClass</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_ModelAttribute_IsValidInitialValue">IsValidInitialValue</a></td><td>
Tests if the InitialValue property is valid for the type indicated</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_ModelAttribute_ToCLRType">ToCLRType</a></td><td>
Converts a C# primitive type to a CLR type.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_ModelAttribute_ToPrimitiveType">ToPrimitiveType</a></td><td>
From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity. Converts a CLR type to a C# primitive type.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_ModelAttribute_ToString">ToString</a></td><td>
Returns a string that represents the current object.
 (Overrides <a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">Object.ToString()</a>.)</td></tr></table>&nbsp;
<a href="#modelattribute-class">Back to Top</a>

## See Also


#### Reference
<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />