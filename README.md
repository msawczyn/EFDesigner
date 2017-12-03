# Entity Framework Designer

## Entity Framework visual design surface and code-first code generation for EF6, Core and beyond.

This Visual Studio 2017 extension adds a new file type (.efmodel) that allows for visual design of persistent classes. Inheritance, unidirectional and bidirectional associations are supported. Enumerations are also included in the visual model, as is the ability to add text blocks to explain potentially arcane parts of your design.

If you are used to the EF visual modeling that comes with VStudio, you'll be pretty much at home.
The goal was to duplicate at least those features and, in addition, 
all the things that annoyed me for their not being there. Things like 
- the ability to show and hide parts of the model
- easy customization of generated output
- class nodes can be colored to visually group the model
- different concerns can be generated into different subdirectories (entities, enums, dbcontext)
- entities by default are generated as partials so the default code can be easily modified
- string length, index flags, required attributes and other properties available in the designer

and many other tiny bits that kept getting under my skin.

Code generation is completely customizable via T4 templates. Included are templates that target both EF6 and EFCore, and generate a code-first DbContext and POCO classes for the modeled classes.

You can read more about how to use the designer in the [Wiki](https://github.com/msawczyn/EFDesigner/wiki).

