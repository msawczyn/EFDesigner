using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   public partial class FindForm : Form
   {
      public FindForm()
      {
         InitializeComponent();
      }

      private void btnOK_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.OK;
      }

      public string FindString => txtFind.Text;
   }
}
