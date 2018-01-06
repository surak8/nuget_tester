using System.Reflection;
using System.Runtime.InteropServices;

[assembly:AssemblyTitle("FakeTask")]
[assembly:AssemblyProduct("FakeTask")]
[assembly:AssemblyDescription("description of FakeTask.")]
[assembly:AssemblyCompany("Rik Cousens")]
[assembly:AssemblyCopyright("Copyright © 2018, Rik Cousens")]
#if DEBUG
[assembly:AssemblyConfiguration("Debug assemblyVersion")]
#else
[assembly:AssemblyConfiguration("Release assemblyVersion")]
#endif
[assembly:ComVisible(false)]

[assembly:AssemblyVersion("1.0.0.0")]
[assembly:AssemblyFileVersion("1.0.0.0")]
[assembly:AssemblyInformationalVersion("1.0.0.0")]

