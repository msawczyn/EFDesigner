using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex1_Person
{
    public partial class frmPerson : Form
    {
      //How to use Entity Framework
      //https://docs.microsoft.com/en-us/ef/ef6/

      public frmPerson()
        {
            InitializeComponent();
        }

      private void Form1_Load(object sender, EventArgs e)
      {
         using (PersonModel context = new PersonModel())
         {
            txtConnection.Text = context.Database.Connection.ConnectionString;
         }

      }

      private void btnTestPerson_Click(object sender, EventArgs e)
      {
         TestPerson();
      }

      private void TestPerson()
      {
         txtDebug.Text = "TestPerson()\r\n";

         using (PersonModel context = new PersonModel())
         {
            // Perform data access using the context
            context.Database.Log = Console.Write;
            
            context.Database.Delete();
            txtDebug.Text += "Deleted DB\r\n";

            context.Database.CreateIfNotExists();
            txtDebug.Text += "Created DB\r\n";

            Person person = new Person();
            person.FirstName = "Bob";
            person.MiddleName = "James";
            person.LastName = "Smith";
            person.Phone = "555-123-321";
            CultureInfo culture = new CultureInfo("en-AU");
            person.DOB = Convert.ToDateTime("6/12/70",culture);

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

      private void button1_Click(object sender, EventArgs e)
      {

      }
   }
}
