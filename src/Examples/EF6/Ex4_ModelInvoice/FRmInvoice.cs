using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex4_ModelInvoice
{
    public partial class FrmInvoice : Form
    {
        public FrmInvoice()
        {
            InitializeComponent();
        }

      private void button1_Click(object sender, EventArgs e)
      {

         txtDebug.Text = "TestOne2One()\r\n";

         using (AccountingSystemModel db = new AccountingSystemModel())
         {
            // Perform data access using the context
            db.Database.Log = Console.Write;
            db.Database.CommandTimeout = 120;

            // ---------------
            // TODO: I dont know where to set AutomaticMigrationDataLossAllowed = true in tool?
            // So doing a delete and recreate
            db.Database.Log = Console.Write;


            txtDebug.Text += "\r\nEx 4: Invoice - Header and Detail\r\n----------------\r\n";

            db.Database.Delete();
            txtDebug.Text += "Deleted DB\r\n";

            db.Database.CreateIfNotExists();
            txtDebug.Text += "Created DB\r\n";
            // -------------


            InvoiceHeaders invHeader = db.InvoiceHeaders.Create();
            InvoiceDetails invDetails = db.InvoiceDetails.Create();

            invHeader.Total = 150M;
           
            invDetails.ItemDescription = "New Item";
            invDetails.Price = 75M;
            invDetails.Quantity = 2;

            //Associate Header and Details
            invHeader.InvoiceDetails.Add(invDetails);
            db.InvoiceHeaders.Add(invHeader);

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

            //Read it back
            DbSet<InvoiceHeaders> records = db.InvoiceHeaders;

            foreach (InvoiceHeaders record in records)
            {
               txtDebug.Text += String.Format("Invoice Id {0} - Total: {1}", record.Id, record.Total) + "\r\n"; ;

               txtDebug.Text += "--- Detail --\r\n";
              
               //TODO: Error? There is already an open DataReader associated with this Command which must be closed first.
               //Cannot figure out how to get detail data?
               //foreach (InvoiceDetails details in record.InvoiceDetails)
               //{
               //  // txtDebug.Text += String.Format("Invoice Id {0} - Total: {1} |  Details Desc:{2} Qty:{3} Price:{4} Total:{5}", record.Id, record.Total, details.ItemDescription, details.Quantity, details.Price, details.Total) + "\r\n";
               //}

            }
         }
      }

      private void FRmInvoice_Load(object sender, EventArgs e)
      {

         using (AccountingSystemModel db = new AccountingSystemModel())
         {
            txtConnection.Text = db.Database.Connection.ConnectionString;
         }
      }
   }
}
