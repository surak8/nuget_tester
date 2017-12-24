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

[assembly:AssemblyVersion(NSTest.test.VERSION0)]
[assembly:AssemblyFileVersion(NSTest.test.VERSION1)]
[assembly:AssemblyInformationalVersion(NSTest.test.VERSION2)]

namespace NSTest {
	public class test {
        public const string VERSION0 = "1.0.3.0";
        public const string VERSION1 = "1.0.3.1";
        public const string VERSION2 = "1.0.3.2";
        [STAThread]
		public static void Main(string[] args){
			Environment.Exit(0);
		}
		
		public test(){}
		
		public int doSomething() { return 0; }
	}
}