This Visual Studio 2019 extension is an opinionated code generator, adding a new file type (.efmodel) that allows for fast, easy and, most importantly, **visual** design 
of persistent classes. Inheritance, unidirectional and bidirectional associations are all supported. Enumerations are also included in 
the visual model, as is the ability to add text blocks to explain potentially arcane parts of your design.

<img src="https://msawczyn.github.io/EFDesigner/images/Designer.jpg">

While giving you complete control over how the code is generated you'll be able to create, out of the box, sophisticated, 
consistent and **correct** Entity Framework code that can be regenerated when your model changes. And, since the code is written using 
partial classes, any additions you make to your generated code are retained across subsequent generations.

If you are used to the EF visual modeling that comes with Visual Studio, you'll be pretty much at home. The goal was to duplicate 
at least those features and, in addition, add all the little things that _should_ have been there. Things like:

*   importing entities from C# source, or existing DbContext definitions (including their entities) from compiled EF6 or EFCore assemblies
*   the ability to show and hide parts of the model
*   easy customization of generated output by editing or even replacing the T4 templates
*   entities by default generated as partial classes so the generated code can be easily extended
*   class and enumeration nodes that can be colored to visually group the model
*   different concerns being generated into different subdirectories (entities, enums, dbcontext)
*   string length, index flags, required attributes and other properties being available in the designer

and many other nice-to-have bits.

For comprehensive documentation, please visit [the project's documentation site](https://msawczyn.github.io/EFDesigner/).

**Known Issues**

**Visual Studio 2019 v16.2.0 currently breaks the designer** -- you're not able to draw connections between
classes, enums, structs and comment blocks. [It was reported to Microsoft](https://developercommunity.visualstudio.com/content/problem/660095/dsl-tools-broken-in-1620-preview-4.html), 
and has since been **fixed as of v16.2.5**, so if you're using a version between 16.2.0 and 16.2.4, you'll want 
to upgrade to 16.2.5 or later in order to use not just this extension, but any extension based on the Microsoft Modeling SDK.

**ChangeLog**

**2.0.0.0** (next version)
   - **Dropped support for Visual Studio 2017 due to the changes in .NET Core 3.0 and above**
   - **[NEW]** Added ability to specify foreign key properties
   - **[NEW]** Modified assembly parsers to find declared foreign keys and add them to the model appropriately
   - Renamed toolbox category to "EF Model Diagrams"
   - **[NEW]** Added options dialog (Tools/Options/Entity Framework Visual Editor)
   - **[NEW]** Added use of GraphViz for model layout (if installed and path is added to "Tools/Options/Entity Framework Visual Editor")

**1.3.0.12** 
   - Fix: Compilation Error after Upgrading to v1.3.0.11 (See https://github.com/msawczyn/EFDesigner/issues/129)
   - Fix: Designer Drag/Drop Interpreter fails (See https://github.com/msawczyn/EFDesigner/issues/128 and https://github.com/msawczyn/EFDesigner/issues/132)
   - Fix: Enumerations generated into entity directory rather than enumeration directory

**1.3.0.11**
   - Removed default checks in constructors for scalars

**1.3.0.10**
   - Build for VS2017 support

**1.3.0.9**
   - Fix: backing fields caused duplicate database columns (See https://github.com/msawczyn/EFDesigner/issues/101)
   - Fix: bad merge broke MaxLength and MinLength properties in entity string properties (See https://github.com/msawczyn/EFDesigner/issues/103)
   - Fix: attribute parser ("edit as code" feature) didn't handle enumeration initial values well; it does now (See https://github.com/msawczyn/EFDesigner/issues/104)
   - Fix: showing cascade delete in the designer worked inconsistently (See https://github.com/msawczyn/EFDesigner/issues/108)
   - Fix: drag and drop developed regressions (See https://github.com/msawczyn/EFDesigner/issues/112 and https://github.com/msawczyn/EFDesigner/issues/114)
   - Fix: undo threw null reference errors when undoing drag and drop from code files (See https://github.com/msawczyn/EFDesigner/issues/113)
   - Fix: 'KeyBuilder' does not contain definition for 'Ignore' in EF Core (See https://github.com/msawczyn/EFDesigner/issues/115)
   - Fix: Identity properties ignore Setter Visibility setting (See https://github.com/msawczyn/EFDesigner/issues/118)
   - **[NEW]** Changed string MaxLength to differentiate between undefined and max length (See https://github.com/msawczyn/EFDesigner/issues/118)
   - Restructured display of namespaces and output directories in the designer's property window
   - Removed visibility option for setters of automatic identity properties. 

**1.3.0.7**
   - Fix: bad merge broke MaxLength and MinLength properties in entity string properties (See https://github.com/msawczyn/EFDesigner/issues/103)
   - Fix: backing fields caused duplicate database columns (See https://github.com/msawczyn/EFDesigner/issues/101)

**1.3.0.6**
   - Added a model fixup for when user doesn't use full enumeration name for a property's initial value in an entity (See https://github.com/msawczyn/EFDesigner/issues/82)
   - **[NEW]** To more fully support DDD models, added a toggle for persisting either the property or its backing field (if not an autoproperty) for EFCore
   - **[NEW]** Can now override the NotifyPropertyChanged value for an entity on a per-property and per-association basis
   - Fix: Removed stray quote marks in default values for string properties (See https://github.com/msawczyn/EFDesigner/issues/86)
   - Fix: Minimum string length was ignored when setting properties via text edit (See https://github.com/msawczyn/EFDesigner/issues/86)
   - Fix: Required string identity property is not present in the constructor (See https://github.com/msawczyn/EFDesigner/issues/93)
   - Fix: Some issues with owned entities in EFCore
   - Fix: If NotifyPropertyChanged is active, wrong Output is generated (See https://github.com/msawczyn/EFDesigner/issues/97)
   - For folks wanting to read and/or modify the source for this tool, added a readme on how to deal with tracking properties

**1.3.0.4**
   - Fixed problematic code generation in constructors for classes having 1..1 associations (See https://github.com/msawczyn/EFDesigner/issues/74)
   - Fixed problem where database was always generating identity values, regardless of setting in the model (See https://github.com/msawczyn/EFDesigner/issues/79)
   - Fixed errors when creating nested project folders (See https://github.com/msawczyn/EFDesigner/issues/77)
   - Fixed cascade delete errors in EFCore when overriding cascade behavior (See https://github.com/msawczyn/EFDesigner/issues/76)
   - Added more information in headers for generated code (tool version, URLs, license info)

**1.3.0.2**
   - Fixed error found in some VS2017 installations preventing running due to dependency problems

**1.3.0.1**
   - Enhanced source code drag/drop to handle bidirectional associations and enumerations better.
   - **[NEW]** Can now import assemblies containing DbContext classes. Dropping a compiled assembly onto the design surface will attempt to process and merge it into the design.
   - **[NEW]** Added ability to merge two unidirectional associations into one bidirectional association (via context menu action)
   - **[NEW]** Added ability to split a bidirectional association to two unidirectional associations (via context menu action)
   - **[NEW]** Added [Microsoft Automatic Graph Layout](https://github.com/Microsoft/automatic-graph-layout), giving the user the ability to choose the diagram's auto-layout strategy 

[Earlier changes](https://github.com/msawczyn/EFDesigner/blob/master/changelog.txt)

A big thanks to <a href="https://www.jetbrains.com/?from=EFDesigner"><img src="https://msawczyn.github.io/EFDesigner/images/jetbrains-variant-2a.png" style="margin-bottom: -30px"></a> &nbsp; for providing free development tools to support this project.
