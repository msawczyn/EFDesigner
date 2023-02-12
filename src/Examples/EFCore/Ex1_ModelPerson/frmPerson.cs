using System.Globalization;
using System.Data.Entity.Validation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

// TOOLS>NUGET PACKAGE MANAGE> 'Nuget GUI' or 'Package Manager Console/CLI'
// PM:> Install-Package Microsoft.EntityFramework
// PM:> Install-Package Microsoft.EntityFrameworkCore.SqlServer


// Enable-> optionsBuilder.UseLazyLoadingProxies();
// using Microsoft.EntityFrameworkCore.Proxies;
// Install-Package Microsoft.EntityFrameworkCore.Proxies


namespace Ex1_ModelPerson
{
    public partial class FrmPerson : Form
    {
        DbContextOptionsBuilder<PersonModel> optionsBuilder;
        public FrmPerson()
        {
            //Setup Connection string holder
            optionsBuilder = new DbContextOptionsBuilder<PersonModel>();
            optionsBuilder.UseSqlServer(PersonModel.ConnectionString);
            if (Debugger.IsAttached)
            {
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }

            InitializeComponent();

        }

        private void FrmPerson_Load(object sender, EventArgs e)
        {
            txtConnection.Text = PersonModel.ConnectionString;
        }

        private void btnTestPerson_Click(object sender, EventArgs e)
        {
            TestPerson();
        }
        private void TestPerson()
        {
            txtDebug.Text = "TestPerson()\r\n";

            //.Net Core has IoC (Inversion Of Control) implemented in it's roots and uses Dependency Injection, Instance pooling 
            // This means; you don't create a context, you ask the framework to give you one, based on some rules you defined before.
            // Unfortunatley, using DI enables many things, true, including complexity.. this is beyond this basic demos requirements.
            
            // So for this we use, a constructor  For this example we create a dbcontext object on request and dynamically point it to the user's individual database
            // Sometimes requirements, time or money constraints, quality attributes or anything else 

            // This is a way to "just a way to instantiate" the object, activator's CreateInstance
            

            using (PersonModel context = new PersonModel(optionsBuilder.Options))
            {
                // Perform data access using the context

                // ---------------
                // TODO: I dont know where to set AutomaticMigrationDataLossAllowed = true in efmodeller tool?
                // Can set AutomaticMigrationDataLossAllowed in break points, runtime doing it via a delete and recreate

                txtDebug.Text += "Attempting to Delete, \r\n";
                context.Database.EnsureDeleted();
                txtDebug.Text += "Deleted DB\r\n";

                context.Database.EnsureCreated();
                txtDebug.Text += "Created DB\r\n";

                Person person = new Person();
                person.FirstName = "Bob";
                person.MiddleName = "James";
                person.LastName = "Smith";
                person.Phone = "555-123-321";
                CultureInfo culture = new CultureInfo("en-AU");
                person.DOB = Convert.ToDateTime("6/12/70", culture);

                context.People.Add(person);

                // This Exception handler helps to describe what went wrong with the EF database save.
                // It decodes why the data did not comply with defined database field structure. e.g too long or wrong type.
                try
                {
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

                //Read it back
                DbSet<Person> people = context.People;

                foreach (Person p in people)
                    txtDebug.Text += String.Format("{0} {1} {2} {3} {4}", p.Id, p.FirstName, p.MiddleName, p.LastName, p.Phone) + "\r\n";
            }
        }
    }
}
