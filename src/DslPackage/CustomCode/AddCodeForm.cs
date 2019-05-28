using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class AddCodeForm : Form
   {
      public AddCodeForm()
      {
         InitializeComponent();
      }

      public AddCodeForm(ModelClass element) : this()
      {
         lblClassName.Text = element.Name;
         txtCode.Lines = element.Attributes.Select(x => $"{x};").ToArray();
         txtCode.AutoCompleteCustomSource.AddRange(element.ModelRoot.ValidTypes);
      }

      public AddCodeForm(ModelEnum element) : this()
      {
         lblClassName.Text = element.Name;
         txtCode.Lines = element.Values.Select(x => x.ToString()).ToArray();
         // ReSharper disable once VirtualMemberCallInConstructor
         Text = "Add values as code";
         label1.Text = "Enum name";
         label2.Text = "Values";
         txtCode.AutoCompleteCustomSource.AddRange(element.ModelRoot.ValidTypes);
      }

      public IEnumerable<string> Lines
      {
         get
         {
            return txtCode.Lines.Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();
         }
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.OK;
      }
   }
}
