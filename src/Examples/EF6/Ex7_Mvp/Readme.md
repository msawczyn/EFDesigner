 ## MVP Design Pattern for winform 
 
 > Mvp is a Paradigm change for C# Winform RAD Visual editor (Drag & Drop) that has the code behind control events.
 
 Change to the code structure:
 
 > Rule #1: Depend on the INTERFACE that is implemented by a class 

 > Do not front end load Winform (CodeBehind) on wired up events

 > Control Events link to Interface methods not Class methods
 
### MVP Workflow 

## MVP SUMMARY:

> REPOSITORY: Is the Db WORKER -> Has the Db CRUD Methods
#### MODEL: [Datacontext] 
> INTERFACE (Repository) : Exposes worker events for the Presenter to databind controls to 
#### VIEW: GUI A databound view
>INTERFACE (View) : -> Properties expose databinding to UI
#### PRESENTER: GUI WORKER 
-> Logic Wires up Repository and View together via Interfaces , uses events to fire GUI Update / reads 

## NOTES:
MVP changes from Frontend loading Winform RAD to MVP GUI changes via the presenter (Due to databinding not being as good as WPF)
i.e dont add gui logis in codebehind methods but reloacte to presenter

So #1 rule of Winform programer changing to MVP is <DON'T DEPEND ON CLASSES> , [DEPEND ON THE <INTERFACES>!]

If you want to have your code testable and maintainable, depend on interface that is implemented by a class.

## MVP Dev Process

## REPOSITORY:
a. Using [EF Visual Editor](https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner2022) manually define data layout/structure then -> Auto Generate MODEL (this feeds the next steps)

Ef Visual Designer not only does the database it also does up the objects, so

``` 
Use visual editor to define Database Db
 <TableA -> FieldA>
 ``` 

 ``` 
Use the 'Auto Generate' to create models inside : Model.generated.cs

 We are after
public partial class PetModel
   {
      partial void Init();

      /// <summary>
      /// Default constructor
      /// </summary>
      public PetModel()
      {
         Init();
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Min length = 3, Max length = 50
      /// </summary>
      [MinLength(3)]
      [MaxLength(50)]
      [StringLength(50)]
      public string Colour { get; set; }
``` 

b. Create: ModelRepository.cs Code up Ef Db context CRUD operations (Create/Read/Update/Delete) methods and filter / select data methods
          
c. Create: IModelRepository.cs -> Manually Expose <b> interface via
  Interface IRepository.cs is like header.h file in c
  
```             
public interface IRepository
{
 void Add(Model model);
 IEnumerable<PetModel> GetAll();
}   
```

## VIEWS:
a. Using <Winform RAD> layout Appearance, generates form.designer.cs
b. Add codebehind to expose forms control fields {get;set;} into form.cs
```
public partial class PetView : Form, IPetView
    {
    ...
    ....

        //Properties
        public string PetId
        {
            get { return txtPetId.Text; }
            set { txtPetId.Text = value; }
        }

        public string PetName
        {
            get { return txtPetName.Text; }
            set { txtPetName.Text = value; }
        }
   ...
   ..
        //Events
        public event EventHandler SearchEvent;
        public event EventHandler SearchClearEvent;
        public event EventHandler AddNewEvent;
        public event EventHandler EditEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler SaveEvent;
        public event EventHandler CancelEvent;

        //Methods
        public void SetPetListBindingSource(BindingSource petList)
        {
            dataGridView.DataSource = petList;
        }
   ...
   ..

```
           
c. Create Interface IView 
```
public interface IPetView
    {
        //Properties - Fields
        string PetId { get; set; }
        string PetName { get; set; }
        string PetType { get; set; }
        string PetColour { get; set; }

        string SearchValue { get; set; }
        bool IsEdit { get; set; }
        bool IsSuccessful { get; set; }
        string Message { get; set; }

        //Events
        event EventHandler SearchEvent;
        event EventHandler SearchClearEvent;
        event EventHandler AddNewEvent;
        event EventHandler EditEvent;
        event EventHandler DeleteEvent;
        event EventHandler SaveEvent;
        event EventHandler CancelEvent;
        
        //Methods
        void SetPetListBindingSource(BindingSource petList);
        void Show();//Optional

    }  
```
## PRESENTER: 
- Presenter Links up using INTERFACES the GUI to the Database for read write

> IRepository <= Presenter => IView

- Db Crud [Repository Layer] 
- <-> Model> layer [Interface] (Wires up Model) 
- <-> Gui Controls via [Presenter]) based upon events  
-  Model<-View Events> [GUI] fire Events

```
 public PetPresenter(IPetView view, IPetRepository repository)
        {
            this.petsBindingSource = new BindingSource();
            this.view = view;
            this.repository = repository;

            //Subscribe event handler methods to view events
            this.view.SearchEvent += SearchPet;
 ```

 # WIP: Multi user Changes to data detect
 To detect changes in a database comes from the ***SqlDependency*** class
 
 The problem is these classes only work with SQL Server non-Express
 
 Some common gotchas:
 - Does not work with SQL Server Express;
 - There’s a specific order by which the connection and SqlDependency.Start must be called, if you stick to my code, it should work;
 - Needs SQL Server Service Broker enabled for the database that we are monitoring, but does not require creating a queue or service, SqlDependency does that for us automatically;
 - Your application pool identity needs to have permissions to connect to the notification queue, see them here;
- Won’t work with any SQL query, a number of restrictions apply; see here;
- SqlDependency is very sensitive, and you can easily run into problems if you try different things.

SignalIR:

 ```
  
  var notifier = new ChangeNotifier();
   2: notifier.Change += this.OnChange;
   3: notifier.Start("MyConnection", "SELECT SomeColumn FROM dbo.SomeTable");
 ```

### Install Nuget Package

Install-Package EntitySignal.Client

 https://entitysignal.com/