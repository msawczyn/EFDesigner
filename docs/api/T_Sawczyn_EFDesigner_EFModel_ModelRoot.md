# ModelRoot Class
 

DomainClass ModelRoot


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a><br />&nbsp;&nbsp;&nbsp;&nbsp;Sawczyn.EFDesigner.EFModel.ModelRoot<br />
**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.2.7.0

## Syntax

**C#**<br />
``` C#
public class ModelRoot : <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>
```

**VB**<br />
``` VB
Public Class ModelRoot
	Inherits <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/CustomCode/Partials/ModelRoot.cs" title="View the source code">View Source</a><br />
The ModelRoot type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_AutomaticMigrationsEnabled">AutomaticMigrationsEnabled</a></td><td>
Gets or sets the value of AutomaticMigrationsEnabled domain property. If true, automatic database migrations will be run when changes are detected.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")![Static member](media/static.gif "Static member")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_CanLoadNugetPackages">CanLoadNugetPackages</a></td><td>
DslPackage might set this to false depending on whether or not it can find the resources needed to load Nuget packages</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ChopMethodChains">ChopMethodChains</a></td><td>
Gets or sets the value of ChopMethodChains domain property. Will chop generated code method chains</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Classes">Classes</a></td><td>
Gets a list of Classes. Description for Sawczyn.EFDesigner.EFModel.ModelRootHasClasses.ModelRoot</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Comments">Comments</a></td><td>
Gets a list of Comments.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ConcurrencyDefault">ConcurrencyDefault</a></td><td>
Gets or sets the value of ConcurrencyDefault domain property. Default concurrency handling strategy</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ConnectionString">ConnectionString</a></td><td>
Gets or sets the value of ConnectionString domain property. Connection string to use. Mutually exclusive with ConnectionStringName</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ConnectionStringName">ConnectionStringName</a></td><td>
Gets or sets the value of ConnectionStringName domain property. Name of connection string in config file. Mutually exclusive with ConnectionString</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ContextOutputDirectory">ContextOutputDirectory</a></td><td>
Gets or sets the value of ContextOutputDirectory domain property. Project directory for DbContext-related files</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_DatabaseInitializerType">DatabaseInitializerType</a></td><td>
Gets or sets the value of DatabaseInitializerType domain property. Initialization strategy to synchronize the underlying database when an instance of this context is used for the first time</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_DatabaseSchema">DatabaseSchema</a></td><td>
Gets or sets the value of DatabaseSchema domain property. The schema to use for table creation</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_DatabaseType">DatabaseType</a></td><td>
Gets or sets the value of DatabaseType domain property. Database manifest token. Optimization if runtime database type is known and unchanging, and only valid for SqlServer.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_DbSetAccess">DbSetAccess</a></td><td>
Gets or sets the value of DbSetAccess domain property. Code visibility for DbSets</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_DefaultCollectionClass">DefaultCollectionClass</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_DefaultIdentityType">DefaultIdentityType</a></td><td>
Gets or sets the value of DefaultIdentityType domain property. Default type for ID properties in new classes</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Description">Description</a></td><td>
Gets or sets the value of Description domain property. Detailed code documentation</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_EntityContainerAccess">EntityContainerAccess</a></td><td>
Gets or sets the value of EntityContainerAccess domain property. Code visibility for entity container</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_EntityContainerName">EntityContainerName</a></td><td>
Gets or sets the value of EntityContainerName domain property. Name of entity container class</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_EntityFrameworkPackageVersion">EntityFrameworkPackageVersion</a></td><td>
Gets or sets the value of EntityFrameworkPackageVersion domain property. Version of Entity Framework code for validation and generated code</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_EntityFrameworkVersion">EntityFrameworkVersion</a></td><td>
Gets or sets the value of EntityFrameworkVersion domain property. Version of Entity Framework for validation and generated code</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_EntityOutputDirectory">EntityOutputDirectory</a></td><td>
Gets or sets the value of EntityOutputDirectory domain property. Output directory for entities</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_EnumOutputDirectory">EnumOutputDirectory</a></td><td>
Gets or sets the value of EnumOutputDirectory domain property. Project directory for enums</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Enums">Enums</a></td><td>
Gets a list of Enums. No description available</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")![Static member](media/static.gif "Static member")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ExecuteValidator">ExecuteValidator</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_FileNameMarker">FileNameMarker</a></td><td>
Gets or sets the value of FileNameMarker domain property. File name suffix for generated files (e.g., foo.generated.cs)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_FullName">FullName</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_InheritanceStrategy">InheritanceStrategy</a></td><td>
Gets or sets the value of InheritanceStrategy domain property. How tables will be created/used to handle inheritance</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_InstallNuGetPackages">InstallNuGetPackages</a></td><td>
Gets or sets the value of InstallNuGetPackages domain property. On save, should the editor install/remove Entity Framework Nuget packages to match the model's EF version settings?</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_LayoutAlgorithm">LayoutAlgorithm</a></td><td>
Gets or sets the value of LayoutAlgorithm domain property. Strategy for auto-layout operations</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_LayoutAlgorithmSettings">LayoutAlgorithmSettings</a></td><td>
Gets or sets the value of LayoutAlgorithmSettings domain property. Tunable parameters for auto-layout of the designer</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_LazyLoadingEnabled">LazyLoadingEnabled</a></td><td>
Gets or sets the value of LazyLoadingEnabled domain property. If true, entity container will use lazy loading</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Namespace">Namespace</a></td><td>
Gets or sets the value of Namespace domain property. Namespace for all generated classes</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_NuGetPackageVersion">NuGetPackageVersion</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139912" target="_blank">Partition</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ProxyGenerationEnabled">ProxyGenerationEnabled</a></td><td>
Gets or sets the value of ProxyGenerationEnabled domain property. If true, context will generate proxies for POCO entities</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ShowCascadeDeletes">ShowCascadeDeletes</a></td><td>
Gets or sets the value of ShowCascadeDeletes domain property. If true, will display cascade deleted associations as dashed red lines</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ShowWarningsInDesigner">ShowWarningsInDesigner</a></td><td>
Gets or sets the value of ShowWarningsInDesigner domain property. If true, will display warning glyphs with tooltips when model elements have non-fatal issues detected</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_SpatialTypes">SpatialTypes</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb139916" target="_blank">Store</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_StructOutputDirectory">StructOutputDirectory</a></td><td>
Gets or sets the value of StructOutputDirectory domain property. Project directory for generated structures (owned/complex types)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Summary">Summary</a></td><td>
Gets or sets the value of Summary domain property. Brief code documentation</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_TransformOnSave">TransformOnSave</a></td><td>
Gets or sets the value of TransformOnSave domain property. If true, will run Transform All Templates when this model is saved</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_Types">Types</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ValidCLRTypes">ValidCLRTypes</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ValidIdentityAttributeTypes">ValidIdentityAttributeTypes</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_ValidTypes">ValidTypes</a></td><td /></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_Sawczyn_EFDesigner_EFModel_ModelRoot_WarnOnMissingDocumentation">WarnOnMissingDocumentation</a></td><td>
Gets or sets the value of WarnOnMissingDocumentation domain property. If true, will generate warnings when summary documentation is missing</td></tr></table>&nbsp;
<a href="#modelroot-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bsc2ak47" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/bb131174" target="_blank">GetDomainClass</a></td><td> (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/bb162926" target="_blank">ModelElement</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_ModelRoot_GetEntityFrameworkPackageVersionNum">GetEntityFrameworkPackageVersionNum</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/zdee4b3y" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/dfwy45w9" target="_blank">GetType</a></td><td>
Gets the <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_Sawczyn_EFDesigner_EFModel_ModelRoot_IsValidCLRType">IsValidCLRType</a></td><td /></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="http://msdn2.microsoft.com/en-us/library/7bxwbwt2" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#modelroot-class">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_Sawczyn_EFDesigner_EFModel_ModelRoot_LayoutAlgorithmDomainPropertyId">LayoutAlgorithmDomainPropertyId</a></td><td>
LayoutAlgorithm domain property Id.</td></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_Sawczyn_EFDesigner_EFModel_ModelRoot_LayoutAlgorithmSettingsDomainPropertyId">LayoutAlgorithmSettingsDomainPropertyId</a></td><td>
LayoutAlgorithmSettings domain property Id.</td></tr><tr><td>![Public field](media/pubfield.gif "Public field")![Static member](media/static.gif "Static member")</td><td><a href="F_Sawczyn_EFDesigner_EFModel_ModelRoot_PluralizationService">PluralizationService</a></td><td /></tr></table>&nbsp;
<a href="#modelroot-class">Back to Top</a>

## See Also


#### Reference
<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />