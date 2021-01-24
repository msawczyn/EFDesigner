# Entity Framework Designer

## Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond.

Model and generate code for both Entity Framework v6.x and Entity Framework Core 2.x, 3.x and 5.

**[Install with NuGet](https://docs.microsoft.com/en-us/visualstudio/ide/finding-and-using-visual-studio-extensions) from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner)**

**Complete documentation in the [project's documentation site](https://msawczyn.github.io/EFDesigner/)**

<table><tbody><tr><td>
<img src="https://msawczyn.github.io/EFDesigner/images/Designer.jpg">
</td></tr></tbody></table>

This Visual Studio 2019 extension is the easiest way to add a consistently correct Entity Framework (EF6 or EFCore) model to your project. 

It's an opinionated code generator, adding a new file type (.efmodel) that allows for fast, easy and, most importantly, **visual** design 
of persistent classes. Inheritance, unidirectional and bidirectional associations are all supported. Enumerations are also included in 
the visual model, as is the ability to add text blocks to explain potentially arcane parts of your design.

While giving you complete control over how the code is generated you'll be able to create, out of the box, sophisticated, 
consistent and **correct** Entity Framework code that can be regenerated when your model changes. And, since the code is written using 
partial classes, any additions you make to your generated code are retained across subsequent generations.
The designer doesn't need to be present to use the code that's generated - it generates standard C#, using the code-first, fluent API - so the tool doesn't
become a dependency to your project.

If you are used to the EF visual modeling that comes with Visual Studio, you'll be pretty much at home.
The goal was to duplicate at least those features and, in addition, 
add all the little things that *should* have been there. Things like: 
*   importing entities from C# source, or existing DbContext definitions (including their entities) from compiled EF6 or EFCore assemblies
*   multiple views of your model to highlight important aspects of your design
*   the ability to show and hide parts of the model
*   easy customization of generated output by editing or even replacing the T4 templates
*   entities by default generated as partial classes so the generated code can be easily extended
*   class and enumeration nodes that can be colored to visually group the model
*   different concerns being generated into different subdirectories (entities, enums, dbcontext)
*   string length, index flags, required attributes and other properties being available in the designer

and many other nice-to-have bits.

Code generation is completely customizable via T4 templates. The tool installs templates that 
target both EF6 and EFCore, and generate both a code-first DbContext class and 
POCO entity classes. The EF6 template's DbContext code is written to allow consumption in 
ASP.Net Core in addition to any other project type, so you'll have flexibility in your development.

You can read more about how to use the designer in the [Documentation site](https://msawczyn.github.io/EFDesigner/).

### Shout out and a big thanks
<!-- <table border="0" cellspacing="0" cellpadding="0" style="border:none">
<tr vstyle="center" style="border:none"><td>to</td><td><a href="https://www.jetbrains.com/?from=EFDesigner"><img src="https://msawczyn.github.io/EFDesigner/images/jetbrains-variant-2a.png"></a></td><td>for providing free development tools to support this project.</td></tr>
</table> -->
to <a href="https://www.jetbrains.com/?from=EFDesigner"><img src="https://msawczyn.github.io/EFDesigner/images/jetbrains-variant-2a.png"></a> for providing free development tools to support this project.

### Change Log

**3.0.3**
   - Added VS UML icon for model file in solution explorer (thanks to https://github.com/dcastenholz for the change)
   - Classes with custom interfaces can now display an indicator with a tooltip indicating the interface type(s). This glyph is enabled/disabled at model level.
   - Added ability to specify that an association should be automatically included in any queries that use it (EFCore5 only). The association connector will appear bolder if at least one end is auto-included.
   - Updated association tooltip to indicate which, if any, end is auto-included
   - Fix to ensure database collation overrides don't get applied to the wrong column types
   - Fix to allow 1..1 association to owned types in EFCore5 (see https://github.com/msawczyn/EFDesigner/issues/252)
   - Fix to calculate EF version number correctly when "Latest" was specified in designer (see https://github.com/msawczyn/EFDesigner/issues/254)
   - Fix to generate correct DeleteBehavior enum values in EFCore < v3 (see https://github.com/msawczyn/EFDesigner/issues/257)
   - Removed INotifyPropertyChanged option from designer. Implementers wanting this interface can add it to a partial class file as any other interface, as there's really nothing special about it.
   - Generated code now honors the ExcludeFromMigration setting for a class

<details>
<summary><b>3.0.2</b></summary>

   - Added setting on designer surface to set visibility defaults for entity default constructors, and overrides for that setting on the entities
   - Added `public bool ModelAttribute.IsForeignKeyProperty` for use by developers doing custom code generation from the model
   - Added option for turning off DbSet and table name pluralization (see https://github.com/msawczyn/EFDesigner/issues/246)
   - Added option for how to name foreign key shadow properties - either with or without underscores (see https://github.com/msawczyn/EFDesigner/issues/250)
   - Added option to generate DbContextFactory class, for use in context pooling. Asking for DbContext factory method generation disables generating OnCreating method, since they don't play well together.
   - Changed property editor for custom attributes to be multiline, to ease editing (see https://github.com/msawczyn/EFDesigner/issues/251)
   - Fixes for code generation of new EFCore5 database collation options
   - Fixed condition where sometimes generated code in entity default constructors would create infinitely recursive calls
   - Stopped escaping standard XML comment tags in summary and description fields (see https://github.com/msawczyn/EFDesigner/issues/248)
   - Due to the new seeding needs in EFCore5, setters for identity properties are now public even if set to be auto-generated
</details>

<details>
<summary><b>3.0.1</b></summary>

   - Added [Description] attribute (to classes, properties, enums and enum values where summary was non-blank) to facilitate tooling use
   - Fixed an issue where EFCore5 code was generating cascade delete commands in the wrong place (see https://github.com/msawczyn/EFDesigner/issues/243)
   - Editing class properties and enum values as text now retains properties that aren't available in the text syntax (see https://github.com/msawczyn/EFDesigner/issues/242)

</details>

<details>
<summary><b>3.0</b></summary>

   - Now supports EFCore5.X 
      - Added `System.Net.IPAddress` and `System.Net.NetworkInformation.PhysicalAddress` to the list of available property types
      - Added ability to specify both default database collation and a collation override at the property level 
      - Many-to-many bidirectional associations are now allowed 
      - Any property type can now be used as an identity 
      - Can now customize backing field names for non-AutoProperty properties 
      - Properties with backing fields (i.e., non-AutoProperty properties) can now choose how EF will read/write those values (see https://docs.microsoft.com/en-us/ef/core/modeling/backing-field).
      - Added support for keyless entity types created by defining queries
      - Added support for keyless entity types coming from database views
   - Default code generation type is now the latest version of EFCore (currently, 5.0)
   - Added ability to globally add and remove exposed foreign key properties to all modeled entities (via menu command) (see https://github.com/msawczyn/EFDesigner/issues/223)
   - Added ability to choose to place newly imported model elements on the diagram where they were dropped. Caution: this can be EXTREMELY slow for large imports. (see https://github.com/msawczyn/EFDesigner/issues/225)
   - Added composition and aggregation indicators to association connectors
   - Fixed inability to paste enumerations using diagram copy/paste
   - Changing an identity property's type now changes the type of any defined foreign-key properties pointing to that identity property
   - Title text color didn't always change when class/enum fill color changed in the diagram
   - Selecting tabs or spaces for indentation in generated code has been moved to a property on the designer surface.
   - Added `ModelRoot.IsEFCore5Plus` convenience property. It can be used in custom T4 edits
   - Possibly breaking changes: 
      - T4 template structure has been changed drastically to simplify managing code generation for the various EF versions.
        If customized T4 templates have been added to a project, they'll still work, but enhancements will continue to be made only to the new, more 
        object-oriented, T4 structure. Updating the model's .tt file to use the new template structure is quite simple; details will be in the documentation 
        at https://msawczyn.github.io/EFDesigner/Customizing.html

</details>

<details>
<summary><b>2.0.5.7</b></summary>

   - Added ability to select tabs or spaces for indentation in generated code (Tools/Options/Entity Framework Visual Editor/Visual Editor Options) (See https://github.com/msawczyn/EFDesigner/issues/221)
   - Fixed an issue with changing visual grid size on design surface.

</details>

<details>
<summary><b>2.0.5.6</b></summary>

   - The project item templates for the model file had wandered away. They're back again. (See https://github.com/msawczyn/EFDesigner/issues/216)
   - Fixed a problem with existing models where class-level "AutoProperty: false" caused bad code generation. (See https://github.com/msawczyn/EFDesigner/issues/215)

</details>

<details>
<summary><b>2.0.5.5</b></summary>

   - Fix: Foreign key crashes when reference is on the derived table (See https://github.com/msawczyn/EFDesigner/issues/212)
   - Fixed a edge condition where an error would be thrown when deleting an association

</details>

<details>
<summary><b>2.0.5.3</b></summary>

   - Provide option to save diagrams as uncompressed XML to facilitate version control (in Tools/Options/Entity Framework Visual Editor)
   - Enhanced error reporting for assembly import errors
   - Assembly import can now process assemblies with more than one DbContext class
   - Cleaned up some ambiguities in how copy/paste was handled with multiple diagrams
   - Performance improvements

</details>

<details>
<summary><b>2.0.4.1</b></summary>

   - Added ability to hide foreign key property names on association connectors in diagrams
   - Attribute glyphs (except for Warning glyphs) in diagrams are now reflected in Model Explorer
   - Sped up reverse engineering a compiled assembly. As a consequence, the diagram is no longer updated when the assembly is imported (but that tended to ruin the diagram anyway)
   - Fix: Self-associations didn't appear when existing class is added to new diagram from the Model Explorer
   - Fix: Diagram no longer loses focus after its saved
   - Fix: Errors when copy/paste between diagrams in same model
   - Fix: Generalization links weren't being handled property when reverse engineering a compiled assembly
   - Fix: Under certain circumstances, declared foreign keys could erroneously be created for EF6 1-1 relationships. EF6 doesn't support this.
   - Fix: ensure glyphs in association compartments are visible
   - Fix: overly-aggressive pruning in foreign keys

</details>

<details>
<summary><b>2.0.3</b></summary>

   - Added ability to hide foreign key property names on association connectors in diagrams
   - Attribute glyphs (except for Warning glyphs) in diagrams are now reflected in Model Explorer
   - Fix: Self-associations didn't appear when existing class is added to new diagram from the Model Explorer
   - Fix: Diagram no longer loses focus after its saved
   - Fix: Errors when copy/paste between diagrams in same model

</details>

<details>
<summary><b>2.02</b></summary>

   - Added count of elements in model explorer tree
   - Added ability to search the model explorer for class and attribute names
   - Comments can also be hidden like classes and enumerations
   - Fix: Changing String Column Name Clears Max Length Property (See https://github.com/msawczyn/EFDesigner/issues/173)
   - Fix: Dropping external files creates elements but not shapes on diagram (See https://github.com/msawczyn/EFDesigner/issues/150)
   - Fix: VS Crash (See https://github.com/msawczyn/EFDesigner/issues/177)
   - Fix: Attributes only show up on diagram where they were added (See https://github.com/msawczyn/EFDesigner/issues/179)
   - Fix: EFDesigner 2.0.1 won't create entities in the efmodel for existing poco .cs files (See https://github.com/msawczyn/EFDesigner/issues/182)

</details>

<details>
<summary><b>2.0.0</b></summary>

   - **Dropped support for Visual Studio 2017**; was getting to be too much to keep the tool viable for that Visual Studio version.
   - It's now possible to have multiple diagrams for the same model, each showing a different view and synchronized as the model changes. Perfect for helping to understand large models.
   - Added ability to specify foreign key properties (See https://github.com/msawczyn/EFDesigner/issues/55)
   - Foreign key properties have a unique glyph so they can be easily picked out of the crowd
   - Foreign key properties that are primary keys also have a unique but different glyph
   - Completely restructured assembly parsers; they now cleanly handle all valid combinations of EF6/EFCore2/EFCore3 and .NETCore2/.NETCore3/.NETFramework
   - Modified assembly parsers to find declared foreign keys and add them to the model appropriately
   - Added options dialog (Tools/Options/Entity Framework Visual Editor)
   - Added use of GraphViz for model layout (if installed and path is added to "Tools/Options/Entity Framework Visual Editor")
   - Added switch to disable generation of classes and enumerations for those cases where they are coming from different assemblies but need to be in the model to avoid errors
   - Added visual indicator on classes and enumerations where code generation is disabled
   - Added ability to override the base class of the generated DbContext to be something other than "DbContext"
   - Join tables in many-to-many associations can now have custom names (EF6 only, until EFCore supports many-to-many cardinalities)
   - Removing an enumeration removes all entity properties that use that enumeration, after displaying a warning.
   - Designer has optional visual grid with color, size and snap-to-grid options available
   - Renamed toolbox category to "EF Model Diagrams"
   - Enhanced display of model elements in the Visual Studio property window's object list
   - Removed MSAGL layouts. No one understood them anyway.
   - Removed tool automatically installing NuGet packages. Too volatile.
   - Fix: OutputDirectory lost on reload (See https://github.com/msawczyn/EFDesigner/issues/144)
   - Fix: Unidirectional Many-to-One Association missing Required (See https://github.com/msawczyn/EFDesigner/issues/145)
   - Fix: Couldn't delete property initial value for Enum values (See https://github.com/msawczyn/EFDesigner/issues/148)
   - Fix: Support for Empty / blank "File Name Marker" (See https://github.com/msawczyn/EFDesigner/issues/149)
   - Fix: Now escaping XML comment text properly
   - Fix: Issue with GeographyPoint: System.Data.Entities.Spatial not found for .Net Core 3.1 (See https://github.com/msawczyn/EFDesigner/issues/159)
   - Fix: HasDefaultSchema doesn't work with MySql (See https://github.com/msawczyn/EFDesigner/issues/160)

</details>

<details>
<summary><b>1.3.0.12</b></summary>

   - **Last version supporting Visual Studio 2017**
   - Fix: Compilation Error after Upgrading to v1.3.0.11 (See https://github.com/msawczyn/EFDesigner/issues/129)
   - Fix: Designer Drag/Drop Interpreter fails (See https://github.com/msawczyn/EFDesigner/issues/128 and https://github.com/msawczyn/EFDesigner/issues/132)
   - Fix: Enumerations generated into entity directory rather than enumeration directory

</details>

<details>
<summary><b>1.3.0.11</b></summary>

   - Removed default checks in constructors for scalars

</details>

<details>
<summary><b>1.3.0.10</b></summary>

   - Build for VS2017 support

</details>

<details>
<summary><b>1.3.0.9</b></summary>

   - Fix: backing fields caused duplicate database columns (See https://github.com/msawczyn/EFDesigner/issues/101)
   - Fix: bad merge broke MaxLength and MinLength properties in entity string properties (See https://github.com/msawczyn/EFDesigner/issues/103)
   - Fix: attribute parser ("edit as code" feature) didn't handle enumeration initial values well; it does now (See https://github.com/msawczyn/EFDesigner/issues/104)
   - Fix: showing cascade delete in the designer worked inconsistently (See https://github.com/msawczyn/EFDesigner/issues/108)
   - Fix: drag and drop developed regressions (See https://github.com/msawczyn/EFDesigner/issues/112 and https://github.com/msawczyn/EFDesigner/issues/114)
   - Fix: undo threw null reference errors when undoing drag and drop from code files (See https://github.com/msawczyn/EFDesigner/issues/113)
   - Fix: 'KeyBuilder' does not contain definition for 'Ignore' in EF Core (See https://github.com/msawczyn/EFDesigner/issues/115)
   - Fix: Identity properties ignore Setter Visibility setting (See https://github.com/msawczyn/EFDesigner/issues/118)
   - Changed string MaxLength to differentiate between undefined and max length (See https://github.com/msawczyn/EFDesigner/issues/118)
   - Restructured display of namespaces and output directories in the designer's property window
   - Removed visibility option for setters of automatic identity properties. 

</details>

<details>
<summary><b>1.3.0.7</b></summary>

   - Fix: bad merge broke MaxLength and MinLength properties in entity string properties (See https://github.com/msawczyn/EFDesigner/issues/103)
   - Fix: backing fields caused duplicate database columns (See https://github.com/msawczyn/EFDesigner/issues/101)

</details>

<details>
<summary><b>1.3.0.6</b></summary>

   - Added a model fixup for when user doesn't use full enumeration name for a property's initial value in an entity (See https://github.com/msawczyn/EFDesigner/issues/82)
   - To more fully support DDD models, added a toggle for persisting either the property or its backing field (if not an autoproperty) for EFCore
   - Can now override the NotifyPropertyChanged value for an entity on a per-property and per-association basis
   - Fix: Removed stray quote marks in default values for string properties (See https://github.com/msawczyn/EFDesigner/issues/86)
   - Fix: Minimum string length was ignored when setting properties via text edit (See https://github.com/msawczyn/EFDesigner/issues/86)
   - Fix: Required string identity property is not present in the constructor (See https://github.com/msawczyn/EFDesigner/issues/93)
   - Fix: Some issues with owned entities in EFCore
   - Fix: If NotifyPropertyChanged is active, wrong Output is generated (See https://github.com/msawczyn/EFDesigner/issues/97)
   - For folks wanting to read and/or modify the source for this tool, added a readme on how to deal with tracking properties

</details>

<details>
<summary><b>1.3.0.4</b></summary>

   - Fixed problematic code generation in constructors for classes having 1..1 associations (See https://github.com/msawczyn/EFDesigner/issues/74)
   - Fixed problem where database was always generating identity values, regardless of setting in the model (See https://github.com/msawczyn/EFDesigner/issues/79)
   - Fixed errors when creating nested project folders (See https://github.com/msawczyn/EFDesigner/issues/77)
   - Fixed cascade delete errors in EFCore when overriding cascade behavior (See https://github.com/msawczyn/EFDesigner/issues/76)
   - Added more information in headers for generated code (tool version, URLs, license info)

</details>

<details>
<summary><b>1.3.0.2</b></summary>

   - Fixed error found in some VS2017 installations preventing running due to dependency problems

</details>

<details>
<summary><b>1.3.0.1</b></summary>

   - Enhanced source code drag/drop to handle bidirectional associations and enumerations better.
   - Can now import assemblies containing DbContext classes. Dropping a compiled assembly onto the design surface will attempt to process and merge it into the design.
   - Added ability to merge two unidirectional associations into one bidirectional association (via context menu action)
   - Added ability to split a bidirectional association to two unidirectional associations (via context menu action)
   - Added [Microsoft Automatic Graph Layout](https://github.com/Microsoft/automatic-graph-layout), giving the user the ability to choose the diagram's auto-layout strategy 

</details>

<details>
<summary><b>Earlier</b></summary>

**1.2.7.2**
   - Added additional types of UInt16, UInt32, UInt64 and SByte to property type list
   - Added the ability to use a modeled enumeration, if it has a proper backing type, as an entity identifier
   - Added DateTime.UtcNow as a valid initial value for a DateTime property
   - Fix: "One-to-one relation in EFCore" (See https://github.com/msawczyn/EFDesigner/issues/71)
   - Remove default DbContext constructor in EFCore to allow support for AddDbContextPool calls in ConfigureServices (See https://github.com/msawczyn/EFDesigner/issues/72)

**1.2.7.1**
   - Works with Visual Studio 2019 - mostly (see Known Issues, above)
   - Better formatting for XML comment docs
   - Added autoproperty toggle for association ends, allowing for implementation of partial methods to examine and/or override association getting and setting
   - Removed experimental method added in 1.2.6.22 for generation of orphan association cleanup in EF6. The experiment failed :-(
   - Documentation enhancements
   - Change in generated code to eliminate name clashes in certain circumstances (See https://github.com/msawczyn/EFDesigner/issues/48)
   - Fix: Removed duplicate indices being created for key fields
   - Fix: "Setting different value than default produces duplicated HasColumnType call in EF Core" (See https://github.com/msawczyn/EFDesigner/issues/58). Thanks to tdabek (https://github.com/tdabek) for the PR!
   - Fix: "Defining ColumnType causes error in generated DBContext" (See https://github.com/msawczyn/EFDesigner/issues/64)
   - Fix: "EFCore indexed column not generated and support for multi column indexing" (See https://github.com/msawczyn/EFDesigner/issues/62)
   - Fix: "One-to-one seems to generate incorrect code" (See https://github.com/msawczyn/EFDesigner/issues/60)
   - Fix: "Error generating column type" (See https://github.com/msawczyn/EFDesigner/issues/58)

**1.2.6.25**
   - Fix for duplicate associations when `Implement Notify` is true

**1.2.6.24**
   - Fix for join table schema generation in certain scenarios (EF6)
   - Fix for regression error producing code gen errors in EFCore navigation properties

**1.2.6.23**
   - Fix for designer item not showing in Add Items dialog

**1.2.6.22**
   - **[NEW]** Added code in EF6 templates to generate orphan cleanup (experimental)
   - Fix for 1..1 and 0-1..0-1 associations in EF Core generated code
   - Entity constructor parameters normalized to help in JSON serialization/deserialization

**1.2.6.21**
   - Generation of column type overrides now generates valid override code in OnModelCreating
   - DbSet properties in DbContext generate as virtual to facilitate mocking

**1.2.6.20**
   - Fixed code generation issue where class and enum directory overrides were being ignored (See https://github.com/msawczyn/EFDesigner/issues/36)
   - Fixed a problem that caused a hard crash when certain model properties were changed under certain conditions (See https://github.com/msawczyn/EFDesigner/issues/38)
   - Removed visibility of source and target roles for all but 1-1 and 0..1-0..1 associations; they can't be changed anyway (See https://github.com/msawczyn/EFDesigner/issues/40)
   - **[NEW]** Added Display Text property to generate [Display(Name="<text>")] for attributes, enum values and navigation properties 
   - **[NEW]** Added ability to specify custom attributes for classes, attributes, enums, enum values and navigation properties

**1.2.6.18**
   - Fixed issue #35, *Concurrency mode: optimistic auto generated Timestamp property* (See https://github.com/msawczyn/EFDesigner/issues/35)
   - Fixed issue #33, *Concurrency mode: optimistic* (See https://github.com/msawczyn/EFDesigner/issues/33)
   - **[NEW]** Added the base class as a property in the property editor to allow for easily adding/removing inheritance relationships for multiple classes

**1.2.6.13**
   - Bugfix to remove unnecessary permission requests to push attributes down when deleting leaf nodes in an inheritance tree
   - Fix to workaround Visual Studio pulling in the wrong System.Net.Http reference. (See https://developercommunity.visualstudio.com/content/problem/296293/vs2017-1575-ignores-the-hintpath-and-take-the-syst.html)
   - Add EFModel.xsd to Visual Studio schema cache in order to avoid editor warnings for missing schema
   - Fixed template issue for non-English-language systems (where Microsoft Pluralization Service is unavailable)
   - Added compartment for association sources so Bidirectional associations would show up

**1.2.6.11**
   - Tweak to force association end roles to be correct when roles or multiplicities change
   - Attribute elements' "String Properties" don't appear unless the attribute is a string
   - Attribute elements' "Indexed Unique" property doesn't appear unless the attribute has "Indexed" equal to "True"
   - Fixed background color on attribute glyph in model explorer
   - Fixed foreground color on enum value glyph on design surface
   - Hid comments in model explorer because they just cluttered up the tree.
   - **[NEW]** Associations now show up in their own compartment in a class on the design surface. Note that this changes the height of your elements, so the first time opening a model you may have to tweak your esthetics a bit.
   - **[NEW]** Double-clicking a class or enum on the designer opens the generated code file, if it exists. If it doesn't exist, you're asked if you'd like to generate the model then, if you do, it tries again.
   - Known issue: EFCore projects won't ask to generate the code if they can't open the file; they just fail silently. 

**1.2.6.7**
   - An entity's concurrency token property is no longer a required parameter in its constructor (https://github.com/msawczyn/EFDesigner/issues/24)
   - Simplified cascade delete settings in property editor for associations
   - Fixed bad code generation in EFCore for cascade delete overrides (https://github.com/msawczyn/EFDesigner/issues/22)
   - Missing files when generating code for .NET Core projects fixed
   - Tightened up and swatted some bugs in INotifyPropertyChanged handling. Added documentation to doc site for this feature (following up on https://github.com/msawczyn/EFDesigner/issues/23)
   - Ensured multiline editing was available in property window for those properties that made sense

**1.2.6.6**
   - **[NEW]** Deleting a generalization or superclass gives the choice of pushing attributes and associations down to the former child class(es)

**1.2.6.5**
   - Comment elements now wrap the text
   - Multiline editor available in property window for element comment descriptions and Comment element text
   - Xml format changed for .efmodel file - can't be loaded by any version < 1.2.6.3
   - Support for automatic migration to new model xml formats

**1.2.6.2**
   - Added XML docs to DbContext, DatabaseInitializer and DbMigrationsConfiguration
   - **[NEW]** Enabled drag and drop reordering of enum values and class properties
   - Gave some color to the enum value glyph in the model explorer - it was so boring!
   - **[NEW]** Class properties and enum values with warnings now show a warning icon on the design surface
   - **[NEW]** Design surface has a property to turn on or off the display of the warning icons 
   - Recategorized a few "Misc" properties on the design surface

**1.2.5.1**
   - Addressed [issue #20 - Abstract/inherited/TPC code still there for abstract class](https://github.com/msawczyn/EFDesigner/issues/20). While the discussion centered around abstract classes and TPC inheritance (which was behaving properly), it did uncover a problem with code generation when namespaces changed from class to class. 

**1.2.5.0**
   - Fix for [issue #19 - Recognize "Id" as primary key on import](https://github.com/msawczyn/EFDesigner/issues/19)

**1.2.4.0**
   - Retargeted immediate error and warning messages to Visual Studio output window rather than error window so they could be cleared
   - Added drag validation to Generalization (inheritance) tool
   - Automatically propagate enum name and value name changes to classes that use them

**1.2.3.3**
   - Reverted the selection of the node in the model explorer when an element is selected in the diagram. Was causing bad user experience.
   - Fix for bad code generation when a class has multiple properties that each have an darabase index specified.

**1.2.3.0**
   - **[NEW]** When element selected in model explorer, no longer highlights in orange but instead selects, centers and zooms the element.
     This was done because the color change flagged the model as modified, making the user either undo or save the changes to keep
     source control happy.
   - **[NEW]** Selecting an element in the diagram also selects it in the model explorer
   - Fix for [issue #12 - Cascade delete](https://github.com/msawczyn/EFDesigner/issues/14). Added another enum value for delete behavior (now is Cascade, None and Default)
     and changed code generation to force no cascade delete if set to 'None' ('None' used to mean 'Use the default behavior', which is now, more explicitly, the 'Default'
     option).
   - Fix for [issue #13 - Unique index not generated in EF6](https://github.com/msawczyn/EFDesigner/issues/13).
   - Fix for [issue #14 - Table with two Primary keys not generated properly in context](https://github.com/msawczyn/EFDesigner/issues/14). Many thanks to @Falthazar!
   - Fix for [issue #18 - Adds ValueGeneratedNever if identity type is Manual](https://github.com/msawczyn/EFDesigner/pull/18). Again, hats off to @Falthazar!

**1.2.2.0**
   - Fix issue with association role end changing without the other side autoatically changing
   - Fix issue with deleting a highlighted element throwing an error when trying to save the file
   - Fixed code generation for dependent classes
   - **[NEW]** Designer now automatically saves before generating code

**1.2.1.0**
   - Bug fix for inheritance strategy automatically changing to table-per-hierarchy if targeting EF Core
   - Updated a few warning and error messages to make them more meaningful
   - Fixes for how dependent types work
   - Remove stale error and warnings prior to save (still a few left hanging around that need looked at)
   - Fixed a few null reference errors

**1.2.0.0**
   - **New Features**
      - Roslyn-based code analysis now allows dragging C# files onto the design surface to add or update classes and enums
      - Can add `INotifyPropertyChanged` interface and implementation for entities
      - Ability to tag model as a specific EF version (especially useful for EF Core as new capabilities are being added often)
      - Support for dependent (complex/owned) types 
      - Option to generate dependent types in a separate directory
      - Output directory overrides for classes and enums
      - On model save, can optionally automatically install EF NuGet packages for the model's EF type and version
      - Context menu action to expand and collapse selected classes and enums 
   - **Enhancements**
      - Added ability to add/edit enum values via text in the same way properties can be added/edited in classes
      - Property grid hides element properties if they're not appropriate for the EF version
      - Inheritance strategy automatically changes to table-per-hierarchy if targeting EF Core
      - Context property `Database Type` changed to `SqlServer Type` to better reflect what it does
      - Selecting an element in the Model Explorer highlights it on the diagram

**1.1.0.0**
   - Bug fixes for exceptions thrown when bad input to model attributes as text
   - **[NEW]** Added MinLength string property (used in EF6 only as of this writing)
   - Modified attribute parser to accept MinLength
   - **[NEW]** Added ColumnName property to model attribute
   - **[NEW]** Added [MEF extension capability](https://docs.microsoft.com/en-us/visualstudio/modeling/extend-your-dsl-by-using-mef)
   - Added some unit tests
   - Added some documentation updates
   - Changed version to 1.1.0 due to MEF capability

**1.0.3.9**
   - If no entities and model is using an unsupported inheritance strategy, 
     changing from EF6 to EFCore doesn't give a message, just changes the strategy.
   - **[NEW]** Added IsFlags attribute (and matching validations and behavior) to Enums
   - NGENed extension assembly

**1.0.3.8**
   - Fixed project item placement
   - Added change checks to diagram so dirty flag doesn't set when nothing changes

**1.0.3.7**
   - Emergency bug fixes

**1.0.3.6**
   - Fixed parser errors when editing model attributes as text
   - Fixed error when auto-generating on save and design surface is not the active window
   - Fixed crash when used on non-English-language systems (where Microsoft Pluralization Service is unavailable)
   - **[NEW]** Added option to generate warnings if no documentation
   - Standardized warning and error message structure
   - Added ability to choose 'None' DatabaseInitializer type; generates SetInitializer(null)

**1.0.3.5**
   - Enhanced portability between EF6 an EFCore

**1.0.3.4**
   - Adds some T4 fixes to make generated code more usable in ASP.NET Core applications. 
   - Fix to spurious error when copying/pasting enum elements.
   - **[NEW]** First release that's available on Visual Studio Marketplace.

**1.0.3.3**
   - Fix to spurious error when copying/pasting model elements
   - **Do not use this release.** Fix didn't extend to enum elements. This is fixed in 1.0.3.4.

**1.0.3.2**
   - Minor bug fix in parsing manually typed attributes. 
   - Loosened model file version check to only check major version.

**1.0.3.0**
   - Enhanced syntax for adding/editing attributes via code
   - Fix for generate-on-save for both Framework and .NET Core projects.

**1.0.2.0**
   - **[NEW]** EFCore T4 template now available

**1.0.1.0**
   - Fix to EF6 T4 for issue where column names in many-to-many association join tables were flipped

**1.0.0.0**
   - Initial release

</details>

