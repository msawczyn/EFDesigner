using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   public partial class ModelSearch : UserControl
   {
      public ModelSearch()
      {
         InitializeComponent();
      }
      public void ClearSearchText()
      {
         txtSearchText.Clear();
      }

      public string SearchText
      {
         get => txtSearchText.Text;
         set => txtSearchText.Text = value;
      }
   }

}
