# Entity Framework Designer

## Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond.

Model and generate code for Entity Framework v6, Entity Framework Core 2.0 and 2.1

**[Install with NuGet](https://docs.microsoft.com/en-us/visualstudio/ide/finding-and-using-visual-studio-extensions) from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner)**

**Complete documentation in the [project's documentation site](https://msawczyn.github.io/EFDesigner/)**

<table><tbody><tr><td>
<img src="https://raw.githubusercontent.com/wiki/msawczyn/EFDesigner/images/Designer.jpg">
</td></tr></tbody></table>

This Visual Studio 2017 extension adds a new file type (.efmodel) that allows for fast, easy and, most
importantly, **visual** design of persistent classes. Inheritance, unidirectional and bidirectional 
associations are all supported. Enumerations are also included in the visual model, as is the 
ability to add text blocks to explain potentially arcane parts of your design.

While giving you complete control over how the code is generated you'll be able to, out of the box,
create sophisticated, consistent and **correct** Entity Framework code that can be regenerated when 
your model changes. And, since the code is written using partial classes, any changes you make
to your generated code are retained across subsequent generations.

If you are used to the EF visual modeling that comes with Visual Studio, you'll be pretty much at home.
The goal was to duplicate at least those features and, in addition, 
add all the little things that *should* have been there. Things like: 
- the ability to show and hide parts of the model
- easy customization of generated output
- class and enumeration nodes that can be colored to visually group the model
- different concerns being generated into different subdirectories (entities, enums, dbcontext)
- entities by default generated as partial classes so the default code can be easily modified
- string length, index flags, required attributes and other properties being available in the designer

and many other nice-to-have bits.

Code generation is completely customizable via T4 templates. The tool installs templates that 
target both EF6 and EFCore, and generate both a code-first DbContext class and 
POCO entity classes. The EF6 template's DbContext code is written to allow consumption in 
ASP.Net Core in addition to any other project type, so you'll have flexibility in your development.

You can read more about how to use the designer in the [Documentation site](https://msawczyn.github.io/EFDesigner/).

 ### Change Log

***1.2.5.1***
   - Addressed [issue #20 - Abstract/inherited/TPC =code still there for abstract class](https://github.com/msawczyn/EFDesigner/issues/20). While the
     discussion centered around abstract classes and TPC inheritance (which was behaving properly), it did uncover a problem with code generation when
     namespaces changed from class to class. 
     
**1.2.5.0**
   - Fix for [issue #19 - Recognize "Id" as primary key on import](https://github.com/msawczyn/EFDesigner/issues/19)

<details>
<summary><b>1.2.4.0</b></summary>

   - Retargeted immediate error and warning messages to Visual Studio output window rather than error window so they could be cleared
   - Added drag validation to Generalization (inheritance) tool
   - Automatically propagate enum name and value name changes to classes that use them

</details>

<details>
<summary><b>1.2.3.3</b></summary>

   - Reverted the selection of the node in the model explorer when an element is selected in the diagram. Was causing bad user experience.
   - Fix for bad code generation when a class has multiple properties that each have an darabase index specified.

</details>

<details>
<summary><b>1.2.3</b></summary>

   - When element selected in model explorer, no longer highlights in orange but instead selects, centers and zooms the element.
     This was done because the color change flagged the model as modified, making the user either undo or save the changes to keep
     source control happy.
   - Selecting an element in the diagram also selects it in the model explorer
   - Fix for [issue #12 - Cascade delete](https://github.com/msawczyn/EFDesigner/issues/14). Added another enum value for delete behavior (now is Cascade, None and Default)
     and changed code generation to force no cascade delete if set to 'None' ('None' used to mean 'Use the default behavior', which is now, more explicitly, the 'Default'
     option).
   - Fix for [issue #13 - Unique index not generated in EF6](https://github.com/msawczyn/EFDesigner/issues/13).
   - Fix for [issue #14 - Table with two Primary keys not generated properly in context](https://github.com/msawczyn/EFDesigner/issues/14). Many thanks to @Falthazar!
   - Fix for [issue #18 - Adds ValueGeneratedNever if identity type is Manual](https://github.com/msawczyn/EFDesigner/pull/18). Again, hats off to @Falthazar!

</details>

<details>
<summary><b>1.2.2</b></summary>

- Fix issue with association role end changing without the other side autoatically changing
   - Fix issue with deleting a highlighted element throwing an error when trying to save the file
   - Fixed code generation for dependent classes
   - Designer now automatically saves before generating code

</details>

<details>
<summary><b>1.2.1</b></summary>

   - Bug fix for inheritance strategy automatically changing to table-per-hierarchy if targeting EF Core
   - Updated a few warning and error messages to make them more meaningful
   - Fixes for how dependent types work
   - Remove stale error and warnings prior to save (still a few left hanging around that need looked at)
   - Fixed a few null reference errors

</details>

<details>
<summary><b>1.2.0</b></summary>

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

</details>

<details>
<summary><b>1.1.0</b></summary>

   - Bug fixes for exceptions thrown when bad input to model attributes as text
   - Added MinLength string property (used in EF6 only as of this writing)
   - Modified attribute parser to accept MinLength
   - Added ColumnName property to model attribute
   - Added [MEF extension capability](https://docs.microsoft.com/en-us/visualstudio/modeling/extend-your-dsl-by-using-mef)
   - Added some unit tests
   - Added some documentation updates
   - Changed version to 1.1.0 due to MEF capability

</details>

<details>
<summary><b>1.0.3.9</b></summary>

   - If no entities and model is using an unsupported inheritance strategy, 
     changing from EF6 to EFCore doesn't give a message, just changes the strategy.
   - Added IsFlags attribute (and matching validations and behavior) to Enums
   - NGENed extension assembly

</details>

<details>
<summary><b>1.0.3.8</b></summary>

   - Fixed project item placement
   - Added change checks to diagram so dirty flag doesn't set when nothing changes

</details>

<details>
<summary><b>1.0.3.7</b></summary>

   - Emergency bug fixes

</details>

<details>
<summary><b>1.0.3.6</b></summary>

   - Fixed parser errors when editing model attributes as text
   - Fixed error when auto-generating on save and design surface is not the active window
   - Fixed crash when used on non-English-language systems (where Microsoft Pluralization Service is unavailable)
   - Added option to generate warnings if no documentation
   - Standardized warning and error message structure
   - Added ability to choose 'None' DatabaseInitializer type; generates SetInitializer(null)

</details>

<details>
<summary><b>1.0.3.5</b></summary>

   - Enhanced portability between EF6 an EFCore

</details>

<details>
<summary><b>1.0.3.4</b></summary>

   - Adds some T4 fixes to make generated code more usable in ASP.NET Core applications. 
   - Fix to spurious error when copying/pasting enum elements.
   - First release that's available on Visual Studio Marketplace.

</details>

<details>
<summary><b>1.0.3.3</b></summary>

   - Fix to spurious error when copying/pasting model elements
   - **Do not use this release.** Fix didn't extend to enum elements. This is fixed in 1.0.3.4.

</details>

<details>
<summary><b>1.0.3.2</b></summary>

   - Minor bug fix in parsing manually typed attributes. 
   - Loosened model file version check to only check major version.

</details>

<details>
<summary><b>1.0.3.0</b></summary>

   - Enhanced syntax for adding/editing attributes via code
   - Fix for generate-on-save for both Framework and .NET Core projects.

</details>

<details>
<summary><b>1.0.2.0</b></summary>

   - EFCore T4 template now available

</details>

<details>
<summary><b>1.0.1.0</b></summary>

   - Fix to EF6 T4 for issue where column names in many-to-many association join tables were flipped

</details>

<details>
<summary><b>1.0.0</b></summary>

   - Initial release

</details>


