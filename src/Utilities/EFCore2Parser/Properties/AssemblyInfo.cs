using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("EFCore2Parser")]
[assembly: AssemblyDescription("Parser for EFCore v2.x assemblies")]
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

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("3.0.4")]
[assembly: AssemblyFileVersion("3.0.4")]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]
