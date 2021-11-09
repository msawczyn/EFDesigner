using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Windows.Forms;

namespace Ex3_ModelManytoMany
{
   public partial class FrmModelOnetoMany : Form
    {
        public FrmModelOnetoMany()
        {
            InitializeComponent();
        }

      private void FrmMany2Many_Load(object sender, EventArgs e)
      {
         using (EFModelOnetoMany context = new EFModelOnetoMany())
         {
            txtConnection.Text = context.Database.Connection.ConnectionString;
         }
      }

      private void btnMany2Many_Click(object sender, EventArgs e)
      {
         //https://entityframework.net/one-to-many-relationship
         TestOne2Many();
      }

      void TestOne2Many()
      {
         txtDebug.Text = "TestMany2May()\r\n";

         using (EFModelOnetoMany context = new EFModelOnetoMany())
         {
            // Perform data access using the context
            context.Database.Log = Console.Write;

            context.Database.Delete();
            txtDebug.Text += "Deleted DB\r\n";

            context.Database.CreateIfNotExists();
            txtDebug.Text += "Created DB\r\n";


            txtDebug.Text += "\r\nEach Author can have many Books\r\n----------------\r\n";
            Author AuthorMT = new Author()
            {
               Firstname = "Mark",
               Lastname = "Twain"

            };


            Author AuthorIA = new Author()
            {
               Firstname = "Izzac",
               Lastname = "Azimov"

            };

           
            Book book = Book.Create(AuthorIA);
            book.Title = "The Complete Robot";
            book.ISBN = "65445635";


            Book book1 = Book.Create(AuthorIA);
            book.Title = "Youth";
            book.ISBN = "43252345243";

            Book book2 = Book.Create(AuthorMT);
            book2.Title = "The Adventures of Huckleberry Finn";
            book2.ISBN = "6436546345";

            Book book3 = Book.Create(AuthorMT);
            book3.Title = "The Prince and the Pauper";
            book3.ISBN = "34523452";
            

            context.Authors.Add(AuthorMT);
            context.Authors.Add(AuthorIA);

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
            var items = context.Authors;

            foreach (var x in items)
            {
               txtDebug.Text += "---------\r\n";
               foreach (Book books in x.Books)
               {
                  txtDebug.Text += String.Format("Auth Id {0} - Author: {1} {2} & Book Title:{3} - {4}", x.AuthorId, x.Firstname, x.Lastname, book.Title, book.ISBN ) + "\r\n";
               }

            }
               
         }
      }
   }
}