# Customizing Code Generation

Easy code customization was a design goal with this tool. There are two levels, 
and you can choose which one is best depending on your needs.

## Partial Classes

Easiest is to simply extend the custom code by implementing partial classes. Use these
to add any methods to the generated classes you need; they'll be untouched if the model is 
later regenerated.

The generated code also creates partial methods at locations that have historically shown themselves to be common
places for extension. Those are:
<ul>
<li>Your DbContext-derived class (CustomInit, OnModelCreatingImpl, and OnModelCreatedImpl)</li>
<li>Each persistent entity (Init)</li>
</ul>

Remember that partial methods, if not implemented, don't generate IL and therefore don't have
any performance overhead, so they're an excellent means of optional behavior injection.

## Replacing the T4 Templates

You can easily replace the T4 templates installed with the extension either globally or on a project-by-project basis.
The trick (thanks Microsoft!) is finding them.

When extensions are installed into Visual Studio, they're copied to 

```
C:\Users\[User Name]\AppData\Local\Microsoft\VisualStudio\15.0\Extensions
```

The "15.0" part may have some hexadecimal gibberish appended to it if you've installed more than one flavor of
Visual Studio 2017 (Community, Enterprise, etc.) since it needs to keep the various
settings separate. If you don't know which one you need, check the shortcut you use to start
Visual Studio.

The fun begins when you get to the Extensions directory ... each extension is installed in a 
unique folder with a random (pseudo-random?) name. The easiest thing to do is to hunt for <code>EFDesigner.ttinclude</code>
starting in the Extensions folder.

When you find it, you'll see five .ttinclude files there:
<table>
<thead>
<tr><th valign="top"><b>File</b></th><th valign="top"><b>Description</b></th></tr>
</thead>
<tbody>
<tr><td valign="top"><code style="background-color: transparent;">EFDesigner.ttinclude</code></td><td valign="top">Code useful to all flavors of Entity Framework. Also contains code to write the entities and enumerations.</td></tr>
<tr><td valign="top"><code style="background-color: transparent;">EF6Designer.ttinclude</code></td><td valign="top">Writes the EF6 DbContext, database initializer and migration configuration. Any code that only works with EF6 is defined here. </td></tr>
<tr><td valign="top"><code style="background-color: transparent;">EFCoreDesigner.ttinclude</code></td><td valign="top">Same as above, but for EF Core instead.</td></tr>
<tr><td valign="top"><code style="background-color: transparent;">MultipleOutputHelper.ttinclude</code></td><td valign="top">Code to create more than one output file, since T4 usually doesn't do that. You probably won't mess with this.</td></tr>
<tr><td valign="top"><code style="background-color: transparent;">VSIntegration.ttinclude</code></td><td valign="top">Code to talk to Visual Studio so we can add project items, etc. You probably won't mess with this either.</td></tr>
</tbody>
</table>

Decide what you want to replace and copy the file into your project directory. Modify at will ...
the templating engine will find your local copy before it looks anywhere else.

For code generation from the T4, the flow of events starts at the [Model Name].tt file in your
project. Its template directives tell it to include the five templates listed above and then,
based on the Entity Framework type specified, call one of two entry points.

You'll notice that these aren't your standard T4 templates, where the bulk of the file is text
to be outpout. Rather, they're like scripting libraries. That's on purpose -- a design goal was
to allow the generation to be customizable but, at the same time, understandable to the average
programmer. A group of function calls is easier to read, IMHO, than your every-day T4 template,
so that's what we have.

The templating engine works through the model, pulling together all the code in memory. It isn't
until the call to `manager.Process(true)` at the end that the files are generated. So any customizations
to the process should come before that call.

### Next Step 
[Contributing](Development)
