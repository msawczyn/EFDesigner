# Customizing Code Generation

Easy code customization was a design goal with this tool. There are two levels, 
and you can choose which one is best depending on your needs.

## Partial Classes

Easiest is to simply extend the custom code by implementing partial classes. Use these
to add any methods to the generated classes you need; they'll be untouched if the model is 
later regenerated.

The generated code also creates partial methods at locations that have historically shown themselves to be common
places for extension. Those are:

   - **Your DbContext-derived class**
     - `CustomInit` - Runs in the constructor of your database context. This is called in each constructor, so should hold common setup code for your context no matter how it's constructed.
     - `OnModelCreatingImpl` - Runs before the code that creates the model in memory.
     - `OnModelCreatedImpl` - Runs after the code that creates the model in memory.
     - `OnBeforeChangesSaved` - Runs before `SaveChanges` processing. Put code here that does common things you want done before any objects are saved (update timestamps, etc. )
     - `OnAfterChangesSaved` - Runs after `SaveChanges` processing. Put code here that does common things you want done after objects are saved. Note that new objects will have ID values at this point, if you have database-generated IDs in play.
   - **Each persistent entity**
     - `Init` - Runs in the object's constructor. Here is where you can add custom constructor behavior, since the constructor itself is generated to ensure the proper containers are created, required parameters are validated, etc.

Note that, if you typically override `SaveChanges` to do custom processing, that override is already generated for you and does some work. 
If this doesn't suit you, you can always put a copy of the appropriate `.ttinclude` file into your project and remove that bit. 

Remember that partial methods, if not implemented, don't generate IL and therefore don't have
any performance overhead, so they're an excellent means of optional behavior injection.

## Replacing the T4 Templates

You can easily replace the T4 templates installed with the extension either globally or on a project-by-project basis.
The trick (thanks Microsoft!) is finding them.

The designer is installed in the all-user extension location, typically

```
C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions
```

(Note that my installation has the `Enterprise` folder in its path; that's because I happen to be using
Visual Studio Enterprise. If you don't, you'll have a slightly different path.)

The fun begins when you get to that Extensions directory ... each extension is installed in a 
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
<tr><td valign="top"><code style="background-color: transparent;">EFCoreDesigner.ttinclude</code></td><td valign="top">Same as above, but for EF Core v2 instead.</td></tr>
<tr><td valign="top"><code style="background-color: transparent;">MultipleOutputHelper.ttinclude</code></td><td valign="top">Code to create more than one output file, since T4 usually doesn't do that. You probably won't mess with this.</td></tr>
<tr><td valign="top"><code style="background-color: transparent;">VSIntegration.ttinclude</code></td><td valign="top">Code to talk to Visual Studio so we can add project items, etc. You probably won't mess with this either.</td></tr>
</tbody>
</table>

Decide what you want to replace and copy the file into the folder containing your .efmodel and .tt
files. Modify at will; the templating engine will find your local copy before it looks anywhere else.
That will customize the code generation for that project. If you want to globally change how code
is generated, edit the `.ttinclude` files directly in the install directory. Be aware, though, that
updates will overwrite those files, and you'll have to go through the excercise of editing them whenever
you install a newer version of the designer. (If you feel strongly that you have a really good
improvement,  you can always submit it as a pull request!)

For code generation from the T4, the flow of events starts at the `[Model Name].tt` file in your
project. Its template directives tell it to include the five templates listed above and then,
based on the Entity Framework type specified, call one of two entry points.

You'll notice that these aren't your standard T4 templates, where the bulk of the file is text
to be output. Rather, they're like scripting libraries. That's on purpose -- a design goal was
to allow the generation to be customizable but, at the same time, understandable to the average
programmer. A group of function calls is easier to read, IMHO, than your everyday T4 template,
so that's what we have.

The templating engine works through the model, pulling together all the code in memory. It isn't
until the call to `manager.Process(true)` at the end that the files are generated. So any customizations
to the process should come before that call.

### Customizing Designer Behavior

Beginning with v1.1, the designer can be extended using Microsoft's [Managed Extensibility Framework](https://docs.microsoft.com/en-us/dotnet/framework/mef/index)
so that you can modify not just the output generating via the T4 templates, but the designer behavior itself.
Microsoft has a nice writeup on how to [write MEF extensions](https://docs.microsoft.com/en-us/visualstudio/modeling/extend-your-dsl-by-using-mef) 
for domain-specific languages that applies not only to the designer but any Visual Studio DSL ... please see that documentation for details.

### Next Step 
[Contributing](Development)
