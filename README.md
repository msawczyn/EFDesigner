# Entity Framework Designer
Entity Framework visual design surface and code-first code generation for EF6, Core and beyond.

- [About](#about)
- [Installation](#installation)
- [Documentation](#documentation)
  - [Templates](#templates)
  - [Designer](#designer)
    - [Entities](#entity)
    - [Associations](#associations)
    - [Inheritance](#inheritance)
    - [Comments](#comments)
    - [Enums](#enums)
- [Development](#development)
- [License](#licence)

## About
This Visual Studio 2017 extension adds a new file type (.efmodel) that allows for visual design of persistent classes. Inheritance, unidirectional and bidirectional associations are supported.
Enumerations are also included in the visual model, as is the ability to add text blocks to explain potentially arcane
parts of your design.

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

Code generation is completely customizable via T4 templates. Included is a template that targets EF6 and generate code-first fluent API for the DbContext, and POCO classes for the modeled classes.
Also included is a T4 for EFCore, but it's not currently active since I haven't had the chance to get my head around the differences yet (lightly veiled hint for someone to step in and flesh it out).

One of my biggest annoyances with EF has been the completely disparate approaches in code-first, model-first and database-first. There is no logical reason why these approaches couldn't center
around a common, easily used core. [Simon Hughes](#https://github.com/sjh37) has been doing a great job with his [EntityFramework-Reverse-POCO-Code-First-Generator](#https://github.com/sjh37/EntityFramework-Reverse-POCO-Code-First-Generator) project,
and this extension aims to continue that momentum with visual modeling, centering everything on what's been commonly called "code-first" EF. 
Because, frankly, the CSDL, SSDL and MSL are implementation details that a good tool should abstract away.

This README is going to take a while to finish, but I wanted to get the project out so folks could kick the tires and let me
know what needs fixing. It's been used in production (i.e., generating code for real projects) for quite a while, but more eyes and different approaches are always good.

## Installation

Run the `Sawczyn.EFDesigner.EFModel.DslPackage.vsix` file to install the extension into Visual Studio. VS2017 is required, but any edition of VS2017 should work.

## Documentation

You should think of this extension as two parts: one is the modeling surface and, separately, the T4 template that generates the code. The template(s) can be modified
without modifying, compiling and reinstalling the extension -- it's designed this way so that the user can make whatever changes suit their fancy (since
everybody's got their own personal quirks on how they want their code to look).

### Templates

The first place you'll want to look if you want to change how code is generated. If you haven't wrapped your head around T4
yet, there are a lot of great resources (too numerous to list) on the web that can get you started.

Four templates are installed with the extension:

- `VSIntegration.ttinclude`
  
  A T4 library that contains useful methods for interrogating the VStudio environment. Methods are fairly self-explanatory, and are:
  * `public EnvDTE.DTE GetDTE()`
  * `public EnvDTE.Solution GetSolution()`
  * `public IEnumerable<EnvDTE.Project> RecurseSolutionFolder(EnvDTE.Project project)`
  * `public IEnumerable<EnvDTE.Project> GetAllProjects()`
  * `public EnvDTE.Project GetCurrentProject()`
  * `public string GetProjectPath(EnvDTE.Project project)`
  * `public EnvDTE.ProjectItem GetDirectoryItem(string target)`

- `MultipleOutputHelper.ttinclude`

  A T4 library (that [first appeared in 2009](#https://damieng.com/blog/2009/01/22/multiple-outputs-from-t4-made-easy) and has been enhanced ever since) for generating multiple files
  from a T4 rather than the default single file it wants to create (although the default is created as well ... it's just empty). Documentation 
  at http://damieng.com/blog/2009/11/06/multiple-outputs-from-t4-made-easy-revisited

- `EF6Designer.ttinclude` and `EFCoreDesigner.ttinclude`

  The T4 libraries responsible for generating Entity Framework 6 and Entity Framework Core code. 
  Rather than being written in classic T4 fashion (where the bulk of the file is a template with replacement variables), this
  is written as a library of methods, taking the procedural approach. IMHO, this is _much_ simpler to 
  read and work with, even with VStudio extensions that give you T4 syntax highlighting.

  **_Note:_** The `EFCoreDesigner.ttinclude` file is, as of this writing, empty. It's a work in progress and, if anyone
  would like to help write it, please feel free to submit a pull request :-).

  Looking at EF6Designer.ttinclude, you'll find the entry point to this library as the first method in the file -- `void GenerateEF6(Manager manager, ModelRoot modelRoot)`.
  It's called from the custom T4 template that you get when you add an entity model to your project (`<modelname>.tt`), allowing the
  project's T4 to be short and generic. It can also be merged into the project's T4 as a starting point
  to modify code generation. To easily do this:
  1. Remove lines 7 and 8 from the .tt file. These read
       ```c#
      #><#@ include file="EF6Designer.ttinclude"
      #><#@ include file="EFCoreDesigner.ttinclude"
      ```
  1. Right before the ending `#>` tag in your custom .tt file, paste the contents of EF6Designer.ttinclude 
  starting at the definition of the first method and ending right before the `#>`
  tag at the bottom of the file. You should have something that looks like the following:
      ```c#
     <#@ template inherits="Microsoft.VisualStudio.TextTemplating.VSHost.ModelingTextTransformation" 
                  hostSpecific="true" language="C#" compilerOptions="/langversion:6"#>
     <#@ assembly name="System.Data.Entity.Design.dll" 
     #><#@ assembly name="EnvDTE"
     #><#@ include file="MultipleOutputHelper.ttinclude"
     #><#@ include file="VSIntegration.ttinclude"
     #><#@ output extension=".cs" 
     #><#@ import namespace="System.Linq" 
     #><#@ import namespace="System" 
     #><#@ import namespace="System.Data.Entity.Design.PluralizationServices" 
     #><#@ import namespace="System.Globalization" 
     #><#@ import namespace="Sawczyn.EFDesigner.EFModel" 
     #><#@ EFModel processor="EFModelDirectiveProcessor" requires="FileName='YourModelName.efmodel'"  
     #><#
        Manager manager = Manager.Create(Host, GenerationEnvironment);
        manager.FileNameMarker = ModelRoot.FileNameMarker;

        if (ModelRoot.Types.OfType<ModelClass>().Any())
        {
           manager.StartHeader(false);
           Output("//------------------------------------------------------------------------------");
           Output("// <auto-generated>");
           Output("//     This code was generated from a template.");
           Output("//");
           Output("//     Manual changes to this file may cause unexpected behavior in your application.");
           Output("//     Manual changes to this file will be overwritten if the code is regenerated.");
           Output("// </auto-generated>");
           Output("//------------------------------------------------------------------------------");
           NL();

           switch (ModelRoot.EntityFrameworkVersion)
           {
              case EFVersion.EF6:
                 GenerateEF6(manager, this.ModelRoot);
                 break;
              case EFVersion.EFCore:
                 break;
           }

        }

        manager.Process(true);

        void GenerateEF6(Manager manager, ModelRoot modelRoot)
        {
           // Entities

           foreach (ModelClass modelClass in modelRoot.Types.OfType<ModelClass>())
           {
              manager.StartNewFile(Path.Combine(modelRoot.EntityOutputDirectory, $"{modelClass.Name}.{modelRoot.FileNameMarker}.cs"));
        
        // etc.

      #>
      ``` 
   You can tidy it up farther by removing the switch statement and replacing that directly with the call to `GenerateEF6` -- the switch
   statement is there so we're ready for `EFCoreDesigner.ttinclude` to be finished.

### Designer

The design surface is where you'll create the visual model of your persistent entities. The properties
you give them here will (or should, if you make changes to the T4 templates) drive the code generation.

When the designer is active, the VStudio Toolbox will display various items that can be added to the designer.

![toolbox](https://raw.githubusercontent.com/msawczyn/EFDesigner/master/docs/Toolbox.jpg)

| Tool                       | Description                                          |
| :------------------------- | :--------------------------------------------------- |
| Entity                     | A persistent class to be generated |
| Unidirectional Association | A navigation property that only goes one way. The starting class will have a property of the type of the ending class (or a collection of them, depending on cardinality), but not the other way around. |
| Bidirectional Association  | A navigation property that will create properties in classes on both ends. |
| Inheritance                | Standard .NET inheritance (and its associated constraints). Start the line at the derived class and point to bhe base class |
| Comment                    | Add a comment box anywhere on the model |
| Comment Link               | Link an existing comment to an Entity or Enum. Not required since comments can be free-floating, but nice to have if the comment refers to something specific on the diagram. |
| Enum                       | An enumeration to be generated |

#### Entities

Coming soon

#### Associations (navigation properties)

Coming soon

##### Unidirectional Associations

Coming soon

##### Bidirectional Associations

Coming soon

#### Inheritance

Coming soon

#### Comments

Coming soon

#### Comment Links

Coming soon

#### Enums

Coming soon

## Development

Coming soon too, but not as soon as Documentation

## License

MIT License

Copyright (c) 2017 Michael Sawczyn

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


