using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("EF6Parser")]
[assembly: AssemblyDescription("Parser for Entity Framework v6.x assemblies")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Michael Sawczyn")]
[assembly: AssemblyProduct("EFDesigner")]
[assembly: AssemblyCopyright("Copyright © Michael Sawczyn 2017-2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("4.1.2.0")]
[assembly: AssemblyFileVersion("4.1.2.0")]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]
