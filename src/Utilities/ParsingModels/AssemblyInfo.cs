using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("ParsingModels")]
[assembly: AssemblyDescription("Shared data models for DbContext parsing")]
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

[assembly: ComVisible(false)]

[assembly: Guid("fb2035a3-09f5-43ff-8545-3af8b814b405")]

[assembly: AssemblyVersion("4.1.2.0")]
[assembly: AssemblyFileVersion("4.1.2.0")]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]
