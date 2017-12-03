# Entity Framework Designer

## Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond.

This Visual Studio 2017 extension adds a new file type (.efmodel) that allows for fast, easy and, most
importantly, **visual** design of persistent classes. Inheritance, unidirectional and bidirectional 
associations are all supported. Enumerations are also included in the visual model, as is the 
ability to add text blocks to explain potentially arcane parts of your design.

While giving you complete control over how the code is generated you'll be able to, out of the box,
create sophisticated, consistent and **correct** Entity Framework code that can be regenerated when 
your model changes. And, since the code is written using partial classes, any additions you make
to your generated code is retained across subsequent generations.

If you are used to the EF visual modeling that comes with VStudio, you'll be pretty much at home.
The goal was to duplicate at least those features and, in addition, 
add all the little things that *should* have been there. Things like: 
- the ability to show and hide parts of the model
- easy customization of generated output
- class and enumeration nodes that can be colored to visually group the model
- different concerns being generated into different subdirectories (entities, enums, dbcontext)
- entities by default generated as partial classes so the default code can be easily modified
- string length, index flags, required attributes and other properties being available in the designer

and [many other nice-to-have bits](https://github.com/msawczyn/EFDesigner/wiki/Using-the-designer).

Code generation is completely customizable via T4 templates. The tool installs templates that 
target both EF6 and EFCore, and generate both a code-first DbContext class and 
POCO entity classes. The EF6 template's DbContext code is written to allow consumption in both
ASP.Net Core as well as any other project type, so you'll have flexibility in your development.

You can read more about how to use the designer in the [Wiki](https://github.com/msawczyn/EFDesigner/wiki).

