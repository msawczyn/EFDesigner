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
   public partial class AddCodeForm : Form
   {
      public AddCodeForm()
      {
         InitializeComponent();
         txtCode.AutoCompleteCustomSource.AddRange(ModelAttribute.ValidTypes);
      }

      public IEnumerable<string> Lines
      {
         get { return txtCode.Lines; }
         set { txtCode.Lines = Lines.ToArray(); }
      }

      public AddCodeForm(ModelClass element) : this()
      {
         lblClassName.Text = element.Name;

         List<string> attributeList = new List<string>();
         foreach (ModelAttribute attr in element.Attributes)
         {
            string nullable = attr.Required ? "" : "?";
            string length = "";
            if (attr.Type == "String" && attr.MaxLength > 0) length = $"[{attr.MaxLength}]";
            attributeList.Add($"{attr.Type}{nullable}{length} {attr.Name};");
         }

         txtCode.Lines = attributeList.ToArray();
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
          DialogResult = DialogResult.OK;
      }
   }
}
