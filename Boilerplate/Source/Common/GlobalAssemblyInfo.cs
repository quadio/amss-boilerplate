using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("MockCompany, Inc.")]
[assembly: AssemblyCopyright("© Copyright 2012, MockCompany, Inc.")]
[assembly: AssemblyProduct("Amss.Boilerplate")]

#if DEBUG

[assembly: AssemblyConfiguration("debug")]
#else
[assembly: AssemblyConfiguration("retail")]
#endif

[assembly: ComVisible(false)]