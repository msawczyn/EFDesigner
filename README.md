# Entity Framework Designer
An Entity Framework design surface and code generation for EF6, Core and beyond.

- [About](#about)
- [Installation](#installation)
- [Documentation](#documentation)
- [Development](#development)
- [License](#licence)

## About
This Visual Studio 2017 extension adds a new file type (.efmodel) that allows for visual design of persistent classes. Inheritance, unidirectional and bidirectional associations are supported.
Enumerations are also included in the visual model, as is the ability to add text blocks to explain potentially arcane
parts of your design.

The modeler was created to provide all the things that annoyed me for their not being there. Things like 
- the ability to show and hide parts of the model
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

Run the Sawczyn.EFDesigner.EFModel.DslPackage.vsix file to install the extension into Visual Studio. VS2017 is required, but any edition of VS2017 should work.

## Documentation

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


