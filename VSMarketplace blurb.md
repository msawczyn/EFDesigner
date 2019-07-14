This Visual Studio 2017/2019 extension adds a new file type (.efmodel) that allows for fast, easy and, most importantly, **visual** design 
of persistent classes. Inheritance, unidirectional and bidirectional associations are all supported. Enumerations are also included in 
the visual model, as is the ability to add text blocks to explain potentially arcane parts of your design.

<img src="https://msawczyn.github.io/EFDesigner/images/Designer.jpg">

While giving you complete control over how the code is generated you'll be able to, out of the box, create sophisticated, 
consistent and **correct** Entity Framework code that can be regenerated when your model changes. And, since the code is written using 
partial classes, any additions you make to your generated code is retained across subsequent generations.

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

In Visual Studio 2019, projects using the new project format (typically .NET Core and .NET Standard projects) will throw an exception when
opening an .efmodel file. This is an issue related to the Visual Studio 16 SDK and is currently under investigation. Visual Studio
2017 does not exhibit this issue.

If this is important to you, you can follow it at [developercommunity.visualstudio.com](https://developercommunity.visualstudio.com/content/problem/539313/microsoftvisualstudioprojectsystemvsimplementation.html).

**Note:** We've [gotten confirmation](https://github.com/msawczyn/EFDesigner/issues/66#issuecomment-506878246) that the problem has been fixed in VS 2019 V16.2 Preview 3.

**ChangeLog**

**1.3.0.5**
   - Added a model fixup for when user doesn't use full enumeration name for a property's initial value in an entity

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

**1.2.7.2**
   - **[NEW]** Added additional types of UInt16, UInt32, UInt64 and SByte to property type list
   - **[NEW]** Added the ability to use a modeled enumeration, if it has a proper backing type, as an entity identifier
   - **[NEW]** Added DateTime.UtcNow as a valid initial value for a DateTime property
   - Fix for "One-to-one relation in EFCore" (See https://github.com/msawczyn/EFDesigner/issues/71)
   - Remove default DbContext constructor in EFCore to allow support for AddDbContextPool calls in ConfigureServices (See https://github.com/msawczyn/EFDesigner/issues/72)

**1.2.7.1**
   - Works with Visual Studio 2019 - mostly (see Known Issues)
   - Better formatting for XML comment docs
   - **[NEW]** Added autoproperty toggle for association ends, allowing for implementation of partial methods to examine and/or override association getting and setting
   - Removed from T4 template the experimental method added in 1.2.6.22 that generated orphaned association cleanup in EF6. The experiment failed :-(
   - Documentation enhancements
   - Change in generated code to eliminate name clashes in certain circumstances (See https://github.com/msawczyn/EFDesigner/issues/48)
   - Fix for duplicate indices being created for key fields
   - Fix for "Setting different value than default produces duplicated HasColumnType call in EF Core" (See https://github.com/msawczyn/EFDesigner/issues/58). Thanks to tdabek (https://github.com/tdabek) for the PR!
   - Fix for "Defining ColumnType causes error in generated DBContext" (See https://github.com/msawczyn/EFDesigner/issues/64)
   - Fix for "EFCore indexed column not generated and support for multi column indexing" (See https://github.com/msawczyn/EFDesigner/issues/62)
   - Fix for "One-to-one seems to generate incorrect code" (See https://github.com/msawczyn/EFDesigner/issues/60)
   - Fix for "Error generating column type" (See https://github.com/msawczyn/EFDesigner/issues/58)

[Earlier changes](https://github.com/msawczyn/EFDesigner/blob/master/changelog.txt)

