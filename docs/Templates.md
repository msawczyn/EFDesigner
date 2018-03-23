# Generating Code

Likely the whole reason you've created the entities in the designer is that you
eventually want to generate code for some application. Nothing could be easier.

If you've set the designer to automatically generate code on save, just save the
model. Your code will be created.

Otherwise, you can right click anywhere and choose *Generate Code*, or use the 
shortcut key (`Ctrl-G` by default).

If previously generated files no longer exist, the T4 templates attempt to clean
them up. They err on the conservative side, though... we wouldn't want to start deleting
code that you spent time creating, would we? So there may be a few strays left behind
when you delete entities or enumerations and regenerate.

Code is generated where you told it go (or, if you haven't, in the same directory and
solution folder where the model file lives).

Since code is generated from the T4 files, you can also do the standard *Run Custom Tool* command
on the .tt file and get the same result.

## What's Generated

While [you can easily change the code that's generated](Customizing), you might want to know exactly what 
you'll get if you use the code that's generated out-of-the-box. If so, read on!

### DbContext

<table>
<thead>
<tr><th>Properties & Methods</th><th>Description</th></tr>
</thead>
<tbody>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><code style="background-color: transparent; font-weight: bold;">[DbContext Name].generated.cs</code></td></tr>
<tr><td valign="top">(various DbSets)</td><td valign="top">One per class in the model</td></tr>
<tr><td valign="top">ConnectionString</td><td valign="top">Static string that will be used as the default connection string</td></tr>
<tr><td valign="top">Constructors</td><td valign="top">Constructors are generated for each constructor signature of the DbContext class. They initialize various properties that were
entered into the designer and, for those things the designer couldn't anticipate, executes a call to <code class="highlighter-rouge">CustomInit()</code>, a partial method you can choose to implement
to do anything else you need to do in the constructor.</td></tr>
<tr><td valign="top">OnModelCreatingImpl</td><td valign="top">Since OnModelCreating is generated, this is a partial method you can optionally implement to do some custom work just <i>before</i> the DbModelBuilder functions are called.</td></tr>
<tr><td valign="top">OnModelCreatedImpl</td><td valign="top">Same as the above, but called just <i>after</i> the DbModelBuilder functions are executed.</td></tr>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><code style="background-color: transparent;">[DbContext Name]DatabaseInitializer.generated.cs</code></td></tr>
<tr><td valign="top" colspan="2">Generated to derive from the initializer class set in the model. Won't be generated if <code>None</code> was chosen. No properties or methods overridden.</td></tr>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><code style="background-color: transparent;">[DbContext Name]DbMigrationConfiguration.generated.cs</code></td></tr>
<tr><td valign="top">Constructor</td><td valign="top">Sets up values indicated in the designer and calls the partial method <code>Init()</code> that you can optionally implement to do custom work. Seed methods can be added to a partial of this class in another file.</td></tr>
</tbody>
</table>

### Entities

<table>
<thead>
<tr><th>Properties & Methods</th><th>Description</th></tr>
</thead>
<tbody>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><code style="background-color: transparent;">[Entity Name].generated.cs</code></td></tr>
<tr><td valign="top">Default Constructor</td><td valign="top">Always generated (because Entity Framework needs it), but will be created as <code>protected</code> if there are any required properties or associations in the class. Calls the partial method <code>Init()</code> that you can optionally implement to do custom work.</td></tr>
<tr><td valign="top">Other Constructor</td><td valign="top">Generated if there are any required properties. Parameters will include those properties, and code in the body will check to ensure they're not null or the default value for their type. Calls the partial method <code>Init()</code> that you can optionally implement to do custom work.</td></tr>
<tr><td valign="top">Create</td><td valign="top">A static convenience method that creates and returns an entity. Useful for LINQ queries where constructor parameters are necessary, since there are requirements to only use parameterless constructors when querying a database.</td></tr>
<tr><td valign="top">(listed properties)</td><td valign="top">Declarations of the properties and associations in the model.</td></tr>
</tbody>
</table>

### Next Step 
[Customizing Code Generation](Customizing)
