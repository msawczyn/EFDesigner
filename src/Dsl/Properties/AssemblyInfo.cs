#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle(@"EntityFramework Designer")]
[assembly: AssemblyDescription(@"")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(@"Michael Sawczyn")]
[assembly: AssemblyProduct(@"EFDesigner")]
[assembly: AssemblyCopyright("(c) 2017 Michael Sawczyn")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion(@"1.0.3.4")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

//
// Make the Dsl project internally visible to the DslPackage assembly
//
[assembly: InternalsVisibleTo(@"Sawczyn.EFDesigner.EFModel.DslPackage, PublicKey=00240000048000009400000006020000002400005253413100040000010001006B3838060EB00642AFEBAF62BCF85AD8BC0743AADC6CFA7EA389DCF853B85157A5A618B494B8DCA7A99D3E49880D68B58D4C75051C2B5E0FFDCC622C34D78CA73E81C0554740474824926DFD6E4451B4A8F674426BB9CDE6C8684151667DDF87ED0CE8161CB8400CD024CE1C9529CF83F94363C8ACF1A75692AC04F5DB4AE7AB")]