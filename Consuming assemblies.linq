<Query Kind="Statements" />

string pathToAssembly = @"C:\Code\IncludeFitness-Cloud\Include.Repository\bin\Debug\Include.Repository.dll";
AssemblyName assemblyName = AssemblyName.GetAssemblyName(pathToAssembly);

string assemblyDirectory = Path.GetDirectoryName(pathToAssembly);
//AppDomain domain = AppDomain.CreateDomain("working", null, new AppDomainSetup { ApplicationBase = assemblyDirectory });
AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;

Assembly loadedAssembly = Assembly.ReflectionOnlyLoadFrom(pathToAssembly);
IEnumerable<Type> theTypes = loadedAssembly.GetExportedTypes().ToList();

foreach (Type type in theTypes)
{
	Console.WriteLine((type.BaseType?.FullName ?? "") + "\t\t" + type.FullName);
}

//System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs args)
//{
//	Console.WriteLine($"*** {args.Name}");
//	AssemblyName name = new AssemblyName(args.Name);
//	
//	try
//	{
//		return System.Reflection.Assembly.ReflectionOnlyLoadFrom(args.Name);
//	}
//	catch
//	{
//		string dependentAssemblyPath = Path.Combine(assemblyDirectory, name.Name) + ".dll";
//		return System.Reflection.Assembly.ReflectionOnlyLoadFrom(dependentAssemblyPath);
//	}
//}

System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs args)
{
	Assembly dep = null;

	try
	{
		/// Try to load the dependency from the same location
		/// than the original assembly. We need to keep track
		/// about the source directory.
		dep = Assembly.ReflectionOnlyLoadFrom(Path.Combine(assemblyDirectory, args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll"));

		if (dep != null)
			return dep;
	}
	catch (System.IO.FileNotFoundException)
	{
		dep = null;
	}

	try
	{
		/// Try to load from the GAC.
		dep = Assembly.ReflectionOnlyLoad(args.Name);

		if (dep != null)
			return dep;
	}
	catch (System.IO.FileLoadException)
	{
		dep = null;
	}


	return null;
}
