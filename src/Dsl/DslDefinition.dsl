<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="9987f227-3d05-49b7-b151-857879f5dfb8" Description="Entity Framework visual editor for EF6, EFCore and beyond." Name="EFModel" DisplayName="Entity Framework Visual Editor" Namespace="Sawczyn.EFDesigner.EFModel" MajorVersion="3" Revision="1" ProductName="EFDesigner" CompanyName="Michael Sawczyn" PackageGuid="56bbe1ba-aaee-4883-848f-e3c8656f8db2" PackageNamespace="Sawczyn.EFDesigner.EFModel" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="95532cb8-3452-4b09-a654-aeb2e2d0b3ad" Description="" Name="ModelRoot" DisplayName="Entity Model" Namespace="Sawczyn.EFDesigner.EFModel">
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="9c8d4478-0eec-40bf-b805-ce3b6ba9ea1a" Description="If true, entity container will use lazy loading" Name="LazyLoadingEnabled" DisplayName="Lazy Loading Enabled" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a75bed72-bf5e-47eb-bf7d-6a7d899f9d94" Description="Code visibility for entity container" Name="EntityContainerAccess" DisplayName="Entity Container Access" DefaultValue="Public" Category="Entity Context">
          <Type>
            <DomainEnumerationMoniker Name="ContainerAccess" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="881415e1-2291-493d-aad9-1abcb667b5cd" Description="Name of entity container class" Name="EntityContainerName" DisplayName="Entity Container Name" Category="Entity Context" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="259ca9ae-0772-41d4-90b5-abd10888e839" Description="Namespace for DBContext and, unless otherwise specified, all other generated code" Name="Namespace" DisplayName="DbContext Namespace" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5b68cf01-65fe-4270-b96f-a3e519b9bb98" Description="Initialization strategy to synchronize the underlying database when an instance of this context is used for the first time" Name="DatabaseInitializerType" DisplayName="Database Initializer Type" DefaultValue="MigrateDatabaseToLatestVersion" Category="Database">
          <Type>
            <DomainEnumerationMoniker Name="DatabaseInitializerKind" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4f313ccf-48d3-4a0c-a5bb-713c2e86df2e" Description="Connection string to use. Mutually exclusive with ConnectionStringName" Name="ConnectionString" DisplayName="Connection String" Category="Database">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Sawczyn.EFDesigner.EFModel.ConnectionStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c455eaf6-f38b-49de-b72e-477d73931dc1" Description="If true, automatic database migrations will be run when changes are detected." Name="AutomaticMigrationsEnabled" DisplayName="Automatic Migrations Enabled" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="368a7c8c-e0db-40ef-b678-dbc4580d9e54" Description="Output directory for entities" Name="EntityOutputDirectory" DisplayName="Output Directory - Entities" Category="Code Generation" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(ProjectDirectoryTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9e0ffa12-8733-40d2-8819-73740de7fecf" Description="Project directory for DbContext-related files" Name="ContextOutputDirectory" DisplayName="Output Directory - DbContext" Category="Code Generation" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(ProjectDirectoryTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5de798f2-310f-40f7-8188-37690e6691b5" Description="Project directory for enums" Name="EnumOutputDirectory" DisplayName="Output Directory - Enums" Category="Code Generation" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(ProjectDirectoryTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="02d27f11-3c30-4837-a56a-d63f0e629d95" Description="The schema to use for table creation" Name="DatabaseSchema" DisplayName="Database Schema Name" DefaultValue="dbo" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e4bc45f1-94fd-480e-a6f9-3cb22c7fd469" Description="Default concurrency handling strategy" Name="ConcurrencyDefault" DisplayName="Concurrency Default" DefaultValue="None" Category="Database">
          <Type>
            <DomainEnumerationMoniker Name="Concurrency" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3dc4c543-b044-44ca-af38-31cf29555403" Description="File name suffix for generated files (e.g., foo.generated.cs)" Name="FileNameMarker" DisplayName="File Name Marker" DefaultValue="generated" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b4e314ff-3c4b-4e1e-8309-8d35dacdc19e" Description="Version of Entity Framework for validation and generated code" Name="EntityFrameworkVersion" DisplayName="Entity Framework Version" DefaultValue="EF6" Category="Code Generation">
          <Type>
            <DomainEnumerationMoniker Name="EFVersion" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="effa53ce-9269-49ed-afb1-232905a85ec2" Description="Name of connection string in config file. Mutually exclusive with ConnectionString" Name="ConnectionStringName" DisplayName="Connection String Name" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c28462ae-e9b9-47e6-86c2-4efe0ab6e1c1" Description="Will chop generated code method chains" Name="ChopMethodChains" DisplayName="Chop Method Chains" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3ea27de3-f83c-44d8-bba5-c1886f65b182" Description="How tables will be created and used to handle inheritance" Name="InheritanceStrategy" DisplayName="Inheritance Strategy" DefaultValue="TablePerHierarchy" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.ReadOnly">
              <Parameters>
                <AttributeParameter Value="false" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(CodeStrategyTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="CodeStrategy" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b33a36f3-c01d-4e65-9946-17e68d78ea6c" Description="The type of container generated to represent associations if not overridden. Must implement ICollection&lt;T&gt;." Name="DefaultCollectionClass" DisplayName="Default Collection Class" DefaultValue="HashSet" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(CollectionTypeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c4dd4e17-94aa-44e7-8cb8-35f7c1a6d374" Description="If true, context will generate proxies for POCO entities" Name="ProxyGenerationEnabled" DisplayName="Proxy Generation Enabled" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0b0ccc1d-e8ea-4dab-9517-b7412cda307e" Description="If true, will trigger code generation when the file is saved. If false, code generation will have to be triggered manually." Name="TransformOnSave" DisplayName="Generate Code on Save" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="07688b4c-ba00-4e51-9abd-7353a4df233d" Description="Default type for ID properties in new classes" Name="DefaultIdentityType" DisplayName="Default Identity Type" DefaultValue="Int64" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(IdentityAttributeTypeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="267e37bd-159f-4b8b-88c3-f1561bf576e0" Description="If true, will display cascade deleted associations as dashed red lines" Name="ShowCascadeDeletes" DisplayName="Show Cascade Deletes" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="35be1c55-6c42-45bf-af62-16d00dbf80c4" Description="Database manifest token. Optimization if runtime database type is known and unchanging, and only valid for SqlServer." Name="DatabaseType" DisplayName="SqlServer Type">
          <Type>
            <DomainEnumerationMoniker Name="DatabaseKind" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="247aa399-3d89-4b48-baeb-992e8cb3d47a" Description="If true, will generate warnings when summary documentation is missing" Name="WarnOnMissingDocumentation" DisplayName="Warn On Missing Documentation" DefaultValue="true" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2a16f8e1-9e68-43ce-b625-5e68e8497abb" Description="Version of Entity Framework code for validation and generated code" Name="EntityFrameworkPackageVersion" DisplayName="Entity Framework Package Version" DefaultValue="Latest" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(EFPackageVersionTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="35350509-1856-4ca7-884a-1cee2552eef9" Description="Project directory for generated structures (owned/complex types)" Name="StructOutputDirectory" DisplayName="Output Directory - Structs" Category="Code Generation" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(ProjectDirectoryTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="950f6c9e-3977-4968-aa8f-697ffb49ac4a" Description="Code visibility for DbSets" Name="DbSetAccess" DisplayName="DbSet Access" DefaultValue="Public" Category="Entity Context">
          <Type>
            <DomainEnumerationMoniker Name="ContainerAccess" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a95a5645-b5d5-4b11-b177-d674cee91290" Description="Detailed code documentation" Name="Description" DisplayName="Comment Detail" DefaultValue="" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a25a2ffe-9ae9-408b-a346-056805de6975" Description="Brief code documentation" Name="Summary" DisplayName="Comment Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b79884bd-572a-444c-b64e-24d66c8efc29" Description="If true, will display warning glyphs with tooltips when model elements have non-fatal issues detected" Name="ShowWarningsInDesigner" DisplayName="Show Warnings In Designer" DefaultValue="true" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f11b0472-3a86-497c-b8a7-cd90350fe289" Description="Namespace for entities" Name="EntityNamespace" DisplayName="Entity Namespace" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f7d6b698-b82a-4438-bf49-c88c9c0299c7" Description="Namespace for enumerations" Name="EnumNamespace" DisplayName="Enum Namespace" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c3e26d6b-3032-4cb2-aac3-057634aa4096" Description="Namespace for owned entities" Name="StructNamespace" DisplayName="Struct Namespace" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="441a9ed6-a2d6-48a2-8d49-136afdeac9fc" Description="Default namespaces for generated code" Name="Namespaces" DisplayName="Namespaces" Kind="CustomStorage" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.ExpandableObjectConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="Namespaces" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ad922a6e-ebe8-4cf7-ae7d-b044101068da" Description="Default output folders for generated code" Name="OutputLocations" DisplayName="Output Locations" Kind="CustomStorage" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.ExpandableObjectConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="OutputLocations" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6f74414d-8d1d-4776-9e5e-76087d2be937" Description="Allow foreign key properties to be available in the POCO entities (recommended: false)" Name="ExposeForeignKeys" DisplayName="Allow Modeled Foreign Keys" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7b3fbfb0-9b8c-4ea6-af94-198df57f15e6" Description="Base class for DbContext in generated code. Should be fully qualified if not in a standard namespace." Name="BaseClass" DisplayName="Context Base Class" DefaultValue="DbContext" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e822ea02-b26c-4105-adf4-bf8041068f35" Description="If true, displays a grid for aligning shapes" Name="ShowGrid" DisplayName="Show Grid" DefaultValue="false" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6d82895b-7648-4c95-a0b8-f9f796e34003" Description="If true, shapes snap to the designer's grid" Name="SnapToGrid" DisplayName="Snap To Grid" DefaultValue="false" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="274ed266-5104-4319-b3de-0070508cc383" Description="Color for designer grid" Name="GridColor" DisplayName="Grid Color" DefaultValue="" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e4121eca-2a99-4d3c-8159-1784fdb72d28" Description="Size of display grid units, in inches" Name="GridSize" DisplayName="Grid Size" DefaultValue="0" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Int16" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1b587ac8-01a1-4758-bdec-42e1086e7622" Description="If true, will show declared foreign key property names (if any) on the association ends" Name="ShowForeignKeyPropertyNames" DisplayName="Show Foreign Key Property Names" DefaultValue="true" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a7b8458b-ade6-4392-859b-ae2dc017c98f" Description="The default collation for database storage." Name="DatabaseCollationDefault" DisplayName="Database Collation Default" DefaultValue="default" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="51108d30-911e-496c-9b41-99d297f287c3" Description="Default property access mode for backing fields" Name="PropertyAccessModeDefault" DisplayName="Property Access Mode Default" Category="Code Generation">
          <Type>
            <DomainEnumerationMoniker Name="PropertyAccessMode" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="56d5d7e6-c806-422f-b906-f12e2b6d89fc" Description="If true, will generate code with tabs instead of spaces for indentation" Name="UseTabs" DisplayName="Use Tabs" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Comment" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelRootHasComments.Comments</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelEnum" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelRootHasEnums.Enums</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelClass" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelRootHasClasses.Classes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelDiagramData" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelRootHasModelDiagrams.Diagrams</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="237aa10f-ae74-41be-bdcf-56d97de6e4c8" Description="" Name="ModelClass" DisplayName="Entity" Namespace="Sawczyn.EFDesigner.EFModel">
      <BaseClass>
        <DomainClassMoniker Name="DesignElement" />
      </BaseClass>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="40e0f86d-e0a7-4e3b-8f83-817fcdfc3ad5" Description="If true, this will be generated as an abstract class" Name="IsAbstract" DisplayName="Abstract" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a674bf44-2df3-44dc-a954-5122556ce4ac" Description="Name of the database table storing data for this class" Name="TableName" DisplayName="Table Name" DefaultValue="" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="03075c04-06b6-4b97-a78a-b3be2c9526d7" Description="The schema to use for table creation. Overrides default schema for model if present." Name="DatabaseSchema" DisplayName="Database Schema" DefaultValue="" Kind="CustomStorage" Category="Database" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="156da044-640c-466a-898d-73114c0f54e8" Description="Overridden concurrency handling strategy." Name="Concurrency" DisplayName="Concurrency" DefaultValue="Default" Category="Code Generation">
          <Type>
            <DomainEnumerationMoniker Name="ConcurrencyOverride" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d1bf6501-cc7e-4994-848f-6dbdcbf21900" Description="If true, ModelClass.DatabaseSchema tracks ModelRoot.DatabaseSchema" Name="IsDatabaseSchemaTracking" DisplayName="Is Database Schema Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="60009c33-0083-41d8-bd9a-159d9d9c5b5c" Description="Overrides default namespace" Name="Namespace" DisplayName="Namespace" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a65b337d-e60f-427e-9c30-bd39e95b8d72" Description="If true, ModelClass.Namespace tracks ModelRoot.Namespace" Name="IsNamespaceTracking" DisplayName="Is Namespace Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bbdbf0f5-65fa-405f-bc5b-36fe3b43c6e4" Description="Name of this class's container variable in the generated context" Name="DbSetName" DisplayName="DbSet Name" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1f3ff088-f688-4573-b2c5-a8b2b64ec10c" Description="" Name="Name" DisplayName="Name" DefaultValue="" Category="Code Generation" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
          <ElementNameProvider>
            <ExternalTypeMoniker Name="ModelClassNameProvider" />
          </ElementNameProvider>
        </DomainProperty>
        <DomainProperty Id="268b5655-43ae-4871-b7e7-2ea7e003d485" Description="Should this class's properties implement INotifyPropertyChanged by default?" Name="ImplementNotify" DisplayName="Implement INotifyPropertyChanged" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="55789969-4fcd-4e41-bf8a-69cf0dd70265" Description="Optional comma-separated list of interfaces that will be added to the class definition" Name="CustomInterfaces" DisplayName="Custom Interfaces" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="21a0cf45-971e-40dd-8940-afafa5985d7f" Description="Is this a completely dependent type that will only exist associated to another object?" Name="IsDependentType" DisplayName="Is Dependent Type" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7b385981-572c-4f6f-81aa-5de8608c3914" Description="Overrides default output directory" Name="OutputDirectory" DisplayName="Output Directory" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8045d89c-6c9a-4775-92bc-841a7f6fe7d3" Description="If true, ModelClass.OutputDirectory tracks ModelRoot.EntityOutputDirectory" Name="IsOutputDirectoryTracking" DisplayName="Is Output Directory Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="90778922-6b74-40a8-a9ef-14871a788d5e" Description="Type of glyph to show on the design surface" Name="GlyphType" DisplayName="Glyph Type" Kind="Calculated" GetterAccessModifier="Assembly" SetterAccessModifier="Assembly" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4b0b6237-2bfc-4be6-a088-271fe34b89ec" Description="Detailed code documentation" Name="Description" DisplayName="Comment Detail" DefaultValue="" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5ff45bb4-658f-412e-8cdc-2593d9ea3d37" Description="Brief code documentation" Name="Summary" DisplayName="Comment Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ed7ce317-4ceb-479e-98d0-d7fd9eb858f0" Description="Exposes Superclass property in property editor" Name="BaseClass" DisplayName="Base Class" Kind="CustomStorage" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(BaseClassTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cae77164-7b85-4c67-8fa4-88f945353e92" Description="Any custom attributes to be generated for this element.  Will be passed through as entered." Name="CustomAttributes" DisplayName="Custom Attributes" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="aae9a58c-df8c-4557-826a-f0a66bb75d66" Description="Default value for this class's attribute AutoProperty setting" Name="AutoPropertyDefault" DisplayName="AutoProperty Default" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="84329360-6f27-42ec-99fe-3b5bff1dc54c" Description="If true (the default), code will be generated for this class. If false, it is assumed to be referenced from another assembly." Name="GenerateCode" DisplayName="Generate Code" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6d12879a-6e9d-4a6f-9e4b-5cf2847a74f3" Description="If true, this class will be implemented as Dictionary&lt;string, object&gt;" Name="IsPropertyBag" DisplayName="Is Property Bag" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9b6ae5c9-fe3f-4b3c-9a76-291d58fde1eb" Description="If true, this class is defined by a SQL query that must be implemented in a partial method. See comments in generated code for more information." Name="IsQueryType" DisplayName="Is Query Type" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="37b0398e-caf1-45d7-acf4-8a44ddcb8543" Description="If true, this class won't be involved in database migrations" Name="ExcludeFromMigrations" DisplayName="Exclude From Migrations" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2d01c28f-20f9-4e44-aaf6-27b5d1541b5f" Description="If true, this class is persisted in a view in the database" Name="IsDatabaseView" DisplayName="Is Database View" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0b8d7bf1-3927-40e6-b0aa-81a8b7dddacb" Description="The name of the database view persisting this class" Name="ViewName" DisplayName="View Name" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelAttribute" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ClassHasAttributes.Attributes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Comment" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CommentReferencesClasses.Comments</DomainPath>
            <DomainPath>ModelRootHasClasses.ModelRoot/!ModelRoot/ModelRootHasComments.Comments</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="8be1f7ab-85c8-4f57-8621-38e1207d8f8d" Description="An attribute of a class." Name="ModelAttribute" DisplayName="Property" Namespace="Sawczyn.EFDesigner.EFModel">
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="7ccdef4e-9305-485f-abdc-7ee9afed3b83" Description="Type of this attribute" Name="Type" DisplayName="Type" DefaultValue="String" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(AttributeTypeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="711a1ade-6ac9-4f06-be35-3f7d0aacd848" Description="Value for attribute when first created" Name="InitialValue" DisplayName="Initial Value" DefaultValue="" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ef3764a4-c46e-443d-86b6-968e828d6d3b" Description="Does this attribute represent the identity of the object?" Name="IsIdentity" DisplayName="Is Identity" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e3b492a9-5341-4694-afa5-cb566ae813a7" Description="If true, cannot be null (or the default value for the property type, if not nullable)" Name="Required" DisplayName="Required" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e19260c6-5ae0-43bc-82eb-dd2b4d234f9e" Description="Attribute will be stored in persistent storage" Name="Persistent" DisplayName="Persistent" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8c128f2b-8f9f-4c8e-acf1-dd5488736b79" Description="Maximum length of the string, A positive number will generate a length constraint, negative number means the database maximum (approx. 2GB), and null or 0 for no max length entry (migration traditionally will set the database column to 2000 characters)." Name="MaxLength" DisplayName="Max Length" DefaultValue="" Category="String Properties">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(MaxLengthTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Nullable&lt;System.Int32&gt;" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="59213f5b-9662-4f70-ade1-280d2d7ab77b" Description="Should this attribute create an index in the database?" Name="Indexed" DisplayName="Indexed" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a4f65b33-e958-4f04-9dd0-160ce72a9f5c" Description="If indexed, is the index a unique index?" Name="IndexedUnique" DisplayName="Indexed Unique" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b129be88-1030-4ef1-9ba2-98a02aa40d2b" Description="If not empty, will create a standard validation annotation for this attribute." Name="StringType" DisplayName="String Type" Category="String Properties">
          <Type>
            <DomainEnumerationMoniker Name="HTML5Type" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5e502e9e-7120-42d1-bd3b-27dfc42f3618" Description="Unused - Alternate table to store this attribute. If empty, the class's table is used." Name="TableOverride" DisplayName="Table Override" Category="Database" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="875fc44f-1355-4b9e-b3bb-377ab3510f3b" Description="If true, this property serves as the concurrency value for the class" Name="IsConcurrencyToken" DisplayName="Is Concurrency Token" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e52b2355-7c6c-4493-8012-291f4f679d7b" Description="If this property is an identity, how the value is generated" Name="IdentityType" DisplayName="Identity Type" DefaultValue="None" Category="Database">
          <Type>
            <DomainEnumerationMoniker Name="IdentityType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d974a312-57fe-42b3-b377-22fe01562356" Description="Detailed code documentation" Name="Description" DisplayName="Comment Detail" DefaultValue="" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1ebd74c3-5011-48e6-b5c1-67d78293067d" Description="Brief code documentation" Name="Summary" DisplayName="Comment Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="df236d99-ef7c-419c-833d-a972bf8ccc52" Description="The name of the property. Must be a valid C# symbol." Name="Name" DisplayName="Name" DefaultValue="" Category="Code Generation" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
          <ElementNameProvider>
            <ExternalTypeMoniker Name="ModelAttributeNameProvider" />
          </ElementNameProvider>
        </DomainProperty>
        <DomainProperty Id="76cc7aca-4420-423c-891c-2d349c8b7bc4" Description="Visibility for property setter; getter is public" Name="SetterVisibility" DisplayName="Setter Visibility" DefaultValue="Public" Category="Code Generation">
          <Type>
            <DomainEnumerationMoniker Name="SetterAccessModifier" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c416dac0-92c6-4b36-89cd-b76380a3c448" Description="If true, property will be generated with the 'virtual' keyword" Name="Virtual" DisplayName="Virtual" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d20e1491-e58d-4a3c-8c0a-24653cc38580" Description="If true, no setter will be generated. Only valid for transient public properties." Name="ReadOnly" DisplayName="Read Only" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bd0273dc-d3ea-44e2-8b01-f79d39ca0704" Description="If false, generates a backing field and a partial method to hook getting and setting the property. If true, generates a simple auto property." Name="AutoProperty" DisplayName="Auto Property" DefaultValue="" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7fe42ef4-9691-4db1-8219-59107c14478b" Description="Minimum length of the string, 0 for no minimum length" Name="MinLength" DisplayName="Min Length" DefaultValue="" Category="String Properties">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(MinLengthTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e4394dde-038f-4fea-a328-3b6bed8571f8" Description="The name for the table column backing this property" Name="ColumnName" DisplayName="Column Name" Kind="CustomStorage" Category="Database" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7e3b91ab-3859-4b80-9270-9adf7b46dbb3" Description="If true, ModelAttribute.ColumnName tracks ModelAttribute.Name" Name="IsColumnNameTracking" DisplayName="Is Column Name Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="17cc5012-1352-4a08-9965-55dcecaa985f" Description="The data type for the table column backing this property" Name="ColumnType" DisplayName="Column Type" Kind="CustomStorage" Category="Database" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8f4f2c30-d7cd-4ee6-aa6b-1bc1dc8fd13c" Description="If true, ModelAttribute.ColumnType tracks ModelAttribute.Type" Name="IsColumnTypeTracking" DisplayName="Is Column Type Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c54c284f-10d6-4a49-8fc1-0bcf4bab8c6f" Description="Any custom attributes to be generated for this element.  Will be passed through as entered." Name="CustomAttributes" DisplayName="Custom Attributes" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7d65b45d-ff09-49c7-b1bc-ea80175686d8" Description="Text for [Display(Name=&quot;&lt;text&gt;&quot;)] attribute" Name="DisplayText" DisplayName="Display Text" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="dfb9a776-9fda-4565-8f78-bcac2a6fb734" Description="Should this class implement INotifyPropertyChanged?" Name="ImplementNotify" DisplayName="Implement INotifyPropertyChanged" DefaultValue="" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="51e44c7a-7fbc-4ddc-ae59-97ea5519baa7" Description="If true, ModelAttribute.IsImplementNotify tracks ModelClass.ImplementNotify" Name="IsImplementNotifyTracking" DisplayName="Is Implement Notify Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="79e4dae8-e284-44a2-b7b2-6aaae8c0239d" Description="If true, ModelAttribute.AutoProperty tracks ModelClass.AutoPropertyDefault" Name="IsAutoPropertyTracking" DisplayName="Is Auto Property Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8282d835-2c0e-4d59-a638-6d3c6e494260" Description="If present, this attribute is a foreign key for the association named by this Guid" Name="IsForeignKeyFor" DisplayName="Is Foreign Key For" GetterAccessModifier="Assembly" SetterAccessModifier="Assembly" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ce0d2639-3b83-4ce7-820e-43456412da05" Description="The name of the backing field for this property" Name="BackingFieldName" DisplayName="Backing Field" Kind="CustomStorage" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="349c587e-0b87-42a7-ac99-ef9648a70325" Description="Overrides the default database collation setting for the column that persists this attribute" Name="DatabaseCollation" DisplayName="Database Collation" DefaultValue="" Kind="CustomStorage" Category="Database" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cd11a7f7-0432-4d16-98d8-5db6a98ffc8b" Description="If true, ModelAttribute.DatabaseCollation tracks ModelRoot.DatabaseCollationDefault" Name="IsDatabaseCollationTracking" DisplayName="Is Database Collation Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fcbce627-878c-468f-84a5-b0e5deedacb0" Description="Defines how EF reads and write this property or its backing field. See  https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.propertyaccessmode" Name="PropertyAccessMode" DisplayName="Property Access Mode" DefaultValue="FieldDuringConstruction" Category="Code Generation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(PropertyAccessModeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="PropertyAccessMode" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="e2c13b26-0944-4b6c-89b5-bb95c500f515" Description="" Name="Comment" DisplayName="Comment" Namespace="Sawczyn.EFDesigner.EFModel">
      <Properties>
        <DomainProperty Id="8c3255f6-3ee8-40e1-b433-0719aaffce04" Description="Comment text" Name="Text" DisplayName="Text" DefaultValue="">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a17c8f41-4a5b-40a1-9f77-71346c428207" Description="Truncated Text property for Explorer display" Name="ShortText" DisplayName="Short Text" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="9c7f55aa-1cc9-4841-b671-0cab31164a24" Description="Represents an enumeration in C#" Name="ModelEnum" DisplayName="Enum" Namespace="Sawczyn.EFDesigner.EFModel">
      <BaseClass>
        <DomainClassMoniker Name="DesignElement" />
      </BaseClass>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="123f14ef-96ff-4a05-9440-888c43c44e36" Description="CLR type implementing this enum (usually Int32)" Name="ValueType" DisplayName="Value Type" DefaultValue="Int32" Category="Code Generation">
          <Type>
            <DomainEnumerationMoniker Name="EnumValueType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="14138023-9e10-4ffd-848b-d77ce3de3cfc" Description="Overrides default namespace" Name="Namespace" DisplayName="Namespace" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4f7323ec-9053-4127-a415-9e9857f6df68" Description="If true, ModelEnum.Namespace tracks ModelRoot.Namespace" Name="IsNamespaceTracking" DisplayName="Is Namespace Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0d249431-c352-4468-8322-eb5910477fee" Description="Name of the enumeration" Name="Name" DisplayName="Name" DefaultValue="" Category="Code Generation" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
          <ElementNameProvider>
            <ExternalTypeMoniker Name="ModelEnumNameProvider" />
          </ElementNameProvider>
        </DomainProperty>
        <DomainProperty Id="2196162d-2f05-48dd-9ae6-d293190c9c77" Description="If true, values in this enumeration are flags and will have initial values set appropriately." Name="IsFlags" DisplayName="Values are Flags" DefaultValue="false" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6556f5a3-cba4-415f-bd8c-1af786b61421" Description="Overrides default output directory" Name="OutputDirectory" DisplayName="Output Directory" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1a498d1a-15d7-48fe-b911-17b1028da5f7" Description="If true, ModelEnum.OutputDirectory tracks ModelRoot.EnumOutputDirectory" Name="IsOutputDirectoryTracking" DisplayName="Is Output Directory Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b643f6b2-d807-4318-a291-424b74893f3c" Description="Type of glyph to show on the design surface" Name="GlyphType" DisplayName="Glyph Type" Kind="Calculated" GetterAccessModifier="Assembly" SetterAccessModifier="Assembly" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e0146bec-7ec5-4bba-ad7b-6c305a421c0c" Description="Detailed code documentation" Name="Description" DisplayName="Comment Detail" DefaultValue="" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="12ea7154-be17-4707-ac96-eb1f8f2029ce" Description="Brief code documentation" Name="Summary" DisplayName="Comment Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d4b2ab6e-54f5-4b70-94b0-403025b01541" Description="Any custom attributes to be generated for this element. Will be passed through as entered." Name="CustomAttributes" DisplayName="Custom Attributes" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8ebaa1e0-1528-478d-aa32-0a2cedd89254" Description="If true (the default), code will be generated for this enum. If false, it is assumed to be referenced from another assembly." Name="GenerateCode" DisplayName="Generate Code" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelEnumValue" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModelEnumHasValues.Values</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Comment" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CommentReferencesEnums.Comments</DomainPath>
            <DomainPath>ModelRootHasEnums.ModelRoot/!ModelRoot/ModelRootHasComments.Comments</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="89938de9-60f8-472a-9507-f7c7de18a511" Description="Represents a value in a C# enumeration" Name="ModelEnumValue" DisplayName="Value" Namespace="Sawczyn.EFDesigner.EFModel">
      <Properties>
        <DomainProperty Id="f67322ba-10ef-44d8-bd5f-b54955cb70ff" Description="" Name="Name" DisplayName="Name" DefaultValue="" Category="Code Generation" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="23fd110b-aff0-4abd-87f6-c38ab7ba3f19" Description="Optional value for this enum element. Must be in the range of the enum's ValueType" Name="Value" DisplayName="Value" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="295765ec-8896-4a75-b4f6-a9b5f6c663cd" Description="Detailed code documentation" Name="Description" DisplayName="Comment Detail" DefaultValue="" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4eec932a-9148-4d3c-9f29-35d8dbd13844" Description="Brief code documentation" Name="Summary" DisplayName="Comment Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f01f16bc-6d9b-4b19-ad24-182b32379961" Description="Any custom attributes to be generated for this element. Will be passed through as entered." Name="CustomAttributes" DisplayName="Custom Attributes" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c776a259-4e87-42d3-ade8-a6170087c11b" Description="Text for [Display(Name=&quot;&lt;text&gt;&quot;)] attribute" Name="DisplayText" DisplayName="Display Text" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="460f3d41-02c2-41dd-8fd3-8286e400e6f1" Description="Description for Sawczyn.EFDesigner.EFModel.DesignElement" Name="DesignElement" DisplayName="Design Element" InheritanceModifier="Abstract" Namespace="Sawczyn.EFDesigner.EFModel" />
    <DomainClass Id="9698525f-c2b0-4283-99f4-6660e10fa082" Description="Represents a viewable model diagram" Name="ModelDiagramData" DisplayName="Diagram" Namespace="Sawczyn.EFDesigner.EFModel">
      <Properties>
        <DomainProperty Id="202e04fe-de2a-4030-9e96-0a7e9df613ab" Description="Diagram name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
          <ElementNameProvider>
            <ExternalTypeMoniker Name="ModelDiagramDataNameProvider" />
          </ElementNameProvider>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="ce77f831-a92d-4274-823a-3a8441a65f3a" Description="Associations between Classes." Name="Association" DisplayName="Association" InheritanceModifier="Abstract" Namespace="Sawczyn.EFDesigner.EFModel" AllowsDuplicates="true">
      <Notes>This is the abstract base relationship of the several kinds of association between Classes.
      It defines the Properties that are attached to each association.</Notes>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="50d076b7-4a3e-4b87-b5bb-8537520adc72" Description="The allowed number of entities at this end of the association" Name="SourceMultiplicity" DisplayName="End1 Multiplicity" DefaultValue="One" Category="End 1">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(SourceMultiplicityTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="Multiplicity" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b84f185b-4eea-4454-a47d-26b1a4b09523" Description="The allowed number of entities at this end of the association" Name="TargetMultiplicity" DisplayName="End2 Multiplicity" DefaultValue="ZeroMany" Category="End 2">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(TargetMultiplicityTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="Multiplicity" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="81625766-4885-46ba-a535-c3e23a7c5f88" Description="Name of the entity property that returns the value at this end" Name="TargetPropertyName" DisplayName="End1 Navigation Property" DefaultValue="" Category="End 2">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8c8b1118-d3f2-494a-b18e-8dbe26212910" Description="If false, this is a transient association not stored in the database but instead created in code" Name="Persistent" DisplayName="Persistent" DefaultValue="true" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0346043b-1ed8-4299-b84a-57167b4c00db" Description="" Name="SourceMultiplicityDisplay" DisplayName="Source Multiplicity Display" Kind="Calculated" Category="End 1" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="450de663-cfb6-48e3-888a-4fbcee3ac778" Description="Decorator text" Name="TargetMultiplicityDisplay" DisplayName="Target Multiplicity Display" Kind="Calculated" Category="End 2" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f40a8fc6-0b1b-4c1b-a46c-75d3450cd6c8" Description="The action to take when an entity on this end is deleted." Name="SourceDeleteAction" DisplayName="End1 On Delete" DefaultValue="Default" Category="End 1">
          <Type>
            <DomainEnumerationMoniker Name="DeleteAction" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6e502a47-428b-455f-b155-edf310ce6c73" Description="The action to take when an entity on this end is deleted." Name="TargetDeleteAction" DisplayName="End2 On Delete" DefaultValue="Default" Category="End 2">
          <Type>
            <DomainEnumerationMoniker Name="DeleteAction" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="59735c14-84b9-4b9c-b449-2248cb67d65a" Description="Class used to instanciate association properties. Implements ICollection&lt;&gt;" Name="CollectionClass" DisplayName="Collection Class" DefaultValue="" Kind="CustomStorage" Category="Code Generation" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(CollectionTypeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8983d9c4-c5f3-4eaa-b8b2-14d18a858f21" Description="Detailed code documentation for this end of the association" Name="TargetDescription" DisplayName="End2 Comment Detail" DefaultValue="" Category="End 2">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="edec72cc-c40b-4c64-b2d4-e713f691ecd0" Description="Short code documentation for this end of the association" Name="TargetSummary" DisplayName="End2 Comment Summary" Category="End 2">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="53747127-cd19-43e0-b37b-1b669d506ed2" Description="Brief code documentation" Name="Summary" DisplayName="Comment Summary" Category="Documentation" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e73f6b6d-22b1-4dd7-8d8d-37b7f1cac4a0" Description="If true, Association.CollectionClass tracks ModelRoot.DefaultCollectionClass" Name="IsCollectionClassTracking" DisplayName="Is Collection Class Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8f74b05b-3bb2-480b-9513-50efd91588ec" Description="Which object(s) in this association is/are the principal object(s)?" Name="SourceRole" DisplayName="End1 Role" DefaultValue="NotSet" Category="End 1">
          <Type>
            <DomainEnumerationMoniker Name="EndpointRole" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2eb18350-1736-4c15-84c4-219119a56a2a" Description="Which object(s) in this association is/are the dependent object(s)?" Name="TargetRole" DisplayName="End2 Role" DefaultValue="NotSet" Category="End 2">
          <Type>
            <DomainEnumerationMoniker Name="EndpointRole" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c0b9ec69-21ba-432e-a8e9-3afa83f8b2b7" Description="Which class should hold the foreign key for this relationship" Name="ForeignKeyLocation" DisplayName="Foreign Key Location" SetterAccessModifier="Assembly" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <DomainEnumerationMoniker Name="ForeignKeyOwner" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a667dd36-ac5c-4c98-b368-b84778bdcd56" Description="Any custom attributes to be generated for the target property. Will be passed through as entered." Name="TargetCustomAttributes" DisplayName="End2 Custom Attributes" Category="End 2">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4f71c60f-e2d6-475c-b545-1aa18d85d5ab" Description="Text for [Display(Name=&quot;&lt;text&gt;&quot;)] attribute on this end's property" Name="TargetDisplayText" DisplayName="End2 Display Text" Category="End 2">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="637b64d2-193d-47f9-b63f-df8ccc23f900" Description="Should this end participate in INotifyPropertyChanged activities? Only valid for non-collection targets." Name="TargetImplementNotify" DisplayName="End2 Implement INotifyPropertyChanged" Kind="CustomStorage" Category="End 2" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="30a6413f-ab71-4297-9d37-e53f0eda887f" Description="Description for Sawczyn.EFDesigner.EFModel.Association.Is Target Implement Notify Tracking" Name="IsTargetImplementNotifyTracking" DisplayName="Is Target Implement Notify Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1d5c6e0b-4942-4504-971d-9301be5e07f1" Description="Name of property holding foreign key value for this association" Name="FKPropertyName" DisplayName="Foreign Key Property" Category="Code Generation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0b56420b-5f48-4e04-9288-6d6a6f12ce9f" Description="Decorator text" Name="TargetPropertyNameDisplay" DisplayName="Target Property Name Display" Kind="Calculated" Category="End2" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="21f7d7e2-e0bc-4d59-b776-55a567c16cb0" Description="Solely for display in the object list of the VStudio property window" Name="Name" DisplayName="Name" Kind="Calculated" SetterAccessModifier="Private" IsElementName="true" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="37a67510-4e09-4d8b-a0b1-a3401b2a5f15" Description="Optional name of the database table used to join the two end classes for many-to-many associations. If empty, a reasonable default name will be used." Name="JoinTableName" DisplayName="Join Table Name" DefaultValue="" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a922ed78-450a-42bd-87f9-c355a432a67f" Description="The name of the backing field for this property" Name="TargetBackingFieldName" DisplayName="End1 Backing Field" Kind="CustomStorage" Category="End 2">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9110fead-38f5-46b5-b3b1-e9c3c6cea99b" Description="Defines how EF reads and write this property or its backing field. See  https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.propertyaccessmode" Name="TargetPropertyAccessMode" DisplayName="End1 Property Access Mode" DefaultValue="FieldDuringConstruction" Category="End 2">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(PropertyAccessModeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="PropertyAccessMode" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6b0a49d6-1220-4c69-a7d4-35fb2469bd3e" Description="If false, generates a backing field and a partial method to hook getting and setting the property. If true, generates a simple auto property. Only valid for non-collection properties." Name="TargetAutoProperty" DisplayName="End1 Is Auto Property" DefaultValue="" Kind="CustomStorage" Category="End 2" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="aa5ed789-f003-420c-9496-df031f1b21b6" Description="If true, Association.TargetAutoProperty tracks ModelClass.AutoPropertyDefault" Name="IsTargetAutoPropertyTracking" DisplayName="Is Target Auto Property Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="d2edf927-64c2-4fe3-8d4e-c44e87142c4c" Description="" Name="Source" DisplayName="Source" PropertyName="Targets" PropertyDisplayName="Targets">
          <Notes>The Targets property on a ModelClass will include all the elements targeted by every kind of Association.</Notes>
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="1a39c29f-8df6-4063-bf60-cfe2c0c619fa" Description="" Name="Target" DisplayName="Target" PropertyName="Sources" PropertyDisplayName="Sources">
          <Notes>The Sources property on a ModelClass will include all the elements sourced by every kind of Association.</Notes>
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="71aa98ea-ecd0-4096-b185-5c63efa364eb" Description="" Name="UnidirectionalAssociation" DisplayName="Unidirectional Association" Namespace="Sawczyn.EFDesigner.EFModel" AllowsDuplicates="true">
      <BaseRelationship>
        <DomainRelationshipMoniker Name="Association" />
      </BaseRelationship>
      <Source>
        <DomainRole Id="2ece6e4a-4505-4e70-8f12-59525acae945" Description="" Name="UnidirectionalSource" DisplayName="Unidirectional Source" PropertyName="UnidirectionalTargets" PropertyDisplayName="Unidirectional Targets">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="0cd8b649-09aa-4c6b-9e7a-95088c246e5f" Description="" Name="UnidirectionalTarget" DisplayName="Unidirectional Target" PropertyName="UnidirectionalSources" PropertyDisplayName="Unidirectional Sources">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="75261e55-91dc-47d5-aa17-0eceb29660b5" Description="" Name="ClassHasAttributes" DisplayName="Class Has Attributes" Namespace="Sawczyn.EFDesigner.EFModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="97fe55a1-616b-4c9e-89be-2a8e41e8ebaa" Description="" Name="ModelClass" DisplayName="Model Class" PropertyName="Attributes" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Attributes">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="970ec7f5-c880-454c-ab1d-d5706315c530" Description="" Name="Attribute" DisplayName="Attribute" PropertyName="ModelClass" Multiplicity="ZeroOne" PropagatesDelete="true" PropertyDisplayName="Model Class">
          <RolePlayer>
            <DomainClassMoniker Name="ModelAttribute" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="f531650c-f0d4-47a2-be7c-c3a564194629" Description="" Name="ModelRootHasComments" DisplayName="Model Root Has Comments" Namespace="Sawczyn.EFDesigner.EFModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="ab8f00b5-d976-4ffa-b9da-be285acbbe91" Description="" Name="ModelRoot" DisplayName="Model Root" PropertyName="Comments" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Comments">
          <RolePlayer>
            <DomainClassMoniker Name="ModelRoot" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="e13a639c-a993-4cf5-9c8d-aca52b7aeab6" Description="" Name="Comment" DisplayName="Comment" PropertyName="ModelRoot" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Model Root">
          <RolePlayer>
            <DomainClassMoniker Name="Comment" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c6eff342-0a73-4d2f-aa0e-b2811663fb60" Description="Inheritance between Classes." Name="Generalization" DisplayName="Inheritance" Namespace="Sawczyn.EFDesigner.EFModel">
      <Properties>
        <DomainProperty Id="76e2afe8-4124-448f-83e1-b6bef1d1e9d7" Description="Solely for display in the object list of the VStudio property window" Name="Name" DisplayName="Name" Kind="Calculated" SetterAccessModifier="Private" IsElementName="true" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="6df2e060-abe4-4aae-a9e7-ae6fb38ca1c3" Description="" Name="Superclass" DisplayName="Base Type" PropertyName="Subclasses" PropertyDisplayName="Subclasses">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="d805f6db-aa0c-42e0-b41d-a4ff898c6404" Description="" Name="Subclass" DisplayName="Subclass" PropertyName="Superclass" Multiplicity="ZeroOne" IsPropertyBrowsable="false" PropertyDisplayName="Superclass">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="73a40ac4-1b7a-4b4a-8099-3783fabcca5b" Description="" Name="BidirectionalAssociation" DisplayName="Bidirectional Association" Namespace="Sawczyn.EFDesigner.EFModel" AllowsDuplicates="true">
      <BaseRelationship>
        <DomainRelationshipMoniker Name="Association" />
      </BaseRelationship>
      <Properties>
        <DomainProperty Id="1e0e43de-1ed5-42e9-9c81-8fee8d85b4cf" Description="Name of the entity property that returns the value at this end" Name="SourcePropertyName" DisplayName="End2 Navigation Property" DefaultValue="" Category="End 1">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="378e5c5a-9eb0-4d42-ad4c-7feca0176995" Description="Detailed code documentation for this end of the association" Name="SourceDescription" DisplayName="End1 Comment Detail" DefaultValue="" Category="End 1">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="73bbc365-57ca-4f07-a834-9fe6605b76d0" Description="Short code documentation for this end of the association" Name="SourceSummary" DisplayName="End1 Comment Summary" Category="End 1">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="124bb49e-c952-4a7f-801a-a7af0a985fc4" Description="Any custom attributes to be generated for the source property. Will be passed through as entered." Name="SourceCustomAttributes" DisplayName="End1 Custom Attributes" Category="End 1">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5069324e-4190-403e-8791-416c692c872a" Description="Text for [Display(Name=&quot;&lt;text&gt;&quot;)] attribute on this end's property" Name="SourceDisplayText" DisplayName="End1 Display Text" Category="End 1">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="44fda399-d8f1-4d63-80f1-881277cb8115" Description="Should this end participate in INotifyPropertyChanged activities? Only valid for non-collection targets." Name="SourceImplementNotify" DisplayName="Implement INotifyPropertyChanged" Kind="CustomStorage" Category="End 1" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4b2acec5-2746-43a0-b4be-176e3cfa533f" Description="Description for Sawczyn.EFDesigner.EFModel.BidirectionalAssociation.Is Source Implement Notify Tracking" Name="IsSourceImplementNotifyTracking" DisplayName="Is Source Implement Notify Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0f8bd2a8-4b1c-429d-96db-afbe59253dc6" Description="Decorator text" Name="SourcePropertyNameDisplay" DisplayName="Source Property Name Display" Kind="Calculated" Category="End2" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4c6e4f43-ba09-43bf-bb1c-ca0ead535a0d" Description="Defines how EF reads and write this property or its backing field. See  https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.propertyaccessmode" Name="SourcePropertyAccessMode" DisplayName="End 2 Property Access Mode" DefaultValue="FieldDuringConstruction" Category="End 1">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(PropertyAccessModeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="PropertyAccessMode" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8b64e83b-9fcc-42b7-a362-9c2931de3ce2" Description="The name of the backing field for this property" Name="SourceBackingFieldName" DisplayName="End 2 Backing Field" Kind="CustomStorage" Category="End 1">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d21b9158-3044-468d-8987-073fb9976623" Description="If false, generates a backing field and a partial method to hook getting and setting the property. If true, generates a simple auto property. Only valid for non-collection properties." Name="SourceAutoProperty" DisplayName="End2 Is Auto Property" DefaultValue="" Kind="CustomStorage" Category="End 1" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="545b091f-abd0-4ad4-8151-80c546ed1049" Description="If true, Association.SourceAutoProperty tracks ModelClass.AutoPropertyDefault" Name="IsSourceAutoPropertyTracking" DisplayName="Is Source Auto Property Tracking" DefaultValue="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="a775052d-a6a9-4916-8c4a-1bd7724b6e8b" Description="" Name="BidirectionalSource" DisplayName="Bidirectional Source" PropertyName="BidirectionalTargets" PropertyDisplayName="Bidirectional Targets">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="59ead621-a9cd-4372-b48f-2772530d09e4" Description="" Name="BidirectionalTarget" DisplayName="Bidirectional Target" PropertyName="BidirectionalSources" PropertyDisplayName="Bidirectional Sources">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="7937b5d4-2003-470b-9140-051f2dcd8dd0" Description="Relationship rooting ModelEnum domain entities to the tree" Name="ModelRootHasEnums" DisplayName="Model Root Has Enums" Namespace="Sawczyn.EFDesigner.EFModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="a613cf7f-477b-4842-b1c2-9586977463f8" Description="No description available" Name="ModelRoot" DisplayName="Model Root" PropertyName="Enums" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Enums">
          <RolePlayer>
            <DomainClassMoniker Name="ModelRoot" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="3e74772a-5e00-4977-81e5-bcfc90a8b8d9" Description="No description available" Name="ModelEnum" DisplayName="Enum" PropertyName="ModelRoot" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Model Root">
          <RolePlayer>
            <DomainClassMoniker Name="ModelEnum" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="168660d9-3989-40a9-b6ef-25d54c6e6d34" Description="Relationship linking enumeration values to an enumeration" Name="ModelEnumHasValues" DisplayName="Model Enum Has Values" Namespace="Sawczyn.EFDesigner.EFModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="0e073c3b-ea79-41d0-b820-b972052cfb86" Description="No description available" Name="Enum" DisplayName="Enum" PropertyName="Values" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Values">
          <RolePlayer>
            <DomainClassMoniker Name="ModelEnum" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="a9be9f1b-6dc5-4dae-b345-a47b3db19d2b" Description="No description available" Name="Value" DisplayName="Value" PropertyName="Enum" Multiplicity="ZeroOne" PropagatesDelete="true" PropertyDisplayName="Enum">
          <RolePlayer>
            <DomainClassMoniker Name="ModelEnumValue" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="08ff1339-a992-4ffe-b350-6ba2eab5d7a4" Description="Description for Sawczyn.EFDesigner.EFModel.ModelRootHasClasses" Name="ModelRootHasClasses" DisplayName="Model Root Has Classes" Namespace="Sawczyn.EFDesigner.EFModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="435f6b0f-d7e3-43c3-8b08-0f2a95ddd755" Description="Description for Sawczyn.EFDesigner.EFModel.ModelRootHasClasses.ModelRoot" Name="ModelRoot" DisplayName="Model Root" PropertyName="Classes" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Classes">
          <RolePlayer>
            <DomainClassMoniker Name="ModelRoot" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="82403bdb-397b-48d8-989d-a74827dcb272" Description="Description for Sawczyn.EFDesigner.EFModel.ModelRootHasClasses.ModelClass" Name="ModelClass" DisplayName="Model Class" PropertyName="ModelRoot" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Model Root">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c1a798b4-85dc-4479-9c35-30f5b15d8aa1" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesSubjects" Name="CommentReferencesSubjects" DisplayName="Comment References Subjects" InheritanceModifier="Abstract" Namespace="Sawczyn.EFDesigner.EFModel">
      <Source>
        <DomainRole Id="8624f267-1304-43ef-acc4-f0b7d67b2856" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesSubjects.Comment" Name="Comment" DisplayName="Comment" PropertyName="Subjects" IsPropertyBrowsable="false" PropertyDisplayName="Subjects">
          <RolePlayer>
            <DomainClassMoniker Name="Comment" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f2d70cc8-21b0-455b-9841-bee17b5ad5d9" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesSubjects.DesignElement" Name="DesignElement" DisplayName="Design Element" PropertyName="Comments" PropertyDisplayName="Comments">
          <RolePlayer>
            <DomainClassMoniker Name="DesignElement" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="cedffe97-5a26-4774-89bf-ea0bda108db2" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesClasses" Name="CommentReferencesClasses" DisplayName="Comment References Classes" Namespace="Sawczyn.EFDesigner.EFModel">
      <BaseRelationship>
        <DomainRelationshipMoniker Name="CommentReferencesSubjects" />
      </BaseRelationship>
      <Source>
        <DomainRole Id="ebcf4701-c44e-4ec9-8a34-0f8428cac352" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesClasses.Comment" Name="Comment" DisplayName="Comment" PropertyName="Classes" IsPropertyBrowsable="false" PropertyDisplayName="Classes">
          <RolePlayer>
            <DomainClassMoniker Name="Comment" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="2495fc6d-7420-46b0-8565-21e381828d40" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesClasses.ModelClass" Name="ModelClass" DisplayName="Model Class" PropertyName="Comments" IsPropertyGenerator="false" PropertyDisplayName="Comments">
          <RolePlayer>
            <DomainClassMoniker Name="ModelClass" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c2ca35b4-7dc5-4a7d-9a6f-cfd1cc9aedac" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesEnums" Name="CommentReferencesEnums" DisplayName="Comment References Enums" Namespace="Sawczyn.EFDesigner.EFModel">
      <BaseRelationship>
        <DomainRelationshipMoniker Name="CommentReferencesSubjects" />
      </BaseRelationship>
      <Source>
        <DomainRole Id="ed267b9d-cbe7-4888-9a95-e7519135c76b" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesEnums.Comment" Name="Comment" DisplayName="Comment" PropertyName="Enums" IsPropertyBrowsable="false" PropertyDisplayName="Enums">
          <RolePlayer>
            <DomainClassMoniker Name="Comment" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="d5ba04cb-a560-4293-bdee-b45b62742864" Description="Description for Sawczyn.EFDesigner.EFModel.CommentReferencesEnums.ModelEnum" Name="ModelEnum" DisplayName="Model Enum" PropertyName="Comments" IsPropertyGenerator="false" PropertyDisplayName="Comments">
          <RolePlayer>
            <DomainClassMoniker Name="ModelEnum" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="bbdf2307-a6c2-4cf5-b2d9-290b94558d42" Description="Description for Sawczyn.EFDesigner.EFModel.ModelRootHasModelDiagrams" Name="ModelRootHasModelDiagrams" DisplayName="Model Root Has Model Diagrams" Namespace="Sawczyn.EFDesigner.EFModel" IsEmbedding="true">
      <Source>
        <DomainRole Id="76e3ca2c-137a-4b7f-a61b-cfb0e4fe5590" Description="" Name="ModelRoot" DisplayName="Model Root" PropertyName="Diagrams" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Diagrams">
          <RolePlayer>
            <DomainClassMoniker Name="ModelRoot" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="669f90c2-5bc9-475d-b07b-1ae358cbb671" Description="Description for Sawczyn.EFDesigner.EFModel.ModelRootHasModelDiagrams.ModelDiagramData" Name="ModelDiagramData" DisplayName="Model Diagram Data" PropertyName="ModelRoot" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Model Root">
          <RolePlayer>
            <DomainClassMoniker Name="ModelDiagramData" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <DomainEnumeration Name="TypeAccessModifier" Namespace="Sawczyn.EFDesigner.EFModel" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="Public" Value="0" />
        <EnumerationLiteral Description="" Name="Private" Value="1" />
        <EnumerationLiteral Description="No description available" Name="Protected" Value="2" />
        <EnumerationLiteral Description="No description available" Name="Internal" Value="3" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="InheritanceModifier" Namespace="Sawczyn.EFDesigner.EFModel" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="None" Value="0" />
        <EnumerationLiteral Description="" Name="Abstract" Value="1" />
        <EnumerationLiteral Description="" Name="Sealed" Value="2" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="Multiplicity" Namespace="Sawczyn.EFDesigner.EFModel" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="ZeroMany" Value="0" />
        <EnumerationLiteral Description="" Name="One" Value="1" />
        <EnumerationLiteral Description="" Name="ZeroOne" Value="2" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="ContainerAccess" Namespace="Sawczyn.EFDesigner.EFModel" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="Public" Value="1" />
        <EnumerationLiteral Description="" Name="Internal" Value="0" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <DomainEnumeration Name="DeleteAction" Namespace="Sawczyn.EFDesigner.EFModel" Description="No description available">
      <Literals>
        <EnumerationLiteral Description="Force a cascade delete across this association" Name="Cascade" Value="0" />
        <EnumerationLiteral Description="Ensure other end is not automatically deleted when one end is deleted." Name="None" Value="1" />
        <EnumerationLiteral Description="Use the default Entity Framework behavior for the type of association" Name="Default" Value="2" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="DashStyle" Namespace="System.Drawing.Drawing2D" />
    <ExternalType Name="LinearGradientMode" Namespace="System.Drawing.Drawing2D" />
    <DomainEnumeration Name="CollectionType" Namespace="Sawczyn.EFDesigner.EFModel" Description="No description available">
      <Literals>
        <EnumerationLiteral Description="No description available" Name="List" Value="1" />
        <EnumerationLiteral Description="No description available" Name="HashSet" Value="0" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="EnumValueType" Namespace="Sawczyn.EFDesigner.EFModel" Description="No description available">
      <Literals>
        <EnumerationLiteral Description="short" Name="Int16" Value="0" />
        <EnumerationLiteral Description="int" Name="Int32" Value="1" />
        <EnumerationLiteral Description="long" Name="Int64" Value="2" />
        <EnumerationLiteral Description="byte" Name="Byte" Value="3" />
        <EnumerationLiteral Description="sbyte" Name="SByte" Value="4" />
        <EnumerationLiteral Description="ushort" Name="UInt16" Value="5" />
        <EnumerationLiteral Description="uint" Name="UInt32" Value="6" />
        <EnumerationLiteral Description="ulong" Name="UInt64" Value="7" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="DatabaseInitializerKind" Namespace="Sawczyn.EFDesigner.EFModel" Description="Description for Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind">
      <Literals>
        <EnumerationLiteral Description="Will recreate and optionally re-seed the database only if the database does not exist." Name="CreateDatabaseIfNotExists" Value="0" />
        <EnumerationLiteral Description="Will always recreate and optionally re-seed the database the first time that a context is used in the app domain." Name="DropCreateDatabaseAlways" Value="1" />
        <EnumerationLiteral Description="Will delete, recreate, and optionally re-seed the database only if the model has changed since the database was created." Name="DropCreateDatabaseIfModelChanges" Value="2" />
        <EnumerationLiteral Description="Will use Code First Migrations to update the database to the latest version." Name="MigrateDatabaseToLatestVersion" Value="3" />
        <EnumerationLiteral Description="Null configuration. Will not check database for correctness, speeding up initialization and queries." Name="None" Value="4" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="HTML5Type" Namespace="Sawczyn.EFDesigner.EFModel" Description="Description for Sawczyn.EFDesigner.EFModel.HTML5Type">
      <Literals>
        <EnumerationLiteral Description="No special meaning" Name="None" Value="0" />
        <EnumerationLiteral Description="Value is a color" Name="Color" Value="1" />
        <EnumerationLiteral Description="Value is a date (consider changing type to DateTime)" Name="Date" Value="2" />
        <EnumerationLiteral Description="Value is a date/time (consider changing type to DateTime)" Name="DateTime" Value="3" />
        <EnumerationLiteral Description="Value is an email address" Name="Email" Value="4" />
        <EnumerationLiteral Description="Value is a month" Name="Month" Value="5" />
        <EnumerationLiteral Description="Value is a number (consider changing type to a numeric type)" Name="Number" Value="6" />
        <EnumerationLiteral Description="Value is a numeric range" Name="Range" Value="8" />
        <EnumerationLiteral Description="Value will be used in a search predicate" Name="Search" Value="9" />
        <EnumerationLiteral Description="Value is a telephone number" Name="Telephone" Value="10" />
        <EnumerationLiteral Description="Value is a time" Name="Time" Value="11" />
        <EnumerationLiteral Description="Value is a URL" Name="URl" Value="12" />
        <EnumerationLiteral Description="Value is a calendar week" Name="Week" Value="13" />
        <EnumerationLiteral Description="Value is a password" Name="Password" Value="7" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="Concurrency" Namespace="Sawczyn.EFDesigner.EFModel" Description="Types of concurrency handling">
      <Literals>
        <EnumerationLiteral Description="Don't generate code to handle concurrency" Name="None" Value="0" />
        <EnumerationLiteral Description="Generate timestamp columns to handle optimistic concurrency" Name="Optimistic" Value="1" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="ConcurrencyOverride" Namespace="Sawczyn.EFDesigner.EFModel" Description="Description for Sawczyn.EFDesigner.EFModel.ConcurrencyOverride">
      <Literals>
        <EnumerationLiteral Description="Don't generate code to handle concurrency" Name="None" Value="1" />
        <EnumerationLiteral Description="Generate timestamp columns to handle optimistic concurrency" Name="Optimistic" Value="2" />
        <EnumerationLiteral Description="Use default for model" Name="Default" Value="0" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="EFVersion" Namespace="Sawczyn.EFDesigner.EFModel" Description="Description for Sawczyn.EFDesigner.EFModel.EFVersion">
      <Literals>
        <EnumerationLiteral Description="Entity Framework 6" Name="EF6" Value="0" />
        <EnumerationLiteral Description="Entity Framework Core" Name="EFCore" Value="1" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="IdentityType" Namespace="Sawczyn.EFDesigner.EFModel" Description="Describes identity generation">
      <Literals>
        <EnumerationLiteral Description="The associated property is not an identity" Name="None" Value="2" />
        <EnumerationLiteral Description="The value will be automatically generated in the database" Name="AutoGenerated" Value="0" />
        <EnumerationLiteral Description="The value will be entered by the program" Name="Manual" Value="1" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="SetterAccessModifier" Namespace="Sawczyn.EFDesigner.EFModel" Description="Determines attribute setter visibility">
      <Literals>
        <EnumerationLiteral Description="Property is public" Name="Public" Value="2" />
        <EnumerationLiteral Description="Property is protected" Name="Protected" Value="1" />
        <EnumerationLiteral Description="Property is internal" Name="Internal" Value="0" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="ModelClassNameProvider" Namespace="Sawczyn.EFDesigner.EFModel" />
    <ExternalType Name="ModelAttributeNameProvider" Namespace="Sawczyn.EFDesigner.EFModel" />
    <ExternalType Name="ModelEnumNameProvider" Namespace="Sawczyn.EFDesigner.EFModel" />
    <DomainEnumeration Name="CodeStrategy" Namespace="Sawczyn.EFDesigner.EFModel" Description="Which database strategy to use in code generation">
      <Literals>
        <EnumerationLiteral Description="A table will be used for every class" Name="TablePerType" Value="2" />
        <EnumerationLiteral Description="Only concrete (not abstract) classes will have tables" Name="TablePerConcreteType" Value="0" />
        <EnumerationLiteral Description="A table will be used for every inheritance chain" Name="TablePerHierarchy" Value="1" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="EndpointRole" Namespace="Sawczyn.EFDesigner.EFModel" Description="Role a class plays on an association">
      <Literals>
        <EnumerationLiteral Description="Class is the dependent part of the association" Name="Dependent" Value="1" />
        <EnumerationLiteral Description="Class is the controlling (i.e, principal) part of the association" Name="Principal" Value="2" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.EndpointRole.NotApplicable" Name="NotApplicable" Value="3" />
        <EnumerationLiteral Description="Dependent/Principal roles are design decisions and have not been set" Name="NotSet" Value="0" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="ForeignKeyOwner" Namespace="Sawczyn.EFDesigner.EFModel" Description="The class that contains the foreign key in a relationship">
      <Literals>
        <EnumerationLiteral Description="No foreign key should be generated" Name="None" Value="0" />
        <EnumerationLiteral Description="Foreign key is contained in the Source class" Name="Source" Value="1" />
        <EnumerationLiteral Description="Foreign key is contained in the Target class" Name="Target" Value="2" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="DatabaseKind" Namespace="Sawczyn.EFDesigner.EFModel" Description="Database manifest token. Optimization if runtime database type is known and unchanging.">
      <Literals>
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.DatabaseKind.SqlServer" Name="SqlServer" Value="1" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.DatabaseKind.SqlServer2012" Name="SqlServer2012" Value="2" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.DatabaseKind.None" Name="None" Value="0" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="ValueConverter" Namespace="Sawczyn.EFDesigner.EFModel" Description="Available value converters for EFCore (&gt;= 2.1)">
      <Literals>
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.BoolToZeroOneConverter" Name="BoolToZeroOneConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.BoolToStringConverter" Name="BoolToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.BoolToTwoValuesConverter" Name="BoolToTwoValuesConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.BytesToStringConverter" Name="BytesToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.CastingConverter" Name="CastingConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.CharToStringConverter" Name="CharToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.DateTimeOffsetToBinaryConverter" Name="DateTimeOffsetToBinaryConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.DateTimeOffsetToBytesConverter" Name="DateTimeOffsetToBytesConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.DateTimeOffsetToStringConverter" Name="DateTimeOffsetToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.DateTimeToBinaryConverter" Name="DateTimeToBinaryConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.DateTimeToStringConverter" Name="DateTimeToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.DateTimeToTicksConverter" Name="DateTimeToTicksConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.EnumToNumberConverter" Name="EnumToNumberConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.EnumToStringConverter" Name="EnumToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.GuidToBytesConverter" Name="GuidToBytesConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.GuidToStringConverter" Name="GuidToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.NumberToBytesConverter" Name="NumberToBytesConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.NumberToStringConverter" Name="NumberToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.StringToBytesConverter" Name="StringToBytesConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.TimeSpanToStringConverter" Name="TimeSpanToStringConverter" Value="" />
        <EnumerationLiteral Description="Description for Sawczyn.EFDesigner.EFModel.ValueConverter.TimeSpanToTicksConverter" Name="TimeSpanToTicksConverter" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="AutomaticAction" Namespace="Sawczyn.EFDesigner.EFModel" Description="Description for Sawczyn.EFDesigner.EFModel.AutomaticAction">
      <Literals>
        <EnumerationLiteral Description="Never perform the action automatically" Name="False" Value="0" />
        <EnumerationLiteral Description="Always perform the action automatically" Name="True" Value="1" />
        <EnumerationLiteral Description="Ask each time if should perform the action automatically" Name="Ask" Value="2" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Namespaces" Namespace="Sawczyn.EFDesigner.EFModel" />
    <ExternalType Name="OutputLocations" Namespace="Sawczyn.EFDesigner.EFModel" />
    <ExternalType Name="Nullable&lt;System.Int32&gt;" Namespace="System" />
    <ExternalType Name="ModelDiagramDataNameProvider" Namespace="Sawczyn.EFDesigner.EFModel" />
    <DomainEnumeration Name="PropertyAccessMode" Namespace="Sawczyn.EFDesigner.EFModel" Description="Description for Sawczyn.EFDesigner.EFModel.PropertyAccessMode">
      <Literals>
        <EnumerationLiteral Description="Enforces that all accesses to the property must go through the field. An exception will be thrown if this mode is set and it is not possible to read from or write to the field." Name="Field" Value="0" />
        <EnumerationLiteral Description="Enforces that all accesses to the property must go through the field when new instances are being constructed. New instances are typically constructed when entities are queried from the database. An exception will be thrown if this mode is set and it is not possible to write to the field. All other uses of the property will go through the property getters and setters, unless this is not possible because, for example, the property is read-only, in which case these accesses will also use the field." Name="FieldDuringConstruction" Value="1" />
        <EnumerationLiteral Description="All accesses to the property goes directly to the field, unless the field is not known, in which as access goes through the property." Name="PreferField" Value="3" />
        <EnumerationLiteral Description="All accesses to the property when constructing new entity instances goes directly to the field, unless the field is not known, in which as access goes through the property. All other uses of the property will go through the property getters and setters, unless this is not possible because, for example, the property is read-only, in which case these accesses will also use the field." Name="PreferFieldDuringConstruction" Value="4" />
        <EnumerationLiteral Description="All accesses to the property go through the property, unless there is no property or it is missing a setter/getter, in which as access goes directly to the field." Name="PreferProperty" Value="5" />
        <EnumerationLiteral Description="Enforces that all accesses to the property must go through the property getters and setters, even when new objects are being constructed. An exception will be thrown if this mode is set and it is not possible to read from or write to the property, for example because it is read-only." Name="Property" Value="2" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="PropertyExposure" Namespace="Sawczyn.EFDesigner.EFModel" Description="How the code generator will present this property to Entity Framework (EFCore5+ only)">
      <Literals>
        <EnumerationLiteral Description="Generated code will create and use this attribute as a .NET property," Name="Property" Value="" />
        <EnumerationLiteral Description="Generated code will create and use this attribute as a .NET field" Name="Field" Value="" />
      </Literals>
    </DomainEnumeration>
  </Types>
  <Shapes>
    <CompartmentShape Id="8055f08f-3d3a-435f-8b47-7afcd0e051bd" Description="" Name="ClassShape" DisplayName="Class Shape" Namespace="Sawczyn.EFDesigner.EFModel" GeneratesDoubleDerived="true" FixedTooltipText="Class Shape" TextColor="White" ExposesTextColor="true" FillColor="0, 122, 204" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" ExposesOutlineColorAsProperty="true" ExposesFillColorAsProperty="true" ExposesOutlineDashStyleAsProperty="true" ExposesOutlineThicknessAsProperty="true" Geometry="Rectangle">
      <Properties>
        <DomainProperty Id="77fd1ed0-30ca-4c62-8d29-bfc23ce78a18" Description="Fill color for shape" Name="FillColor" DisplayName="Fill Color" DefaultValue="" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a1bc67d8-5957-4fd3-ac57-e320db11bfe2" Description="Color of the shape's text" Name="TextColor" DisplayName="Text Color" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b60bda34-6c70-4825-af60-482a9112bdf8" Description="Color of the shape's outline" Name="OutlineColor" DisplayName="Outline Color" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="88235240-d714-4315-9f26-54ff8406a6b6" Description="Line style for the shape's outline" Name="OutlineDashStyle" DisplayName="Outline Dash Style" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2e283376-63d0-454e-9e56-fc6d8cb56689" Description="If true, shape is visible." Name="Visible" DisplayName="Visible" DefaultValue="true" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="18455bd8-9cd4-4566-98de-794629f3086e" Description="Thickness, in inches, of the shapes's outline" Name="OutlineThickness" DisplayName="Outline Thickness" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Single" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="Name" DisplayName="Name" DefaultText="Name" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapse" DisplayName="Expand Collapse" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="EntityGlyph" DisplayName="Entity Glyph" DefaultIcon="Resources\EntityGlyph.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="AbstractEntityGlyph" DisplayName="Abstract Entity Glyph" DefaultIcon="Resources\AbstractEntityGlyph.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="WarningGlyph" DisplayName="Warning Glyph" DefaultIcon="Resources\Warning.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TransientGlyph" DisplayName="Transient Glyph" DefaultIcon="Resources\TransientClass.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="NoGenGlyph" DisplayName="No Code Generation" DefaultIcon="Resources\No.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="DictionaryGlyph" DisplayName="Property Bag" DefaultIcon="Resources\Dictionary_16x.png" />
      </ShapeHasDecorators>
      <Compartment Name="AttributesCompartment" Title="Properties" />
      <Compartment Name="AssociationsCompartment" Title="Association Targets" />
      <Compartment Name="SourcesCompartment" Title="Association Sources" />
    </CompartmentShape>
    <GeometryShape Id="ac82cb66-4d3d-46ac-a7e2-b7f0cd67a73f" Description="" Name="CommentBoxShape" DisplayName="Comment Box Shape" Namespace="Sawczyn.EFDesigner.EFModel" GeneratesDoubleDerived="true" FixedTooltipText="Comment Box Shape" ExposesTextColor="true" FillColor="255, 255, 204" OutlineColor="204, 204, 102" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="ForwardDiagonal" ExposesOutlineColorAsProperty="true" ExposesFillColorAsProperty="true" ExposesOutlineDashStyleAsProperty="true" ExposesOutlineThicknessAsProperty="true" Geometry="RoundedRectangle">
      <Properties>
        <DomainProperty Id="276258af-085d-4201-b3e8-fb26a2d6efc4" Description="Fill color for shape" Name="FillColor" DisplayName="Fill Color" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b83f02a5-312b-4fa4-862a-93e7d51766e5" Description="Color of the shape's outline" Name="OutlineColor" DisplayName="Outline Color" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ba60eaf8-ceb3-4542-a89e-65f69b9dcbc3" Description="Color of the shape's text" Name="TextColor" DisplayName="Text Color" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e3459214-9d50-4fe5-b99f-cd1889898547" Description="Line style for the shape's outline" Name="OutlineDashStyle" DisplayName="Outline Dash Style" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="47b0cf2f-0712-480f-bdf7-d27cfbd8db3e" Description="Thickness, in inches, of the shapes's outline" Name="OutlineThickness" DisplayName="Outline Thickness" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System/Single" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8604c11f-3053-4b68-9c10-722a0aaa3b1f" Description="If true, shape is visible." Name="Visible" DisplayName="Visible" DefaultValue="true" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="Center" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="Comment" DisplayName="Comment" DefaultText="" />
      </ShapeHasDecorators>
    </GeometryShape>
    <CompartmentShape Id="de514c36-0966-422a-9511-997b89ac7a56" Description="" Name="EnumShape" DisplayName="Enum Shape" Namespace="Sawczyn.EFDesigner.EFModel" GeneratesDoubleDerived="true" FixedTooltipText="Enum Shape" TextColor="White" ExposesTextColor="true" FillColor="Gray" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" ExposesOutlineColorAsProperty="true" ExposesFillColorAsProperty="true" ExposesOutlineDashStyleAsProperty="true" ExposesOutlineThicknessAsProperty="true" Geometry="Rectangle">
      <Properties>
        <DomainProperty Id="b4b78660-37fd-48ee-90c1-4a338c5db791" Description="Fill color for shape" Name="FillColor" DisplayName="Fill Color" DefaultValue="" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="764aa9b3-eb39-4bc7-9371-0e1ec80f3cff" Description="Color of the shape's text" Name="TextColor" DisplayName="Text Color" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e8ae18fb-d462-4a71-8b57-00ff03bb5506" Description="Color of the shape's outline" Name="OutlineColor" DisplayName="Outline Color" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="53db1f2a-6349-411e-b28a-08a6f8ed36aa" Description="If true, shape is visible." Name="Visible" DisplayName="Visible" DefaultValue="true" Kind="CustomStorage" Category="Display">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="22d0c259-c2d2-4bd5-a6c8-5eb78701ead0" Description="Thickness, in inches, of the shapes's outline" Name="OutlineThickness" DisplayName="Outline Thickness" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Single" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="06df5a34-08ff-4a03-bea5-444f2bef9454" Description="Line style for the shape's outline" Name="OutlineDashStyle" DisplayName="Outline Dash Style" Kind="CustomStorage" Category="Display" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="Name" DisplayName="Name" DefaultText="Name" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapse" DisplayName="Expand/Collapse" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="EnumGlyph" DisplayName="Enumeration" DefaultIcon="Resources\Enumerator_16x.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="WarningGlyph" DisplayName="Warning" DefaultIcon="Resources\Warning.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="16" VerticalOffset="0">
        <IconDecorator Name="NoGenGlyph" DisplayName="No Code Generation" DefaultIcon="Resources\No.png" />
      </ShapeHasDecorators>
      <Compartment Name="ValuesCompartment" Title="Values" />
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="6b6c3915-4ad2-4118-ab70-d1adf80dc3ba" Description="" Name="AssociationConnector" DisplayName="Association Connector" InheritanceModifier="Abstract" Namespace="Sawczyn.EFDesigner.EFModel" GeneratesDoubleDerived="true" FixedTooltipText="Association Connector" Color="113, 111, 110" Thickness="0.01" ExposesColorAsProperty="true" ExposesDashStyleAsProperty="true">
      <Properties>
        <DomainProperty Id="87700344-4a49-4734-b376-47dd7ec79b24" Description="No description available" Name="Color" DisplayName="Color" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="657f50bd-4284-4ec3-bc1d-7dcdf731ab62" Description="No description available" Name="DashStyle" DisplayName="Dash Style" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
      </Properties>
      <ConnectorHasDecorators Position="TargetBottom" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="TargetPropertyNameDisplay" DisplayName="Target Property Name Display" DefaultText="TargetPropertyNameDisplay" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="SourceBottom" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="SourceMultiplicityDisplay" DisplayName="Source Multiplicity Display" DefaultText="SourceMultiplicityDisplay" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="TargetTop" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="TargetMultiplicityDisplay" DisplayName="Target Multiplicity Display" DefaultText="TargetMultiplicityDisplay" />
      </ConnectorHasDecorators>
    </Connector>
    <Connector Id="f7199b16-1d1e-4783-ac2f-f0e4d12b219f" Description="" Name="UnidirectionalConnector" DisplayName="Unidirectional Connector" Namespace="Sawczyn.EFDesigner.EFModel" TooltipType="Variable" FixedTooltipText="Unidirectional Connector" Color="113, 111, 110" TargetEndStyle="EmptyArrow" Thickness="0.01">
      <BaseConnector>
        <ConnectorMoniker Name="AssociationConnector" />
      </BaseConnector>
    </Connector>
    <Connector Id="84eb1033-0168-44f5-bf5d-76a6a748c53f" Description="" Name="BidirectionalConnector" DisplayName="Bidirectional Connector" Namespace="Sawczyn.EFDesigner.EFModel" TooltipType="Variable" FixedTooltipText="" Color="113, 111, 110" Thickness="0.01">
      <BaseConnector>
        <ConnectorMoniker Name="AssociationConnector" />
      </BaseConnector>
      <ConnectorHasDecorators Position="SourceTop" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="SourcePropertyNameDisplay" DisplayName="Source Property Name Display" DefaultText="SourcePropertyNameDisplay" />
      </ConnectorHasDecorators>
    </Connector>
    <Connector Id="6c2b3d5f-a15e-4480-913f-b1dac8612868" Description="" Name="GeneralizationConnector" DisplayName="Generalization Connector" Namespace="Sawczyn.EFDesigner.EFModel" TooltipType="Variable" FixedTooltipText="Generalization Connector" Color="113, 111, 110" SourceEndStyle="HollowArrow" Thickness="0.01" />
    <Connector Id="bb10a438-849e-4964-b7c6-55f2c121965a" Description="" Name="CommentConnector" DisplayName="Comment Connector" Namespace="Sawczyn.EFDesigner.EFModel" FixedTooltipText="Comment Connector" Color="113, 111, 110" DashStyle="Dot" Thickness="0.01" RoutingStyle="Straight" />
  </Connectors>
  <XmlSerializationBehavior Name="EFModelSerializationBehavior" Namespace="Sawczyn.EFDesigner.EFModel">
    <ClassData>
      <XmlClassData TypeName="Association" MonikerAttributeName="" SerializeId="true" MonikerElementName="associationMoniker" ElementName="association" MonikerTypeName="AssociationMoniker">
        <DomainRelationshipMoniker Name="Association" />
        <ElementData>
          <XmlPropertyData XmlName="sourceMultiplicity">
            <DomainPropertyMoniker Name="Association/SourceMultiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetMultiplicity">
            <DomainPropertyMoniker Name="Association/TargetMultiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetPropertyName">
            <DomainPropertyMoniker Name="Association/TargetPropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="persistent">
            <DomainPropertyMoniker Name="Association/Persistent" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceMultiplicityDisplay" Representation="Ignore">
            <DomainPropertyMoniker Name="Association/SourceMultiplicityDisplay" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetMultiplicityDisplay" Representation="Ignore">
            <DomainPropertyMoniker Name="Association/TargetMultiplicityDisplay" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceDeleteAction">
            <DomainPropertyMoniker Name="Association/SourceDeleteAction" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetDeleteAction">
            <DomainPropertyMoniker Name="Association/TargetDeleteAction" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="collectionClass">
            <DomainPropertyMoniker Name="Association/CollectionClass" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetDescription">
            <DomainPropertyMoniker Name="Association/TargetDescription" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetSummary">
            <DomainPropertyMoniker Name="Association/TargetSummary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Association/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isCollectionClassTracking">
            <DomainPropertyMoniker Name="Association/IsCollectionClassTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceRole">
            <DomainPropertyMoniker Name="Association/SourceRole" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetRole">
            <DomainPropertyMoniker Name="Association/TargetRole" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="foreignKeyLocation">
            <DomainPropertyMoniker Name="Association/ForeignKeyLocation" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetCustomAttributes">
            <DomainPropertyMoniker Name="Association/TargetCustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetDisplayText">
            <DomainPropertyMoniker Name="Association/TargetDisplayText" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetImplementNotify">
            <DomainPropertyMoniker Name="Association/TargetImplementNotify" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isTargetImplementNotifyTracking">
            <DomainPropertyMoniker Name="Association/IsTargetImplementNotifyTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fKPropertyName">
            <DomainPropertyMoniker Name="Association/FKPropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetPropertyNameDisplay" Representation="Ignore">
            <DomainPropertyMoniker Name="Association/TargetPropertyNameDisplay" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name" Representation="Ignore">
            <DomainPropertyMoniker Name="Association/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="joinTableName">
            <DomainPropertyMoniker Name="Association/JoinTableName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetBackingFieldName">
            <DomainPropertyMoniker Name="Association/TargetBackingFieldName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetPropertyAccessMode">
            <DomainPropertyMoniker Name="Association/TargetPropertyAccessMode" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetAutoProperty">
            <DomainPropertyMoniker Name="Association/TargetAutoProperty" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isTargetAutoPropertyTracking">
            <DomainPropertyMoniker Name="Association/IsTargetAutoPropertyTracking" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClassHasAttributes" MonikerAttributeName="" SerializeId="true" MonikerElementName="classHasAttributesMoniker" ElementName="classHasAttributes" MonikerTypeName="ClassHasAttributesMoniker">
        <DomainRelationshipMoniker Name="ClassHasAttributes" />
      </XmlClassData>
      <XmlClassData TypeName="ModelRootHasComments" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelRootHasCommentsMoniker" ElementName="modelRootHasComments" MonikerTypeName="ModelRootHasCommentsMoniker">
        <DomainRelationshipMoniker Name="ModelRootHasComments" />
      </XmlClassData>
      <XmlClassData TypeName="Generalization" MonikerAttributeName="" SerializeId="true" MonikerElementName="generalizationMoniker" ElementName="generalization" MonikerTypeName="GeneralizationMoniker">
        <DomainRelationshipMoniker Name="Generalization" />
        <ElementData>
          <XmlPropertyData XmlName="name" Representation="Ignore">
            <DomainPropertyMoniker Name="Generalization/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelRoot" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelRootMoniker" ElementName="modelRoot" MonikerTypeName="ModelRootMoniker">
        <DomainClassMoniker Name="ModelRoot" />
        <ElementData>
          <XmlRelationshipData RoleElementName="comments">
            <DomainRelationshipMoniker Name="ModelRootHasComments" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="lazyLoadingEnabled">
            <DomainPropertyMoniker Name="ModelRoot/LazyLoadingEnabled" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="entityContainerAccess">
            <DomainPropertyMoniker Name="ModelRoot/EntityContainerAccess" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="entityContainerName">
            <DomainPropertyMoniker Name="ModelRoot/EntityContainerName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="ModelRoot/Namespace" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="enums">
            <DomainRelationshipMoniker Name="ModelRootHasEnums" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="databaseInitializerType">
            <DomainPropertyMoniker Name="ModelRoot/DatabaseInitializerType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="connectionString">
            <DomainPropertyMoniker Name="ModelRoot/ConnectionString" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="automaticMigrationsEnabled">
            <DomainPropertyMoniker Name="ModelRoot/AutomaticMigrationsEnabled" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="entityOutputDirectory">
            <DomainPropertyMoniker Name="ModelRoot/EntityOutputDirectory" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="contextOutputDirectory">
            <DomainPropertyMoniker Name="ModelRoot/ContextOutputDirectory" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="enumOutputDirectory">
            <DomainPropertyMoniker Name="ModelRoot/EnumOutputDirectory" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseSchema">
            <DomainPropertyMoniker Name="ModelRoot/DatabaseSchema" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="concurrencyDefault">
            <DomainPropertyMoniker Name="ModelRoot/ConcurrencyDefault" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fileNameMarker">
            <DomainPropertyMoniker Name="ModelRoot/FileNameMarker" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="entityFrameworkVersion">
            <DomainPropertyMoniker Name="ModelRoot/EntityFrameworkVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="connectionStringName">
            <DomainPropertyMoniker Name="ModelRoot/ConnectionStringName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="chopMethodChains">
            <DomainPropertyMoniker Name="ModelRoot/ChopMethodChains" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="inheritanceStrategy">
            <DomainPropertyMoniker Name="ModelRoot/InheritanceStrategy" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="defaultCollectionClass">
            <DomainPropertyMoniker Name="ModelRoot/DefaultCollectionClass" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="proxyGenerationEnabled">
            <DomainPropertyMoniker Name="ModelRoot/ProxyGenerationEnabled" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="transformOnSave">
            <DomainPropertyMoniker Name="ModelRoot/TransformOnSave" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="defaultIdentityType">
            <DomainPropertyMoniker Name="ModelRoot/DefaultIdentityType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="showCascadeDeletes">
            <DomainPropertyMoniker Name="ModelRoot/ShowCascadeDeletes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseType">
            <DomainPropertyMoniker Name="ModelRoot/DatabaseType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="warnOnMissingDocumentation">
            <DomainPropertyMoniker Name="ModelRoot/WarnOnMissingDocumentation" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="entityFrameworkPackageVersion">
            <DomainPropertyMoniker Name="ModelRoot/EntityFrameworkPackageVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="structOutputDirectory">
            <DomainPropertyMoniker Name="ModelRoot/StructOutputDirectory" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dbSetAccess">
            <DomainPropertyMoniker Name="ModelRoot/DbSetAccess" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ModelRoot/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ModelRoot/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="showWarningsInDesigner">
            <DomainPropertyMoniker Name="ModelRoot/ShowWarningsInDesigner" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="classes">
            <DomainRelationshipMoniker Name="ModelRootHasClasses" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="entityNamespace">
            <DomainPropertyMoniker Name="ModelRoot/EntityNamespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="enumNamespace">
            <DomainPropertyMoniker Name="ModelRoot/EnumNamespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="structNamespace">
            <DomainPropertyMoniker Name="ModelRoot/StructNamespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespaces" Representation="Ignore">
            <DomainPropertyMoniker Name="ModelRoot/Namespaces" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outputLocations" Representation="Ignore">
            <DomainPropertyMoniker Name="ModelRoot/OutputLocations" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="exposeForeignKeys">
            <DomainPropertyMoniker Name="ModelRoot/ExposeForeignKeys" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="baseClass">
            <DomainPropertyMoniker Name="ModelRoot/BaseClass" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="diagrams">
            <DomainRelationshipMoniker Name="ModelRootHasModelDiagrams" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="showGrid">
            <DomainPropertyMoniker Name="ModelRoot/ShowGrid" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="snapToGrid">
            <DomainPropertyMoniker Name="ModelRoot/SnapToGrid" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="gridColor">
            <DomainPropertyMoniker Name="ModelRoot/GridColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="gridSize">
            <DomainPropertyMoniker Name="ModelRoot/GridSize" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="showForeignKeyPropertyNames">
            <DomainPropertyMoniker Name="ModelRoot/ShowForeignKeyPropertyNames" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseCollationDefault">
            <DomainPropertyMoniker Name="ModelRoot/DatabaseCollationDefault" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="propertyAccessModeDefault">
            <DomainPropertyMoniker Name="ModelRoot/PropertyAccessModeDefault" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="useTabs">
            <DomainPropertyMoniker Name="ModelRoot/UseTabs" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelClass" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelClassMoniker" ElementName="modelClass" MonikerTypeName="ModelClassMoniker">
        <DomainClassMoniker Name="ModelClass" />
        <ElementData>
          <XmlPropertyData XmlName="isAbstract">
            <DomainPropertyMoniker Name="ModelClass/IsAbstract" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="unidirectionalTargets">
            <DomainRelationshipMoniker Name="UnidirectionalAssociation" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="attributes">
            <DomainRelationshipMoniker Name="ClassHasAttributes" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="subclasses">
            <DomainRelationshipMoniker Name="Generalization" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="bidirectionalTargets">
            <DomainRelationshipMoniker Name="BidirectionalAssociation" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="targets">
            <DomainRelationshipMoniker Name="Association" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="tableName">
            <DomainPropertyMoniker Name="ModelClass/TableName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseSchema">
            <DomainPropertyMoniker Name="ModelClass/DatabaseSchema" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="concurrency">
            <DomainPropertyMoniker Name="ModelClass/Concurrency" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isDatabaseSchemaTracking">
            <DomainPropertyMoniker Name="ModelClass/IsDatabaseSchemaTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="ModelClass/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isNamespaceTracking">
            <DomainPropertyMoniker Name="ModelClass/IsNamespaceTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dbSetName">
            <DomainPropertyMoniker Name="ModelClass/DbSetName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModelClass/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="implementNotify">
            <DomainPropertyMoniker Name="ModelClass/ImplementNotify" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="customInterfaces">
            <DomainPropertyMoniker Name="ModelClass/CustomInterfaces" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isDependentType">
            <DomainPropertyMoniker Name="ModelClass/IsDependentType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outputDirectory">
            <DomainPropertyMoniker Name="ModelClass/OutputDirectory" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isOutputDirectoryTracking">
            <DomainPropertyMoniker Name="ModelClass/IsOutputDirectoryTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="glyphType" Representation="Ignore">
            <DomainPropertyMoniker Name="ModelClass/GlyphType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ModelClass/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ModelClass/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="baseClass" Representation="Ignore">
            <DomainPropertyMoniker Name="ModelClass/BaseClass" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="ModelClass/CustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="autoPropertyDefault">
            <DomainPropertyMoniker Name="ModelClass/AutoPropertyDefault" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="generateCode">
            <DomainPropertyMoniker Name="ModelClass/GenerateCode" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isPropertyBag">
            <DomainPropertyMoniker Name="ModelClass/IsPropertyBag" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isQueryType">
            <DomainPropertyMoniker Name="ModelClass/IsQueryType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="excludeFromMigrations">
            <DomainPropertyMoniker Name="ModelClass/ExcludeFromMigrations" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isDatabaseView">
            <DomainPropertyMoniker Name="ModelClass/IsDatabaseView" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="viewName">
            <DomainPropertyMoniker Name="ModelClass/ViewName" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelAttribute" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelAttributeMoniker" ElementName="modelAttribute" MonikerTypeName="ModelAttributeMoniker">
        <DomainClassMoniker Name="ModelAttribute" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="ModelAttribute/Type" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="initialValue">
            <DomainPropertyMoniker Name="ModelAttribute/InitialValue" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isIdentity">
            <DomainPropertyMoniker Name="ModelAttribute/IsIdentity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="required">
            <DomainPropertyMoniker Name="ModelAttribute/Required" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="persistent">
            <DomainPropertyMoniker Name="ModelAttribute/Persistent" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="maxLength">
            <DomainPropertyMoniker Name="ModelAttribute/MaxLength" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="indexed">
            <DomainPropertyMoniker Name="ModelAttribute/Indexed" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="indexedUnique">
            <DomainPropertyMoniker Name="ModelAttribute/IndexedUnique" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="stringType">
            <DomainPropertyMoniker Name="ModelAttribute/StringType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="tableOverride">
            <DomainPropertyMoniker Name="ModelAttribute/TableOverride" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isConcurrencyToken">
            <DomainPropertyMoniker Name="ModelAttribute/IsConcurrencyToken" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="identityType">
            <DomainPropertyMoniker Name="ModelAttribute/IdentityType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ModelAttribute/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ModelAttribute/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModelAttribute/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="setterVisibility">
            <DomainPropertyMoniker Name="ModelAttribute/SetterVisibility" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="virtual">
            <DomainPropertyMoniker Name="ModelAttribute/Virtual" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="readOnly">
            <DomainPropertyMoniker Name="ModelAttribute/ReadOnly" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="autoProperty">
            <DomainPropertyMoniker Name="ModelAttribute/AutoProperty" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="minLength">
            <DomainPropertyMoniker Name="ModelAttribute/MinLength" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="columnName">
            <DomainPropertyMoniker Name="ModelAttribute/ColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isColumnNameTracking">
            <DomainPropertyMoniker Name="ModelAttribute/IsColumnNameTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="columnType">
            <DomainPropertyMoniker Name="ModelAttribute/ColumnType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isColumnTypeTracking">
            <DomainPropertyMoniker Name="ModelAttribute/IsColumnTypeTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="ModelAttribute/CustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="displayText">
            <DomainPropertyMoniker Name="ModelAttribute/DisplayText" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="implementNotify">
            <DomainPropertyMoniker Name="ModelAttribute/ImplementNotify" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isImplementNotifyTracking">
            <DomainPropertyMoniker Name="ModelAttribute/IsImplementNotifyTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isAutoPropertyTracking">
            <DomainPropertyMoniker Name="ModelAttribute/IsAutoPropertyTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isForeignKeyFor">
            <DomainPropertyMoniker Name="ModelAttribute/IsForeignKeyFor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="backingFieldName">
            <DomainPropertyMoniker Name="ModelAttribute/BackingFieldName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseCollation">
            <DomainPropertyMoniker Name="ModelAttribute/DatabaseCollation" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isDatabaseCollationTracking">
            <DomainPropertyMoniker Name="ModelAttribute/IsDatabaseCollationTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="propertyAccessMode">
            <DomainPropertyMoniker Name="ModelAttribute/PropertyAccessMode" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Comment" MonikerAttributeName="" SerializeId="true" MonikerElementName="commentMoniker" ElementName="comment" MonikerTypeName="CommentMoniker">
        <DomainClassMoniker Name="Comment" />
        <ElementData>
          <XmlPropertyData XmlName="text">
            <DomainPropertyMoniker Name="Comment/Text" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="subjects">
            <DomainRelationshipMoniker Name="CommentReferencesSubjects" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="classes">
            <DomainRelationshipMoniker Name="CommentReferencesClasses" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="enums">
            <DomainRelationshipMoniker Name="CommentReferencesEnums" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="shortText" Representation="Ignore">
            <DomainPropertyMoniker Name="Comment/ShortText" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="UnidirectionalAssociation" MonikerAttributeName="" SerializeId="true" MonikerElementName="unidirectionalAssociationMoniker" ElementName="unidirectionalAssociation" MonikerTypeName="UnidirectionalAssociationMoniker">
        <DomainRelationshipMoniker Name="UnidirectionalAssociation" />
      </XmlClassData>
      <XmlClassData TypeName="BidirectionalAssociation" MonikerAttributeName="" SerializeId="true" MonikerElementName="bidirectionalAssociationMoniker" ElementName="bidirectionalAssociation" MonikerTypeName="BidirectionalAssociationMoniker">
        <DomainRelationshipMoniker Name="BidirectionalAssociation" />
        <ElementData>
          <XmlPropertyData XmlName="sourcePropertyName">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourcePropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceDescription">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceDescription" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceSummary">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceSummary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceCustomAttributes">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceCustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceDisplayText">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceDisplayText" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceImplementNotify">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceImplementNotify" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isSourceImplementNotifyTracking">
            <DomainPropertyMoniker Name="BidirectionalAssociation/IsSourceImplementNotifyTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourcePropertyNameDisplay" Representation="Ignore">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourcePropertyNameDisplay" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourcePropertyAccessMode">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourcePropertyAccessMode" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceBackingFieldName">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceBackingFieldName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceAutoProperty">
            <DomainPropertyMoniker Name="BidirectionalAssociation/SourceAutoProperty" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isSourceAutoPropertyTracking">
            <DomainPropertyMoniker Name="BidirectionalAssociation/IsSourceAutoPropertyTracking" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClassShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="classShapeMoniker" ElementName="classShape" MonikerTypeName="ClassShapeMoniker">
        <CompartmentShapeMoniker Name="ClassShape" />
        <ElementData>
          <XmlPropertyData XmlName="fillColor">
            <DomainPropertyMoniker Name="ClassShape/FillColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="textColor">
            <DomainPropertyMoniker Name="ClassShape/TextColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineColor">
            <DomainPropertyMoniker Name="ClassShape/OutlineColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineDashStyle">
            <DomainPropertyMoniker Name="ClassShape/OutlineDashStyle" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="visible">
            <DomainPropertyMoniker Name="ClassShape/Visible" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineThickness">
            <DomainPropertyMoniker Name="ClassShape/OutlineThickness" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CommentBoxShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="commentBoxShapeMoniker" ElementName="commentBoxShape" MonikerTypeName="CommentBoxShapeMoniker">
        <GeometryShapeMoniker Name="CommentBoxShape" />
        <ElementData>
          <XmlPropertyData XmlName="fillColor">
            <DomainPropertyMoniker Name="CommentBoxShape/FillColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineColor">
            <DomainPropertyMoniker Name="CommentBoxShape/OutlineColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="textColor">
            <DomainPropertyMoniker Name="CommentBoxShape/TextColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineDashStyle">
            <DomainPropertyMoniker Name="CommentBoxShape/OutlineDashStyle" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineThickness">
            <DomainPropertyMoniker Name="CommentBoxShape/OutlineThickness" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="visible">
            <DomainPropertyMoniker Name="CommentBoxShape/Visible" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AssociationConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="associationConnectorMoniker" ElementName="associationConnector" MonikerTypeName="AssociationConnectorMoniker">
        <ConnectorMoniker Name="AssociationConnector" />
        <ElementData>
          <XmlPropertyData XmlName="color">
            <DomainPropertyMoniker Name="AssociationConnector/Color" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dashStyle">
            <DomainPropertyMoniker Name="AssociationConnector/DashStyle" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="UnidirectionalConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="unidirectionalConnectorMoniker" ElementName="unidirectionalConnector" MonikerTypeName="UnidirectionalConnectorMoniker">
        <ConnectorMoniker Name="UnidirectionalConnector" />
      </XmlClassData>
      <XmlClassData TypeName="BidirectionalConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="bidirectionalConnectorMoniker" ElementName="bidirectionalConnector" MonikerTypeName="BidirectionalConnectorMoniker">
        <ConnectorMoniker Name="BidirectionalConnector" />
      </XmlClassData>
      <XmlClassData TypeName="GeneralizationConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="generalizationConnectorMoniker" ElementName="generalizationConnector" MonikerTypeName="GeneralizationConnectorMoniker">
        <ConnectorMoniker Name="GeneralizationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="CommentConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="commentConnectorMoniker" ElementName="commentConnector" MonikerTypeName="CommentConnectorMoniker">
        <ConnectorMoniker Name="CommentConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EFModelDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="eFModelDiagramMoniker" ElementName="eFModelDiagram" MonikerTypeName="EFModelDiagramMoniker">
        <DiagramMoniker Name="EFModelDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="ModelEnum" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelEnumMoniker" ElementName="modelEnum" MonikerTypeName="ModelEnumMoniker">
        <DomainClassMoniker Name="ModelEnum" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="values">
            <DomainRelationshipMoniker Name="ModelEnumHasValues" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="valueType">
            <DomainPropertyMoniker Name="ModelEnum/ValueType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="ModelEnum/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isNamespaceTracking">
            <DomainPropertyMoniker Name="ModelEnum/IsNamespaceTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModelEnum/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isFlags">
            <DomainPropertyMoniker Name="ModelEnum/IsFlags" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outputDirectory">
            <DomainPropertyMoniker Name="ModelEnum/OutputDirectory" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isOutputDirectoryTracking">
            <DomainPropertyMoniker Name="ModelEnum/IsOutputDirectoryTracking" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="glyphType" Representation="Ignore">
            <DomainPropertyMoniker Name="ModelEnum/GlyphType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ModelEnum/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ModelEnum/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="ModelEnum/CustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="generateCode">
            <DomainPropertyMoniker Name="ModelEnum/GenerateCode" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelRootHasEnums" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelRootHasEnumsMoniker" ElementName="modelRootHasEnums" MonikerTypeName="ModelRootHasEnumsMoniker">
        <DomainRelationshipMoniker Name="ModelRootHasEnums" />
      </XmlClassData>
      <XmlClassData TypeName="ModelEnumValue" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelEnumValueMoniker" ElementName="modelEnumValue" MonikerTypeName="ModelEnumValueMoniker">
        <DomainClassMoniker Name="ModelEnumValue" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModelEnumValue/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="ModelEnumValue/Value" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ModelEnumValue/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ModelEnumValue/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="customAttributes">
            <DomainPropertyMoniker Name="ModelEnumValue/CustomAttributes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="displayText">
            <DomainPropertyMoniker Name="ModelEnumValue/DisplayText" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelEnumHasValues" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelEnumHasValuesMoniker" ElementName="modelEnumHasValues" MonikerTypeName="ModelEnumHasValuesMoniker">
        <DomainRelationshipMoniker Name="ModelEnumHasValues" />
      </XmlClassData>
      <XmlClassData TypeName="EnumShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="enumShapeMoniker" ElementName="enumShape" MonikerTypeName="EnumShapeMoniker">
        <CompartmentShapeMoniker Name="EnumShape" />
        <ElementData>
          <XmlPropertyData XmlName="fillColor">
            <DomainPropertyMoniker Name="EnumShape/FillColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="textColor">
            <DomainPropertyMoniker Name="EnumShape/TextColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineColor">
            <DomainPropertyMoniker Name="EnumShape/OutlineColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="visible">
            <DomainPropertyMoniker Name="EnumShape/Visible" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineThickness">
            <DomainPropertyMoniker Name="EnumShape/OutlineThickness" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineDashStyle">
            <DomainPropertyMoniker Name="EnumShape/OutlineDashStyle" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelRootHasClasses" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelRootHasClassesMoniker" ElementName="modelRootHasClasses" MonikerTypeName="ModelRootHasClassesMoniker">
        <DomainRelationshipMoniker Name="ModelRootHasClasses" />
      </XmlClassData>
      <XmlClassData TypeName="DesignElement" MonikerAttributeName="" SerializeId="true" MonikerElementName="designElementMoniker" ElementName="designElement" MonikerTypeName="DesignElementMoniker">
        <DomainClassMoniker Name="DesignElement" />
      </XmlClassData>
      <XmlClassData TypeName="CommentReferencesSubjects" MonikerAttributeName="" SerializeId="true" MonikerElementName="commentReferencesSubjectsMoniker" ElementName="commentReferencesSubjects" MonikerTypeName="CommentReferencesSubjectsMoniker">
        <DomainRelationshipMoniker Name="CommentReferencesSubjects" />
      </XmlClassData>
      <XmlClassData TypeName="CommentReferencesClasses" MonikerAttributeName="" SerializeId="true" MonikerElementName="commentReferencesClassesMoniker" ElementName="commentReferencesClasses" MonikerTypeName="CommentReferencesClassesMoniker">
        <DomainRelationshipMoniker Name="CommentReferencesClasses" />
      </XmlClassData>
      <XmlClassData TypeName="CommentReferencesEnums" MonikerAttributeName="" SerializeId="true" MonikerElementName="commentReferencesEnumsMoniker" ElementName="commentReferencesEnums" MonikerTypeName="CommentReferencesEnumsMoniker">
        <DomainRelationshipMoniker Name="CommentReferencesEnums" />
      </XmlClassData>
      <XmlClassData TypeName="ModelDiagramData" MonikerAttributeName="" MonikerElementName="modelDiagramDataMoniker" ElementName="modelDiagramData" MonikerTypeName="ModelDiagramDataMoniker">
        <DomainClassMoniker Name="ModelDiagramData" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModelDiagramData/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelRootHasModelDiagrams" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelRootHasModelDiagramsMoniker" ElementName="modelRootHasModelDiagrams" MonikerTypeName="ModelRootHasModelDiagramsMoniker">
        <DomainRelationshipMoniker Name="ModelRootHasModelDiagrams" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="EFModelExplorer">
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\EntityGlyph.bmp">
        <Class>
          <DomainClassMoniker Name="ModelClass" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Enumerator_16x.png">
        <Class>
          <DomainClassMoniker Name="ModelEnum" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Attribute.bmp">
        <Class>
          <DomainClassMoniker Name="ModelAttribute" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\EnumItem_16x.png">
        <Class>
          <DomainClassMoniker Name="ModelEnumValue" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\CommentTool.bmp">
        <Class>
          <DomainClassMoniker Name="Comment" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <DomainPropertyMoniker Name="Comment/ShortText" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\ShowDiagramPane_16x.png">
        <Class>
          <DomainClassMoniker Name="ModelDiagramData" />
        </Class>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
    <HiddenNodes>
      <DomainPath>ModelRootHasComments.Comments</DomainPath>
    </HiddenNodes>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="UnidirectionalAssociationBuilder">
      <LinkConnectDirective UsesCustomConnect="true">
        <DomainRelationshipMoniker Name="UnidirectionalAssociation" />
        <SourceDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelAttribute" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelAttribute" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="BidirectionalAssociationBuilder">
      <LinkConnectDirective UsesCustomConnect="true">
        <DomainRelationshipMoniker Name="BidirectionalAssociation" />
        <SourceDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelAttribute" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true" UsesRoleSpecificCustomConnect="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelAttribute" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="GeneralizationBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="Generalization" />
        <SourceDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="CommentReferencesClassesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="CommentReferencesClasses" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Comment" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ModelClass" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="CommentReferencesEnumsBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="CommentReferencesEnums" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Comment" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ModelEnum" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="4bd5b7e6-86b6-43d2-962d-b6e87ac4690a" Description="" Name="EFModelDiagram" DisplayName="Class Diagram" Namespace="Sawczyn.EFDesigner.EFModel">
    <Class>
      <DomainClassMoniker Name="ModelRoot" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="ModelClass" />
        <ParentElementPath>
          <DomainPath>ModelRootHasClasses.ModelRoot/!ModelRoot</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ClassShape/Name" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ModelClass/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="ClassShape/EntityGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelClass/GlyphType" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="EntityGlyph" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="ClassShape/AbstractEntityGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelClass/GlyphType" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="AbstractEntityGlyph" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="ClassShape/WarningGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelClass/GlyphType" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="WarningGlyph" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="ClassShape/TransientGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelClass/GlyphType" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="TransientGlyph" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="ClassShape/NoGenGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelClass/GenerateCode" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="False" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ClassShape" />
        <CompartmentMap DisplaysCustomString="true">
          <CompartmentMoniker Name="ClassShape/AttributesCompartment" />
          <ElementsDisplayed>
            <DomainPath>ClassHasAttributes.Attributes/!Attribute</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ModelAttribute/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap DisplaysCustomString="true">
          <CompartmentMoniker Name="ClassShape/AssociationsCompartment" />
          <ElementsDisplayed>
            <DomainPath>Association.Targets</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ModelClass/Name" />
              <DomainPath>Association!Target</DomainPath>
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap DisplaysCustomString="true">
          <CompartmentMoniker Name="ClassShape/SourcesCompartment" />
          <ElementsDisplayed>
            <DomainPath>BidirectionalAssociation.BidirectionalSources</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ModelClass/Name" />
              <DomainPath>BidirectionalAssociation!BidirectionalSource</DomainPath>
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="Comment" />
        <ParentElementPath>
          <DomainPath>ModelRootHasComments.ModelRoot/!ModelRoot</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="CommentBoxShape/Comment" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Comment/Text" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="CommentBoxShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="ModelEnum" />
        <ParentElementPath>
          <DomainPath>ModelRootHasEnums.ModelRoot/!ModelRoot</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EnumShape/Name" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ModelEnum/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="EnumShape/EnumGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelEnum/GlyphType" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="EnumGlyph" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="EnumShape/WarningGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelEnum/GlyphType" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="WarningGlyph" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="EnumShape/NoGenGlyph" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="ModelEnum/GenerateCode" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="False" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EnumShape" />
        <CompartmentMap DisplaysCustomString="true">
          <CompartmentMoniker Name="EnumShape/ValuesCompartment" />
          <ElementsDisplayed>
            <DomainPath>ModelEnumHasValues.Values/!Value</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ModelEnumValue/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="BidirectionalConnector" />
        <DomainRelationshipMoniker Name="BidirectionalAssociation" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/TargetPropertyNameDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/TargetPropertyNameDisplay" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="BidirectionalConnector/SourcePropertyNameDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="BidirectionalAssociation/SourcePropertyNameDisplay" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/SourceMultiplicityDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/SourceMultiplicityDisplay" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/TargetMultiplicityDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/TargetMultiplicityDisplay" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="UnidirectionalConnector" />
        <DomainRelationshipMoniker Name="UnidirectionalAssociation" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/TargetPropertyNameDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/TargetPropertyNameDisplay" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/TargetMultiplicityDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/TargetMultiplicityDisplay" />
              <DomainPath />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/SourceMultiplicityDisplay" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Association/SourceMultiplicityDisplay" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="GeneralizationConnector" />
        <DomainRelationshipMoniker Name="Generalization" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="CommentConnector" />
        <DomainRelationshipMoniker Name="CommentReferencesSubjects" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="efmodel" Icon="Resources\File.ico" EditorGuid="4e135186-c9c4-4b55-8959-217a3e025622" usesStickyToolboxItems="true">
    <RootClass>
      <DomainClassMoniker Name="ModelRoot" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="true">
      <XmlSerializationBehaviorMoniker Name="EFModelSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="EF Model Diagrams">
      <ElementTool Name="ModelClass" ToolboxIcon="Resources\EntityGlyph.bmp" Caption="Entity" Tooltip="Create a persistent entity" HelpKeyword="ModelClassF1Keyword">
        <DomainClassMoniker Name="ModelClass" />
      </ElementTool>
      <ConnectionTool Name="UnidirectionalAssociation" ToolboxIcon="Resources\UnidirectionTool.bmp" Caption="Unidirectional Association" Tooltip="Create a link between two entities with a navigation property on one side only" HelpKeyword="ConnectUnidirectionalAssociationF1Keyword">
        <ConnectionBuilderMoniker Name="EFModel/UnidirectionalAssociationBuilder" />
      </ConnectionTool>
      <ConnectionTool Name="BidirectionalAssociation" ToolboxIcon="Resources\AssociationTool.bmp" Caption="Bidirectional Association" Tooltip="Create a link between two entities with navigation properties on both sides" HelpKeyword="ConnectBidirectionalAssociationF1Keyword">
        <ConnectionBuilderMoniker Name="EFModel/BidirectionalAssociationBuilder" />
      </ConnectionTool>
      <ConnectionTool Name="Generalization" ToolboxIcon="resources\generalizationtool.bmp" Caption="Inheritance" Tooltip="Create an inheritance relationship between two entities" HelpKeyword="GeneralizationF1Keyword" ReversesDirection="true">
        <ConnectionBuilderMoniker Name="EFModel/GeneralizationBuilder" />
      </ConnectionTool>
      <ElementTool Name="Comment" ToolboxIcon="resources\commenttool.bmp" Caption="Comment" Tooltip="Create a comment on this diagram" HelpKeyword="CommentF1Keyword">
        <DomainClassMoniker Name="Comment" />
      </ElementTool>
      <ConnectionTool Name="CommentLink" ToolboxIcon="resources\commentlinktool.bmp" Caption="Comment Link" Tooltip="Link a comment to an element" HelpKeyword="CommentsReferenceDesignElementsF1Keyword">
        <ConnectionBuilderMoniker Name="EFModel/CommentReferencesClassesBuilder" />
      </ConnectionTool>
      <ElementTool Name="Enumeration" ToolboxIcon="Resources\EnumTool.bmp" Caption="Enum" Tooltip="Creates an enumeration" HelpKeyword="ModelEnumF1Keyword">
        <DomainClassMoniker Name="ModelEnum" />
      </ElementTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="true" UsesSave="true" UsesCustom="true" UsesLoad="true" />
    <DiagramMoniker Name="EFModelDiagram" />
  </Designer>
  <Explorer ExplorerGuid="860f0cbe-0c84-4abe-8062-fa681f8038db" Title="Entity Model Explorer">
    <ExplorerBehaviorMoniker Name="EFModel/EFModelExplorer" />
  </Explorer>
</Dsl>