<!--# Entity Framework Visual Designer

### Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond.
-->
This Visual Studio 2017 IDE extension adds a new file type (.efmodel) that allows for visual 
design of persistent classes. Inheritance, unidirectional and bidirectional associations are all 
supported. Enumerations are also included in the visual model, as is the ability to add text 
blocks to explain potentially arcane parts of your design. 

T4 files are used to generate code-first DBContext and POCO class files for both EF6 and EFCore, 
and these can be modified to generate your own particular style.

<table width="100%"><tr><td align="center" width="100%">
<img src="images/Designer.jpg" width="95%"/>
</td></tr></table>

If you are used to the Entity Framework visual modeling that came with prior versions of 
Visual Studio, you'll be pretty much at home. The goal was to duplicate at least those features 
and, in addition, all the things that should have been there. Things like
- the ability to show and hide parts of the model
- easy customization of generated output
- class nodes that can be colored to visually group the model
- different concerns that can be generated into different subdirectories (entities, enums, dbcontext)
- entities by default being generated as partials so the default code can be easily modified
- string length, index flags, required attributes and other properties being available in the designer
- plus many other tiny bits that kept getting under my skin.

You should think of this extension as two parts: first the design surface and, separately, the 
T4 templates that generate the code. The template(s) can be changed without modifying, compiling 
and reinstalling the extension -- it's designed this way so that developers can easily make 
whatever changes suit their fancy to how the code is generated, since everybody's got their own 
personal quirks on how their code should look.

One of my biggest annoyances with EF has been the completely disparate approaches in code-first, 
model-first and database-first. There is no logical reason why these approaches couldn't center around a common, easily used core. [Simon Hughes](https://github.com/sjh37) has been doing a great job with his [EntityFramework-Reverse-POCO-Code-First-Generator](https://github.com/sjh37/EntityFramework-Reverse-POCO-Code-First-Generator) project, and this extension aims to continue that momentum with visual modeling, centering everything on what's been commonly called "code-first" EF. Because, frankly, the conceptual schema definition language (CSDL), store schema definition language (SSDL), and mapping specification language (MSL) are implementation details that a good tool should just plain abstract away.

This documentation is a work-in-progress, so suggestions are welcome. The tool has been used in 
production (i.e., generating code for real projects) for quite a while, but more eyes and different 
approaches are always good.

## Quick Start

- Install the plugin by either downloading the VSIX file in the /dist directory or, better, using NuGet to pull it from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner). The NuGet approach will help you keep it up to date when enhancements and bug fixes are made.
- Create a project in Visual Studio. Any project type _except_ for ASP.NET Core -- that's got limited support for the kinds of items you can add to the project, and data items aren't yet available. If you haven't already done so, use NuGet to add Entity Framework or Entity Framework Core to the project (since the code you'll generate references it and, besides, it doesn't make much sense to create EF code without using EF, right?).
- Right-click the project root and select _Add/New Item..._ from the dropdown menu.
- Under _Visual C# Items_ in the tree to the left you'll see a _Data_ folder. Select that folder and then select _EFDesigner_ in the list on the right. 
- Rename the file to your liking. The name of the file will become the default name of the generated DbContext, though you can change that later.
- Click Add.
- Opening the _[your file name].efmodel_ file will present you with a blank design surface, and your toolbox will display the items you can add to your model.
- Read the rest of this site to get details on things you don't find obvious :-).

### Next Step 
[Getting Started](Getting-Started)
