using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   public partial class AddCodeForm : Form
   {
      public AddCodeForm()
      {
         InitializeComponent();
         txtCode.AutoCompleteCustomSource.AddRange(ModelAttribute.ValidTypes);
      }

      public AddCodeForm(ModelClass element) : this()
      {
         lblClassName.Text = element.Name;
         txtCode.Lines = element.Attributes.Select(x => $"{x};").ToArray();
      }

      public IEnumerable<string> Lines => txtCode.Lines;

      private void btnOk_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.OK;
      }
   }
}
