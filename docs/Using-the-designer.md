# Using the Designer 
Let's add a design surface to our new project.

<img src="images/AddNewItem.jpg">

<img align="right" src="images/Solution2.jpg" hspace="10">

- Right-click the project root and select _Add/New Item..._ from the dropdown menu.
- Find the _Data_ folder under _Visual C# Items_ in the tree to the left and select _EFDesigner_ from the list.
- Name the file to your liking. The name of the file will become the default name of the generated DbContext, though you can change that later. For our example we'll call it _MyModel_.
- Click _Add_.


Three files will be added to your project: MyModel.efmodel, MyModel.efmodel.diagram and MyModel.tt

- The .efmodel file holds the details of your design
- The .efmodel.diagram file holds the details of the visual representation of your design, defining what you see on the screen
- The .tt file is a T4 template, the entry point into code generation. Other T4 templates were installed, but those are in a common location where your Visual Studio extensions reside

Should you decide to rename these files later, **it's important that the file names stay in sync**. In fact, if you do rename them, you'll need to make a manual edit to the .tt to let it know the name of the .efmodel file, since it needs to find that when you want to generate code. That's in line 17 of the .tt file, but we'll talk more about that later.

<img align="right" src="images/Toolbox.jpg" hspace="10" vspace="10">

Open the .efmodel file and -- so exciting! -- you're presented with a blank screen. No worries -- this is where you'll model your classes. In your toolbox, you'll see various items that can be added to model.

<table>
<thead>
<tr><th valign="top"><b>Tool</b></th><th valign="top"><b>Description</b></th></tr>
</thead>
<tbody>
<tr><td valign="top"> Entity                     </td><td valign="top"> A persistent class </td></tr>
<tr><td valign="top"> Unidirectional Association </td><td valign="top"> A navigation property that only goes one way. The starting class will have a property of the type of the ending class (or a collection of them, depending on cardinality), but not the other way around. </td></tr>
<tr><td valign="top"> Bidirectional Association  </td><td valign="top"> A navigation property that will create properties in classes on both ends. </td></tr>
<tr><td valign="top"> Inheritance                </td><td valign="top"> Standard .NET inheritance (and its associated constraints). Start the line at the derived class and point to bhe base class </td></tr>
<tr><td valign="top"> Comment                    </td><td valign="top"> Add a comment box anywhere on the model </td></tr>
<tr><td valign="top"> Comment Link               </td><td valign="top"> Link an existing comment to an Entity or Enum. Not required since comments can be free-floating, but nice to have if the comment refers to something specific on the diagram. </td></tr>
<tr><td valign="top"> Enum                       </td><td valign="top"> An enumeration </td></tr>
</tbody>
</table>

## Design Surface Properties

A peek at the Property page when you have the design surface selected (the background, rather than anything on it), shows the properties pertinent to the model itself and the DbContext class you'll generate. These are as follows:

