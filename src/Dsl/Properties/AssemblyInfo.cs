using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

[assembly: AssemblyTitle("Entity Framework Visual Designer")]
[assembly: AssemblyDescription("Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Michael Sawczyn")]
[assembly: AssemblyProduct("EFDesigner")]
[assembly: AssemblyCopyright("Copyright © Michael Sawczyn 2017-2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("3.0.4")]
[assembly: AssemblyFileVersion("3.0.4")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

[assembly: InternalsVisibleTo("Sawczyn.EFDesigner.EFModel.DslPackage, PublicKey=00240000048000009400000006020000002400005253413100040000010001006B3838060EB00642AFEBAF62BCF85AD8BC0743AADC6CFA7EA389DCF853B85157A5A618B494B8DCA7A99D3E49880D68B58D4C75051C2B5E0FFDCC622C34D78CA73E81C0554740474824926DFD6E4451B4A8F674426BB9CDE6C8684151667DDF87ED0CE8161CB8400CD024CE1C9529CF83F94363C8ACF1A75692AC04F5DB4AE7AB")]
