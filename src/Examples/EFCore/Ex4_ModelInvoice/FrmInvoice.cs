using System;
using System.Windows.Forms;

using System.Data.Entity.Validation;
using Microsoft.EntityFrameworkCore;

// Error CS1061	'DbContextOptionsBuilder' does not contain a definition for 'UseLazyLoadingProxies'
// and no accessible extension method 'UseLazyLoadingProxies' accepting a first argument of type
// 'DbContextOptionsBuilder' could be found (are you missing a using directive or an assembly reference?)	


namespace Ex4_ModelInvoice
{
    public partial class FrmInvoice : Form
    {
        DbContextOptionsBuilder<AccountingSystemModel> optionsBuilder;

        public FrmInvoice()
        {
            InitializeComponent();
            optionsBuilder = new DbContextOptionsBuilder<AccountingSystemModel>();
            optionsBuilder.UseSqlServer(AccountingSystemModel.ConnectionString);
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.EnableDetailedErrors();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            txtDebug.Text = "TestOne2One()\r\n";

            using (AccountingSystemModel db = new AccountingSystemModel(optionsBuilder.Options))
            {
                txtConnection.Text = AccountingSystemModel.ConnectionString;

                txtDebug.Text += "\r\nEx 4: Invoice - Header and Detail\r\n----------------\r\n";

                db.Database.EnsureDeleted();
                txtDebug.Text += "Deleted DB\r\n";

                db.Database.EnsureCreated();
                txtDebug.Text += "Created DB\r\n";

                InvoiceHeaders invHeader = new InvoiceHeaders();

                InvoiceDetails invDetails_Line1 = new InvoiceDetails(invHeader);
                invDetails_Line1.ItemDescription = "Item 1";
                invDetails_Line1.Price = 11.25M;
                invDetails_Line1.Quantity = 2570;
                invDetails_Line1.Total = invDetails_Line1.Price * invDetails_Line1.Quantity;

                InvoiceDetails invDetails_Line2 = new InvoiceDetails(invHeader);
                invDetails_Line2.ItemDescription = "Item 2";
                invDetails_Line2.Price = 5.25M;
                invDetails_Line2.Quantity = 1520;
                invDetails_Line2.Total = invDetails_Line2.Price * invDetails_Line2.Quantity; 

                //Associate Header and Details
                invHeader.InvoiceDetails.Add(invDetails_Line1);

                //Associate Header and Details
                invHeader.InvoiceDetails.Add(invDetails_Line2);

                invHeader.Total = invDetails_Line1.Total + invDetails_Line2.Total;

                //Save rows to Db
                db.InvoiceHeaders.Add(invHeader);
                db.InvoiceDetails.Add(invDetails_Line1); // Necessary?
                db.InvoiceDetails.Add(invDetails_Line2); // Necessary?

                try
                {
                    db.SaveChanges();
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

            using (AccountingSystemModel db = new AccountingSystemModel(optionsBuilder.Options))
            {
                //Read it back
                DbSet<InvoiceHeaders> invoices = db.InvoiceHeaders;

                foreach (InvoiceHeaders invoice in invoices)
                {
                    txtDebug.Text += $"Invoice Id {invoice.Id} - Total: {invoice.Total} \r\n"; 

                    txtDebug.Text += "--- Detail --\r\n";

                    // Error: There is already an open DataReader associated with this Command which must be closed first.
                    // or 'DbContextOptionsBuilder' does not contain a definition for 'UseLazyLoadingProxies' 
                    // Install Missing : PM> Install-Package Microsoft.EntityFrameworkCore.Proxies
                    foreach (InvoiceDetails LineItem in invoice.InvoiceDetails)
                    {
                        txtDebug.Text += $"Line Id {LineItem.Id} Details Desc:{LineItem.ItemDescription}\t Qty:{LineItem.Quantity} x Price:{LineItem.Price} = Sub Total:\t{LineItem.Total}\r\n";
                    }
                }
            }
        }
        private void FRmInvoice_Load(object sender, EventArgs e)
        {

            using (AccountingSystemModel db = new AccountingSystemModel(optionsBuilder.Options))
            {
                txtConnection.Text = AccountingSystemModel.ConnectionString;
            }
        }
    }
}