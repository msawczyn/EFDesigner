using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel.DslPackage
{
   public class OptionsPage : Microsoft.VisualStudio.Shell.DialogPage
   {
      [Category("Display")]
      [DisplayName("GraphViz dot.exe path")]
      [Description("Path to the GraphViz dot.exe file, if present")]
      [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
      public string DotExePath { get; set; }

      [Category("Code Generation")]
      [DisplayName("Default string length")]
      [Description("Initial length of all string properties. Can be blank for no default")]
      public int? DefaultStringLength { get; set; }
   }
}
