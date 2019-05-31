using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ChooseForm : Form
   {
      public ChooseForm()
      {
         InitializeComponent();
      }

      public string Title
      {
         get { return lblTitle.Text; }
         set { lblTitle.Text = value; }
      }

      public string Selection
      {
         get { return lbChoices.Text; }
      }

      private void LbChoices_SelectedIndexChanged(object sender, EventArgs e)
      {
         btnOK.Enabled = lbChoices.SelectedIndex >= 0;
      }

      public void SetChoices(IEnumerable<string> choices)
      {
         lbChoices.Items.Clear();

         if (choices == null)
            return;

         // ReSharper disable once CoVariantArrayConversion
         lbChoices.Items.AddRange(choices.ToArray());
      }

      private void LbChoices_DoubleClick(object sender, EventArgs e)
      {
         DialogResult = DialogResult.OK;
      }

      private void BtnOK_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.OK;
      }

      private void BtnCancel_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.Cancel;
      }
   }
}