<table>
<thead>
<tr><th valign="top"><b>Property</b></th><th valign="top"><b>Description</b></th></tr>
</thead>
<tbody>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><b>Code Generation</b></td></tr>
<tr><td valign="top"> Chop Method Chains           </td><td valign="top"> <i>Boolean</i>. If true, will chop (and align on dot) long method chains in the generated code</td></tr>
<tr><td valign="top"> DbContext Output Directory   </td><td valign="top"> <i>String</i>. The project directory for generated DbContext-related files</td></tr>
<tr><td valign="top"> Default Collection Class     </td><td valign="top"> <i>String</i>. The type of container generated for associations (if not overridden). Must implement ICollection&lt;T&gt;.</td></tr>
<tr><td valign="top"> Default Identity Type        </td><td valign="top"> <i>String</i>. Default type for ID properties in new classes. Valid values are 'Int16', 'Int32', 'Int64', 'Byte' and 'Guid'</td></tr>
<tr><td valign="top"> Entity Framework Version     </td><td valign="top"> <i>String</i>. Version of Entity Framework for generated code. Values are currently 'EF6' and 'EFCore'</td></tr>
<tr><td valign="top"> Entity Output Directory      </td><td valign="top"> <i>String</i>. The project directory for generated Entity files</td></tr>
<tr><td valign="top"> Enum Output Directory        </td><td valign="top"> <i>String</i>. The project directory for generated enumeration files</td></tr>
<tr><td valign="top"> File Name Marker             </td><td valign="top"> <i>String</i>. The text appended to the entity name for generated files. E.g., if set to "generated" (the default), files will be named "MyClass.generated.cs", "MyDbContext.generated.cs", etc.</td></tr>
<tr><td valign="top"> Inheritance Strategy         </td><td valign="top"> <i>String</i>. How tables will be created/used to handle inheritance. Values are "TablePerConcreteType", "TablePerHierarchy" and "TablePerType". See <a href="https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-inheritance-with-the-entity-framework-in-an-asp-net-mvc-application">Implementing Inheritance with the Entity Framework 6 in an ASP.NET MVC 5 Application </a> and <a href="https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/inheritance">Inheritance - EF Core with ASP.NET Core MVC tutorial</a></td></tr>
<tr><td valign="top"> Lazy Loading Enabled         </td><td valign="top"> <i>Boolean</i>. If true, entity container will allow lazy loading. See <a href="https://msdn.microsoft.com/en-us/library/system.data.entity.infrastructure.dbcontextconfiguration.lazyloadingenabled(v=vs.113).aspx">DbContextConfiguration.LazyLoadingEnabled Property</a></td></tr>
<tr><td valign="top"> Namespace                    </td><td valign="top"> <i>String</i>. Default namespace for all generated classes. Can be overridden on a class-by-class basis.</td></tr>
<tr><td valign="top"> Proxy Generation Enabled     </td><td valign="top"> <i>Boolean</i>. If true, context will generate proxies for POCO entities. See <a href="https://msdn.microsoft.com/en-us/library/system.data.entity.infrastructure.dbcontextconfiguration.proxycreationenabled(v=vs.113).aspx">DbContextConfiguration.ProxyCreationEnabled Property</a>.</td></tr>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><b>Database</b></td></tr>
<tr><td valign="top"> Automatic Migrations Enabled </td><td valign="top"> <i>Boolean</i>. If true, code will allow automatic database migrations to be run when changes are detected. See <a href="https://msdn.microsoft.com/en-us/library/system.data.entity.migrations.dbmigrationsconfiguration.automaticmigrationsenabled(v=vs.113).aspx">DbMigrationsConfiguration.AutomaticMigrationsEnabled Property</a>.</td></tr>
<tr><td valign="top"> Concurrency Default          </td><td valign="top"> <i>String</i>. Default concurrency handling strategy. Values are 'Optimistic' and 'None'. See <a href="https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/handling-concurrency-with-the-entity-framework-in-an-asp-net-mvc-application">Handling Concurrency with the Entity Framework 6 in an ASP.NET MVC 5 Application</a>.</td></tr>
<tr><td valign="top"> Connection String            </td><td valign="top"> <i>String</i>. Connection string to use as the default connection string for the DbContext. You can call up the connections string dialog here and build the string so you don't have to remember the syntax. Mutually exclusive with <b>Connection String Name</b></td></tr>
<tr><td valign="top"> Connection String Name       </td><td valign="top"> <i>String</i>. Name of connection string in host application config file to use as the default connection string for the DbContext. Mutually exclusive with <b>Connection String</b></td></tr>
<tr><td valign="top"> Database Initializer Type    </td><td valign="top"> <i>String</i>. Initialization strategy to synchronize the underlying database when the DbContext doesn't match the database. Values are 'CreateDatabaseIfNotExists', 'DropCreateDatabaseAlways', 'DropCreateDatabaseModelChanges', 'MigrateDatabaseToLatestVersion', and 'None'. For a primer on migration strategies, see Julie Lerman's excellent MSDN Magazine article <a href="https://msdn.microsoft.com/en-us/magazine/dn818489.aspx">Data Points : A Code First Migrations Mystery: Solved</a></td></tr>
<tr><td valign="top"> Database Schema              </td><td valign="top"> <i>String</i>. The schema name for your database tables</td></tr>
<tr><td valign="top" colspan="2" style="background-color: gainsboro"><b>Entity Context</b></td></tr>
<tr><td valign="top"> Entity Container Access      </td><td valign="top"> <i>String</i>. Code visibility for entity container. Values are 'Internal' and 'Public'.</td></tr>
<tr><td valign="top"> Entity Container Name        </td><td valign="top"> <i>String</i>. Name of generated DbContext-derived class</td></tr>
<tr><td valign="top"  colspan="2" style="background-color: gainsboro"><b>Misc</b></td></tr>
<tr><td valign="top"> Database Type                </td><td valign="top"> <i>String</i>. Value of ProviderManifestToken attribute, for optimization of runtime database type is known and unchanging. Values are 'None', 'SqlServer' (2008) and 'SqlServer2012'. For more information see <a href="https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/provider-manifest-specification">DbProviderInfo.ProviderManifestToken Property</a>.</td></tr>
<tr><td valign="top"> Show Cascade Deletes         </td><td valign="top"> <i>Boolean</i>. If true, will display associations that cascade delete as dashed red lines, otherwise all associations will display as solid black lines.</td></tr>
<tr><td valign="top"> Transform On Save            </td><td valign="top"> <i>Boolean</i>. If true, will run Visual Studio's Transform All Templates command when the model is saved</td></tr>
<tr><td valign="top"> Warn On Missing Documentation</td><td valign="top"> <i>Boolean</i>. If true, will generate warnings when summary documentation is missing for classes, properties and association ends.</td></tr>
</tbody>
</table>

