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

## Special Note

There's currently a bug in the templates used in DSL generation in the SDK. You'll need to fix it to make
things work.

First, find the DSL Tools text templates. In my installation, they're located at

```
C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\Microsoft\DSL SDK\DSL Designer\15.0\TextTemplates
```

But the `Enterprise` part of that path is there because I'm running Visual Studio Enterprise ... ymmv.

In the `Dsl` folder you'll find `SerializationHelper.tt`. Open it in your favorite text editor.

On line 617 you'll see

```
this.OnPostLoadModel(serializationResult, partition, fileName, modelRoot);
```

change that to

```
this.OnPostLoadModel(serializationResult, partition, location, modelRoot);
```

The bug got introduced when the load process got expanded to load from a stream rather than from
just a file. The parameter name of the surrounding function got changed, but this call didn't and,
from the looks of it, test code coverage never caused this to get used in generation. 

You'll have to make this change every time the SDK gets updated. It was [reported in March of 2017](https://developercommunity.visualstudio.com/content/problem/37185/vs2017-dsl-tools-error-in-serializationhelpertt.html)
and [again in October of the same year](https://developercommunity.visualstudio.com/content/problem/128359/dslmodeling-error-in-generatedcode-of-serializatio.html) 
but is still a problem.

If you have *other* any problems compiling the code, please add an issue to the 
[GitHub issues list](https://github.com/msawczyn/EFDesigner/issues).

And if you'd like to contribute but aren't a programmer, there's still room for you!
The project docs (the thing you're reading) could certainly be improved - it would be
great if qualified writers could help.

## Project Structure

Nothing unique about the DSL; you'll see the two standard projects (Dsl and DslPackage) along with
a Dsl.Tests project for (a handful of) unit tests.

In the Dsl project, the `Custom Code` folder holds handwritten extensions, and the 
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


