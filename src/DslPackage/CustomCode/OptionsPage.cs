using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

using Microsoft.VisualStudio.Shell;

namespace Sawczyn.EFDesigner.EFModel.DslPackage
{
   public class OptionsPage : DialogPage
   {
      [Category("Display")]
      [DisplayName("GraphViz dot.exe path")]
      [Description("Path to the GraphViz dot.exe file, if present")]
      [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
      public string DotExePath { get; set; }

      [Category("File")]
      [DisplayName("Save diagram using legacy binary format")]
      [Description("If true, .diagramx files will be saved in compressed format. If false, they will not.")]
      public bool SaveDiagramsCompressed { get; set; }

      [Category("Code Generation")]
      [DisplayName("Indent with tabs instead of spaces")]
      [Description("If true, code will be generated using tabs and obey the editor's tab width")]
      public bool UseTabs { get; set; }
   }
}