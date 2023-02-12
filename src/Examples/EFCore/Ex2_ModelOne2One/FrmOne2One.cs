using System;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore; //<- Add this

// Install Nuget PAckages
// ----------------------
// TOOLS>NUGET PACKAGE MANAGE> 'Nuget GUI' or 'Package Manager Console'
// PM:> nuget install Microsoft.EntityFramework
// PM:> NUGET Microsoft.EntityFrameworkCore.SqlServer


namespace Ex2_ModelOne2One
{
    public partial class FrmOne2One : Form
    {
        //https://medium.com/@emekadc/how-to-implement-one-to-one-one-to-many-and-many-to-many-relationships-when-designing-a-database-9da2de684710
        DbContextOptionsBuilder<EFModelOne2One> optionsBuilder;

        public FrmOne2One()
        {
            InitializeComponent();

            // If you have issues: 
            // Check nuget packages are installed / reinstall
            // ----------------------------------------------
            // PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer

            // Check your connection string matches you sqllocaldb
            // ----------------------------------------------------
            //> sqllocaldb i

            // MSSQLLocalDB
            // or
            // (localdb)\v11.0


            optionsBuilder = new DbContextOptionsBuilder<EFModelOne2One>();
            optionsBuilder.UseSqlServer(EFModelOne2One.ConnectionString);
            optionsBuilder.LogTo(Console.WriteLine);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            using (EFModelOne2One context = new EFModelOne2One(optionsBuilder.Options))
            {
                txtConnection.Text = EFModelOne2One.ConnectionString;
            }
        }

        private void btnTestOne2One_Click(object sender, EventArgs e)
        {
            TestOne2One();
        }

        void TestOne2One()
        {
            txtDebug.Text = "TestOne2One()\r\n";

            using (EFModelOne2One context = new EFModelOne2One(optionsBuilder.Options))
            {
                // Perform data access using the context
                txtDebug.Text += "Deleting DB...";
                context.Database.EnsureDeleted();
                txtDebug.Text += "Deleted Ok\r\n";

                context.Database.EnsureCreated();
                txtDebug.Text += "Created DB\r\n";

                Person person1 = new Person();
                Address address1 = new Address();

                person1.FirstName = "Bob";
                person1.MiddleName = "James";
                person1.LastName = "Smith";
                person1.Phone = "555-123-321";
                CultureInfo culture = new CultureInfo("en-AU");
                person1.DOB = Convert.ToDateTime("6/12/70", culture);

                address1.Number = "1";
                address1.StreetLine1 = "High St";
                address1.City = "Perth";
                address1.Country = "Australia";

                //Create Address row
                context.Addresses.Add(address1);
                
                // Create reference, One to One
                person1.Address = address1;

                context.People.Add(person1);


                //Create another 1 : 0/1 record 

                Person person2 = new Person();
                Address address2 = new Address();

                person2.FirstName = "Paul";
                person2.MiddleName = "Michael";
                person2.LastName = "Black";
                person2.Phone = "555-324-564";
                person2.DOB = Convert.ToDateTime("10/1/82", culture);

                address2.Number = "34";
                address2.StreetLine1 = "Murray St";
                address2.City = "Melbourne";
                address2.Country = "Australia";

                //Create Address row
                context.Addresses.Add(address2);

                // Create reference, One to One
                person2.Address = address2;
                context.People.Add(person2);

                try
                {
                    // SqlException: Cannot insert duplicate key row in object 'dbo.Addresses' with unique index 'IX_Addresses_AddressId'.
                    // The duplicate key value is (0).
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
                var items = context.People;

                foreach (var x in items)
                {
                    txtDebug.Text += String.Format("{0} {1} {2} {3} {4} ", x.PersonId, x.FirstName, x.MiddleName, x.LastName, x.Phone) + "\r\n";
                    txtDebug.Text += String.Format("{0} {1} {2} {3} {4} {5} ", x.Address.Number, x.Address.StreetLine1, x.Address.StreetLine2, x.Address.StreetType, x.Address.City, x.Address.PostalCode) + "\r\n\r\n";
                }
            }
        }
    }
}