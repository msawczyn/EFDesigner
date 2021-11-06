using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex4_ModelInvoice
{
    public partial class FRmInvoice : Form
    {
        public FRmInvoice()
        {
            InitializeComponent();
        }

      private void button1_Click(object sender, EventArgs e)
      {

         using (AccountingSystemModel db = new AccountingSystemModel())
         {
            //Create new, empty entities after running SQL script generated to get model in database
            var invHeader = db.InvoiceHeaders.Create();
            var invDetails = db.InvoiceDetails.Create();

            invHeader.Total = 150M;
            invDetails.ItemDescription = "New Item";
            invDetails.Price = 75M;
            invDetails.Quantity = 2;



            //Trying to do this..as an example..
            //https://github.com/andern7/EFModelFirst/blob/master/Program.cs

            /
            invHeader.InvoiceDetails.Add(invDetails);
            db.InvoiceHeaders.Add(invHeader);
            db.SaveChanges();

            //This would be good for example 5, ordering system
            //https://github.com/ionbogatu/EFModelDesignerFirst/tree/master/EFModelDesignerFirst
         }

      }
   }
}
