using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;

namespace Sawczyn.EFDesigner.EFModel
{
   internal class ConnectionStringEditor : UITypeEditor
   {
      public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
      {
         return UITypeEditorEditStyle.Modal;
      }

      public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
      {
         using (DataConnectionDialog dialog = new DataConnectionDialog())
         {
            DataSource.AddStandardDataSources(dialog);

            if (string.IsNullOrEmpty((string)value))
            {
               dialog.SelectedDataSource = DataSource.SqlDataSource;
               dialog.SelectedDataProvider = DataProvider.SqlDataProvider;
            }
            else
            {
               dialog.ConnectionString = value.ToString();
            }

            return DataConnectionDialog.Show(dialog) == DialogResult.OK ? dialog.ConnectionString : value;
         }
      }
   }
}
