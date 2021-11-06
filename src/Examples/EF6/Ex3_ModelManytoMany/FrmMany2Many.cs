using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex3_ModelManytoMany
{
    public partial class FrmMany2Many : Form
    {
        public FrmMany2Many()
        {
            InitializeComponent();
        }

      private void FrmMany2Many_Load(object sender, EventArgs e)
      {

      }


      void fdsf()
      {

  
using (using (EFModelOne2One context = new EFModelOne2One())
         {
            txtConnection.Text = context.Database.Connection.ConnectionString;
         }
      }

      private void btnTestOne2One_Click(object sender, EventArgs e)
      {
         TestOne2One();
      }

      void TestOne2One()
      {
         txtDebug.Text = "TestOne2One()\r\n";

         using (EFModelOne2One context = new EFModelOne2One())
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
            person.DOB = Convert.ToDateTime("6/12/70", culture);

            //person.Address =  new Address(); 


            //Address.CreateAddressUnsafe(); // new Address();
            Address.Create(person); // new Address();

            person.Address.Number = "1";
            person.Address.StreetLine1 = "High St";
            person.Address.City = "Perth";
            person.Address.Country = "Australia";

            context.People.Add(person);

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
            var items = context.People;

            foreach (var x in items)
               txtDebug.Text += String.Format("{0} {1} {2} {3} {4}", x.Id, x.FirstName, x.MiddleName, x.LastName, x.Phone) + "\r\n";
         }
      }
      context = new EFModelOne2One())
         {
            txtConnection.Text = context.Database.Connection.ConnectionString;
         }
      }

      private void btnTestOne2One_Click(object sender, EventArgs e)
{
   TestOne2One();
}

void TestOne2One()
{
   txtDebug.Text = "TestOne2One()\r\n";

   using (EFModelOne2One context = new EFModelOne2One())
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
      person.DOB = Convert.ToDateTime("6/12/70", culture);

      //person.Address =  new Address(); 


      //Address.CreateAddressUnsafe(); // new Address();
      Address.Create(person); // new Address();

      person.Address.Number = "1";
      person.Address.StreetLine1 = "High St";
      person.Address.City = "Perth";
      person.Address.Country = "Australia";

      context.People.Add(person);

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
      var items = context.People;

      foreach (var x in items)
         txtDebug.Text += String.Format("{0} {1} {2} {3} {4}", x.Id, x.FirstName, x.MiddleName, x.LastName, x.Phone) + "\r\n";
   }
}
   }
}
