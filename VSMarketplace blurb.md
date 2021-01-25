This Visual Studio 2019 extension is the easiest way to add a consistently correct Entity Framework model to your project with support for EF6, EFCore2, EFCore3 and EFCore5.

It's an opinionated code generator, adding a new file type (.efmodel) that allows for fast, easy and, most importantly, **visual** design of persistent classes. Inheritance, unidirectional and bidirectional associations are all supported. Enumerations are also included in the visual model, as is the ability to add text blocks to explain potentially arcane parts of your design.

<img src="https://msawczyn.github.io/EFDesigner/images/Designer.jpg">

While giving you complete control over how the code is generated you'll be able to create, out of the box, sophisticated, consistent and **correct** Entity Framework code that can be regenerated when your model changes. And, since the code is written using partial classes, any additions you make to your generated code are retained across subsequent generations.
The designer doesn't need to be present to use the code that's generated - it generates standard C#, using the code-first, fluent API - so the tool doesn't become a dependency to your project.

If you are used to the EF visual modeling that comes with Visual Studio, you'll be pretty much at home. The goal was to duplicate at least those features and, in addition, add all the little things that _should_ have been there. Things like:

*   importing entities from C# source, or existing DbContext definitions (including their entities) from compiled EF6 or EFCore assemblies
*   multiple views of your model to highlight important aspects of your design
*   the ability to show and hide parts of the model
*   easy customization of generated output by editing or even replacing the T4 templates
*   entities by default generated as partial classes so the generated code can be easily extended
*   class and enumeration nodes that can be colored to visually group the model
*   different concerns being generated into different subdirectories (entities, enums, dbcontext)
*   string length, index flags, required attributes and other properties being available in the designer

and many other nice-to-have bits.

For comprehensive documentation, please visit [the project's documentation site](https://msawczyn.github.io/EFDesigner/).

**ChangeLog**

**3.0.3**
   - **[NEW]** Added VS UML icon for model file in solution explorer (thanks to https://github.com/dcastenholz for the change)
   - **[NEW]** Classes with custom interfaces can now display an indicator with a tooltip indicating the interface type(s). This glyph is enabled/disabled at model level.
   - **[NEW]** Added ability to specify that an association should be automatically included in any queries that use it (EFCore5 only). The association connector will appear bolder if at least one end is auto-included.
   - **[NEW]** Updated association tooltip to indicate which, if any, end is auto-included
   - Fix to ensure database collation overrides don't get applied to the wrong column types
   - Fix to allow 1..1 association to owned types in EFCore5 (see https://github.com/msawczyn/EFDesigner/issues/252)
   - Fix to calculate EF version number correctly when "Latest" was specified in designer (see https://github.com/msawczyn/EFDesigner/issues/254)
   - Fix to generate correct DeleteBehavior enum values in EFCore < v3 (see https://github.com/msawczyn/EFDesigner/issues/257)
   - Removed `INotifyPropertyChanged` option from designer. Implementers wanting this interface can add it to a partial class file as any other interface, as there's really nothing special about it.
   - Generated code now honors the `ExcludeFromMigration` setting for a class

**3.0.2**
   - **[NEW]** Added setting on designer surface to set visibility defaults for entity default constructors, and overrides for that setting on the entities
   - **[NEW]** Added `public bool ModelAttribute.IsForeignKeyProperty` for use by developers doing custom code generation from the model
   - **[NEW]** Added option for turning off DbSet and table name pluralization (see https://github.com/msawczyn/EFDesigner/issues/246)
   - **[NEW]** Added option for how to name foreign key shadow properties - either with or without underscores (see https://github.com/msawczyn/EFDesigner/issues/250)
   - **[NEW]** Added option to generate DbContextFactory class, for use in context pooling. Asking for DbContext factory method generation disables generating OnCreating method, since they don't play well together.
   - Changed property editor for custom attributes to be multiline, to ease editing (see https://github.com/msawczyn/EFDesigner/issues/251)
   - Fixes for code generation of new EFCore5 database collation options
   - Fixed condition where sometimes generated code in entity default constructors would create infinitely recursive calls
   - Stopped escaping standard XML comment tags in summary and description fields (see https://github.com/msawczyn/EFDesigner/issues/248)
   - Due to the new seeding needs in EFCore5, setters for identity properties are now public even if set to be auto-generated

**3.0.1**
   - **[NEW]** Added [Description] attribute (to classes, properties, enums and enum values where summary was non-blank) to facilitate tooling use
   - Fixed an issue where EFCore5 code was generating cascade delete commands in the wrong place (see https://github.com/msawczyn/EFDesigner/issues/243)
   - Editing class properties and enum values as text now retains properties that aren't available in the text syntax (see https://github.com/msawczyn/EFDesigner/issues/242)

**3.0**
   - **[NEW]** Now supports EFCore5.X 
      - **[NEW]** Added System.Net.IPAddress and System.Net.NetworkInformation.PhysicalAddress to the list of available property types
      - **[NEW]** Added ability to specify both default database collation and a collation override at the property level 
      - **[NEW]** Many-to-many bidirectional associations are now allowed 
      - **[NEW]** Any property type can now be used as an identity 
      - **[NEW]** Can now customize backing field names for non-AutoProperty properties 
      - **[NEW]** Properties with backing fields (i.e., non-AutoProperty properties) can now choose how EF will read/write those values (see https://docs.microsoft.com/en-us/ef/core/modeling/backing-field).
      - **[NEW]** Added support for keyless entity types created by defining queries
      - **[NEW]** Added support for keyless entity types coming from database views
   - Enhancements and Fixes
      - **[NEW]** Added ability to globally add and remove exposed foreign key properties to all modeled entities (via menu command) (see https://github.com/msawczyn/EFDesigner/issues/223)
      - **[NEW]** Added ability to choose to place newly imported model elements on the diagram where they were dropped. Caution: this can be EXTREMELY slow for large imports. (see https://github.com/msawczyn/EFDesigner/issues/225)
      - **[NEW]** Added composition and aggregation indicators to association connectors
      - Default code generation type is now the latest version of EFCore (currently, 5.0)
      - Fixed inability to paste enumerations using diagram copy/paste
      - Changing an identity property's type now changes the type of any defined foreign-key properties pointing to that identity property
      - Title text color didn't always change when class/enum fill color changed in the diagram
      - Selecting tabs or spaces for indentation in generated code has been moved to a property on the designer surface.
      - Added ModelRoot.IsEFCore5Plus convenience property. It can be used in custom T4 edits
   - Possibly breaking changes
      - T4 template structure has been changed drastically to simplify managing code generation for the various EF versions.
        If customized T4 templates have been added to a project, they'll still work, but enhancements will continue to be made only to the new, more 
        object-oriented, T4 structure. Updating the model's .tt file to use the new template structure is quite simple; details will be in the documentation 
        at https://msawczyn.github.io/EFDesigner/Customizing.html

[Earlier changes](https://github.com/msawczyn/EFDesigner/blob/master/changelog.txt)

A big thanks to <a href="https://www.jetbrains.com/?from=EFDesigner"><img src="https://msawczyn.github.io/EFDesigner/images/jetbrains-variant-2a.png" style="margin-bottom: -30px"></a> &nbsp; for providing free development tools to support this project.