## Context Menu

Right-clicking on the design surface displays a Visual Studio context menu with some new choices pertinent to the designer.

<img src="images/DesignerMenu.jpg" hspace="10" vspace="10">

<table>
<thead>
<tr><th valign="top"><b>Menu choice</b></th><th valign="top"><b>Description</b></th></tr>
</thead>
<tbody>
<tr><td valign="top">Cut, Copy, Paste</td><td valign="top">You can cut or copy, then paste, classes and enums. The pasted elements will be adjusted so that they don't violate any rules (such as two elements not having the same name), but otherwise the properties will stay the same. If no classes or enums are selected in the designer, the cut and copy options will be disabled. If no classes or enums are in the clipboard, the paste option will be disabled.</td></tr>
<tr><td valign="top">Validate</td><td valign="top">Checks the currently selected element (class, enum, etc.) against the validation rules built into the designer. Errors or warnings are displayed in Visual Studio's Error List window. If no element is selected, this validates the design surface itself.</td></tr>
<tr><td valign="top">Validate All</td><td valign="top">Checks all model elements against the afore mentioned validation rules. Errors or warnings are displayed in Visual Studio's Error List window.</td></tr>
<tr><td valign="top">Layout Diagram</td><td valign="top">Arranges all model elements on the design surface. If no elements are in the designer, this option will be disabled. Note that this is the stock "Layout Diagram" behavior for Visual Studio's Modeling SDK, so you could almost certainly do a better job by hand.</td></tr>
<tr><td valign="top">Hide Element</td><td valign="top">Hides the currently selected element on the diagram. Any lines to or from that element will be hidden as well. This does not remove the element from the model, only makes it invisible in the diagram. Useful for tidying up a diagram that would otherwise be unreadable due to, for example, a common base class that all other classes inherit from. If no element is selected, this option will be disabled.</td></tr>
<tr><td valign="top">Show Hidden Elements</td><td valign="top">Unhides any elements that were previously hidden, along with their association or inheritance lines. If no elements are hidden, this option will be disabled.</td></tr>
<tr><td valign="top">Generate Code</td><td valign="top">Generates code for the elements modeled in the designer. If no elements are in the designer, this option will be disabled.</td></tr>
<tr><td valign="top">Add properties via Code</td><td valign="top">Displays a dialog that lets you add multiple properties using the designer's custom property syntax. See "<a href="Properties.html#adding-properties-via-code-custom-property-syntax">Adding properties via code</a>" for more details.</td></tr>
<tr><td valign="top">Save as Image</td><td valign="top">Creates a jpg file of the designer and lets you save it as a file. If no elements are in the designer, this option will be disabled.</td></tr>
<tr><td valign="top">Select</td><td valign="top">One of the features of the Visual Studio property editor is the ability to edit properties of multiple items if they share that property. This submenu gives you the ability to select model elements by type so that you can conveniently edit properties of those elements together (e.g., setting the color of multiple classes all at once). If the pertinent element type isn't present in the designer, that option will be disabled.<br/>
<table>
<tr><td valign="top">Select all classes...</td><td valign="top">Select all class elements in the designer</td></tr>
<tr><td valign="top">Select all enums...</td><td valign="top">Select all enum elements in the designer</td></tr>
<tr><td valign="top">Select all associations...</td><td valign="top">Select all association lines (both unidirectional and bidirectional) in the designer</td></tr>
<tr><td valign="top">Select all unidirectional associations...</td><td valign="top">Select all unidirectiional association lines in the designer</td></tr>
<tr><td valign="top">Select all bidirectional associations...</td><td valign="top">Select all bidirectiional association lines in the designer</td></tr>
<tr><td valign="top">Select all bidirectional associations...</td><td valign="top">Select all bidirectiional association lines in the designer</td></tr>
</table>
</td></tr>
<tr><td valign="top">Properties</td><td valign="top">Switches focus to the Properties window.</td></tr>
</tbody>
</table>

### Generating Code

Once you have your model the way you want it (and we'll talk how to do that during the rest of this
documentation), you can generate the code in one of three ways:
   
   - If you've set the `Transform On Save` property on the design surface to `true`, just save the file
   - At any time, you can right click the design surface and choose `Generate Code` from the context menu
   - As with any T4 file, you can always right click the `[Model Name].tt` file in your project and choose `Run Custom Tool` from the context menu

### Next Step 
[Persistent entities](Entities)


