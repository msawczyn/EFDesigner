# Importing Existing Code

You can import existing C# code into your model in two ways: 

 - from source files in projects in your current solution or from external files on your drive, and
 - from compiled assemblies that contain DbContext classes

For source files, the designer supports drag-and-drop from both the Visual Studio Solution Explorer 
window and from Windows File Explorer. Compiled assemblies can be dragged from the Windows File Explorer.

## Importing source code from other projects

Select a project item from a project in the current solution, and drag/drop it onto an open space on the
design surface. The designer will parse the contents of the project item using Roslyn and attempt to add a 
class into the model.

If a class with the same name is already in the model, its properties will be cleared and any properties found
in the dropped class will be added instead. This is true even if you drop a partial class onto the model, since
the designer doesn't have any knowledge of exactly where the dropped data came from. So, for instance, if you have

```
class Foo
{
   public int Bar { get; set; }
}
```

and drop onto the designer surface

```
class Foo
{
   public string Zoom { get; }
}
```

then the definition that contains `Bar` is gone ... you're left with a `Foo` that only has a `Zoom`.

Associations will be left intact, but new associations may be added if they're found in the dropped class. 
The designer will, for associations, try to connect to any properly named classes it can find. If it can't find
one, it will create a new class (that you can later fill in by dropping that class onto the designer so it can
consume its properties).

### Caveats

Something to be aware of is that **bidirectional associations** aren't really a thing in source code. The designer
will attempt to find associations in classes that appear to be inverses of each other and change them from
unidirectional to bidirectional. It might, though, be wrong, so you should review any bidirectional associations to
make sure they're what you intended them to be. For instance, if we have classes A and B with A -> B and B <- A,
the designer will turn that into A <--> B. That might not be accurate, and you might have to split them apart into two
unidirectional associations if that's appropriate. There's a tool for that - select the association and right-click, 
then chose `Split Bidirectional Association` from the context menu. Conversely, if the designer truly drops the 
ball and doesn't merge two unidirectional associations into an appropriate bidirectional linkage, you can use the
`Merge Unidirectional Associations` menu choice after selecting the two associatioins you want to merge.

**Enumeration properties** can be a challenge here. If the designer sees an existing enum with the same name as a property
type in your dropped class, it will create an enum property in the class. If not, it will assume it's a class
it hasn't yet seen and create an empty class with an association to that class. With that behavior in mind, your
best bet is to add any enumerations you're planning to use first; that way the designer knows they're enums and will
handle them correctly.

All modeling rules are applied after the drop so, if it finds problems, you'll get the typical errors and/or warnings about
things like duplicate identifiers, invalid property names and the like.

**Inheritance** is a tricky thing to work with when using Roslyn as a syntax parser. Consider the following:

```
class Foo : Bar, Zoom
{
   ... stuff
}
```

Clearly, `Zoom` is an interface -- .NET restrictions disallow multiple base classes. But is `Bar` a base class
or an interface? Without its definition, we really can't know for sure. Even if we saw `IDisposible` there rather than
`Bar`, we couldn't be *completely* sure without more context since we can have a class named `IDisposible` in a different
namespace (and, if you're considering ever doing that, drop me a line ... we need to have a *long* chat).

So, in those circumstances, the designer might ask you to clarify what it should do with those ... turn them into a
superclass or make them interfaces? Whatever you decide will be set in the entity properties and, of course, you can
change that decision later.

A Visual Studio limitation around drag/drop from the Solution Explorer is that you can only drag one item. So if you want to 
mass import a bunch of things, you might want to consider dragging the files from Windows File Explorer.

## Importing source code from Windows File Explorer

File Explorer doesn't have the one-file limitation that Visual Studio's Solution Explorer does, so you can drag to your heart's
content. The same principles apply as above.

## Importing compiled assemblies

If you have a compiled assembly that contains a DbContext class you want to work in the designer, just drag it from Windows
File Explorer and drop it onto the design surface. The designer will try to open the file and load the class into
memory; because of that, you'll need to make sure all the dependencies that assembly needs are available. The simplest
way to do this is to have them all available either in the same directory or in the GAC.

If the designer runs into any roadblocks, you'll see an error message and the import will abort.

The designer can handle assemblies that are:

 - .NET Core with EFCore `DbContext`
 - .NET Framework with EFCore `DbContext`, and
 - .NET Framework with EF6 `DBContext`

As of this writing, .NET Core with EF6 `DbContext` isn't possible, but that's anticipated to change with .NET Core 3.0
and, when it does, look for an update that supports that combination as well.

When adding classes, you'll see behavior similar to dropping source files: properties in existing classes with the same
name and namespace will be replaced. But here, associations will be replaced as well. The thought is that, since you're working
with a *compiled* assembly, it's pretty accurate and you should get what's in the assembly rather than a merge of what you
have in the designer with the assembly's structure. The best thing to do with assembly import is to import into an empty
design surface. If you have work you need to retain, import into a brand new `efmodel` file, then you can copy/paste into
another model file should that be needed.

Consuming compiled assemblies is a lot more accurate than parsing source code since there is more information available. The issues
with inheritance, associations and enumerations shouldn't be there. But, as always, if you find a problem please file an
issue in Github.


## A quick tip

Importing source code via drag/drop can be a really quick way of making a POCO entity set persistent in Entity Framework.
But it's generally a bad idea to go bat crazy and drop a huge set of files all at once. LOTS of things will get created
that you'll then have to pick through and validate, edit and enhance (not every Entity Framework concept is necessarily present in 
a class definition). Take it a bit at a time -- say, one or two classes. Move the entities around the diagram so that you
can read them and validate that everything's the way you want it, then add a few more. You'll be thankful.

Dropping in an assembly is an all-or-nothing thing, though, so although the model generation is more accurate, you
should plan to spend some time cleaning up your diagram if you have a large number of entities in your universe.

### Next Step 
[Generating Code](Templates)
