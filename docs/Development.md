# Contributing

Contributions are always welcome! [Clone a copy of the project](https://github.com/msawczyn/EFDesigner) and have at it!

I'll attempt to work any pull requests within a few days, but can't promise anything in
stone since life can, at times, get in the way &lt;g&gt;. 

You'll need to have installed the [Visual Studio SDK](http://go.microsoft.com/fwlink/?LinkID=185580) and the 
[Visualization & Modeling SDK](https://blogs.msdn.microsoft.com/devops/2016/12/12/the-visual-studio-modeling-sdk-is-now-available-with-visual-studio-2017/), 
formerly known as DSL Tools, which [can now be found in the Visual Studio installer](https://blogs.msdn.microsoft.com/devops/2016/12/12/the-visual-studio-modeling-sdk-is-now-available-with-visual-studio-2017/). 
Just make sure that's been activated. If you're new to the Modeling SDK, Microsoft has provided 
[some good code samples](https://code.msdn.microsoft.com/site/search?query=%22Modeling%20SDK%22&f%5B0%5D.Value=%22Modeling%20SDK%22&f%5B0%5D.Type=SearchText&ac=5) for you to look at.

In order to modify the attribute parser, you'll want to get the [Gold Parsing System](http://goldparser.org/), an excellent free
product for creating LALR parsers in C#. Documentation on the parsing engine is provided at the Gold site.

## Project Structure

Nothing unique about the DSL; you'll see the two standard projects (Dsl and DslPackage) along with
a Sandcastle project for the API help and a folder called `Metadata Parsing` that holds console-mode applications
for parsing assemblies in both .NET Core and .NET Framework and consuming EF6 and EFCore metadata. Those
are the parsers that you invoke when you drop an assembly onto the design surface for import.

In the Dsl and DslPackage projects, the `Custom Code` folder holds handwritten extensions, and the 
`Generated Code` folder the T4 outputs. `Custom Code` is further broken down into categories
of code, so finding things shouldn't be too difficult.

## Extending via MEF

Beginning with v1.1, the designer can be extended using Microsoft's [Managed Extensibility Framework](https://docs.microsoft.com/en-us/dotnet/framework/mef/index), 
so if you don't want to dig into the code and modify it directly, you can  
[write MEF extensions](https://docs.microsoft.com/en-us/visualstudio/modeling/extend-your-dsl-by-using-mef) 
to add your functionality.

## Things to note

Care has been taken to ensure the Dsl project is purely computational with no dependencies on 
any type of user interface. The DslPackage project is the bit that integrates into Visual Studio
and provides the interface. So far that hasn't led to any stilted code convolutions, so extenders
should think hard if they find a "need" to link UI code in the Dsl project. If you find yourself
wanting to do that, please post an inquiry in the Q&A section and we'll get more heads working on
the problem.

In the root of the code repository there is a `TODO.txt` file that lists the things I've thought about
doing but just haven't gotten around to. This would be a good place to start if you want to contribute
to the project. They're not necessarily easy enhancements, and likely not the most commonly seen
use cases, but certainly things that should be in the tool for completeness sake.

In addition, if there's something in the code that looks like it could be done better or more
efficiently, by all means make the change and put in a pull request. The best software is never written
in a vacuum.

Looking forward to your efforts! And thanks in advance.


