<!--# Entity Framework Visual Designer

### Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond.
-->

<table width="100%"><tr><td align="left" width="100%" style="background-color: lightyellow">
Please be aware that this document is currently being updated for v2.0. 
Information here is a mix between 2.0 features and the older 1.3x version.<br/>
Updates are in progress and, when done, this message will be removed.
</td></tr></table>

This Visual Studio 2019 IDE extension adds a new file type (.efmodel) that allows for visual 
design of persistent classes. Inheritance, unidirectional and bidirectional associations are all 
supported. Enumerations are also included in the visual model, as is the ability to add text 
blocks to explain potentially arcane parts of your design. 

[T4 files](https://docs.microsoft.com/en-us/visualstudio/modeling/writing-a-t4-text-template?view=vs-2019) are used to generate code-first DBContext and POCO class files for both **EF6 _and_ EFCore**, 
and these can be modified to generate your own particular style.

<table width="100%"><tr><td align="center" width="100%">
<img src="images/Designer.jpg" width="95%"/>
</td></tr></table>

If you are used to the Entity Framework visual modeling that came with prior versions of 
Visual Studio, you'll be pretty much at home. The goal was to duplicate at least those features 
and, in addition, all the things that should have been there. Things like

*   importing entities from C# source, or existing DbContext definitions (including their entities) from compiled EF6 or EFCore assemblies
*   the ability to show and hide parts of the model
*   easy customization of generated output by editing or even replacing the T4 templates
*   entities by default generated as partial classes so the generated code can be easily extended
*   class and enumeration nodes that can be colored to visually group the model
*   different concerns being generated into different subdirectories (entities, enums, dbcontext)
*   string length, index flags, required attributes and other properties being available in the designer

You should think of this extension as two parts: first the design surface and, separately, the 
T4 templates that generate the code. The template(s) can be changed without modifying, compiling 
and reinstalling the extension - it's designed this way so that developers can easily make 
whatever changes suit their fancy to how the code is generated, since everybody's got their own 
personal quirks on how their code should look.

### Next Step 
[Getting Started](Getting-Started)
