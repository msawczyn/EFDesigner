using System;
using System.Reflection;
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
[assembly: AssemblyCopyright("Copyright � Michael Sawczyn 2017-2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("4.1.2.0")]
[assembly: AssemblyFileVersion("4.1.2.0")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]
