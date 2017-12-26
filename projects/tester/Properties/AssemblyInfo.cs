using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("tester")]
[assembly: AssemblyProduct("tester")]
[assembly: AssemblyDescription("description of tester.")]
[assembly: AssemblyCompany("Rik Cousens")]
[assembly: AssemblyCopyright("Copyright © 2017, Rik Cousens")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug version")]
#else
[assembly:AssemblyConfiguration("Release version")]
#endif
[assembly: ComVisible(false)]