using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Ex3_ModelOnetoMany;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

// To Enable-> optionsBuilder.UseLazyLoadingProxies();
// <Change from EF6>
// Required for Bi directional 1 to many loading
// using Microsoft.EntityFrameworkCore.Proxies;
// Install-Package Microsoft.EntityFrameworkCore.Proxies

namespace Ex3_ModelManytoMany
{
    public partial class FrmModelOnetoMany : Form
    {

        DbContextOptionsBuilder<EFModelOnetoMany> optionsBuilder;

        public FrmModelOnetoMany()
        {
            InitializeComponent();
            optionsBuilder = new DbContextOptionsBuilder<EFModelOnetoMany>();
            optionsBuilder.UseSqlServer(EFModelOnetoMany.ConnectionString);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        private void FrmMany2Many_Load(object sender, EventArgs e)
        {
            using (EFModelOnetoMany context = new EFModelOnetoMany(optionsBuilder.Options))
            {
                txtConnection.Text = EFModelOnetoMany.ConnectionString;
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

            using (EFModelOnetoMany context = new EFModelOnetoMany(optionsBuilder.Options))
            {
                // Perform data access using the context

                context.Database.EnsureDeleted();
                txtDebug.Text += "Deleted DB\r\n";

                context.Database.EnsureCreated();
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
                context.Books.Add(book);

                Book book1 = Book.Create(AuthorIA);
                book1.Title = "Youth";
                book1.ISBN = "43252345243";
                context.Books.Add(book1);

                Book book2 = Book.Create(AuthorMT);
                book2.Title = "The Adventures of Huckleberry Finn";
                book2.ISBN = "6436546345";
                context.Books.Add(book2);

                Book book3 = Book.Create(AuthorMT);
                book3.Title = "The Prince and the Pauper";
                book3.ISBN = "34523452";
                context.Books.Add(book3);

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
            }

            using (EFModelOnetoMany readback = new EFModelOnetoMany(optionsBuilder.Options))
            {

                // Read it back - need to enable lazy loading!  See top of source
                // Install-Package Microsoft.EntityFrameworkCore.Proxies
                var AllAuthors = readback.Authors.ToList();

                foreach (Author author in readback.Authors)
                {
                    txtDebug.Text += "---------\r\n";
                    txtDebug.Text += $"Author: {author.Firstname} {author.Lastname}\r\n";


                    foreach (Book thisbook in author.Books)
                    // System.InvalidOperationException: 'There is already an open DataReader associated with this Connection which must be closed first.'
                    // This is only single reader -> 'Data Source = (localdb)\mssqllocaldb; Initial Catalog = EFLocalDb; Integrated Security = True'
                    // need multiple, missing 'MultipleActiveResultSets=true'
                    // so -> 'Data Source=(localdb)\mssqllocaldb;Initial Catalog=EFVisualExamples;MultipleActiveResultSets=true;Integrated Security=True'
                    {
                        txtDebug.Text += $"Book Title:{thisbook.Title} - {thisbook.ISBN}\r\n";
                    }

                }
            }
        }
    }
}