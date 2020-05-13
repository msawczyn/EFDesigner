#region Using directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

[assembly: AssemblyTitle("Entity Framework Visual Designer")]
[assembly: AssemblyDescription("Entity Framework visual design surface and code-first code generation for EF6, EFCore and beyond")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Michael Sawczyn")]
[assembly: AssemblyProduct("EFDesigner")]
[assembly: AssemblyCopyright("Copyright © Michael Sawczyn 2017-2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

[assembly: AssemblyVersion("2.0.4.0")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]