using System;
using NSTester;

namespace NSTestDriver {
    public class test_driver {

        public static void Main(string[] args) {
            int exitCode = 0;

            TesterClass tc;

            try {
                tc = new TesterClass();
                tc.readPackage("docs\\simple.nuspec");
            }catch(Exception ex) {
                Console.Error.WriteLine(ex.Message);
                exitCode = 1;

            }
            Environment.Exit(exitCode);
        }
    }
}