using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly:AssemblyTitle("tmp")]
[assembly:AssemblyProduct("tmp")]
[assembly:AssemblyDescription("description of tmp.")]
[assembly:AssemblyCompany("Rik Cousens")]
[assembly:AssemblyCopyright("Copyright © 2017, Rik Cousens")]
#if DEBUG
[assembly:AssemblyConfiguration("Debug assemblyVersion")]
#else
[assembly:AssemblyConfiguration("Release assemblyVersion")]
#endif
[assembly:ComVisible(false)]

[assembly:AssemblyVersion(NSTest.test.VERSION)]
[assembly:AssemblyFileVersion(NSTest.test.VERSION)]
[assembly:AssemblyInformationalVersion(NSTest.test.VERSION)]

namespace NSTest {
	public class test {
		public const string VERSION="1.0.2.0";
		[STAThread]
		public static void Main(string[] args){
			Environment.Exit(0);
		}
		
		public test(){}
		
		public int doSomething() { return 0; }
	}
}