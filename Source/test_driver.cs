using System;
using System.Diagnostics;
using NSTester;

namespace NSTestDriver {
    public class test_driver {

        public static void Main(string[] args) {
            int exitCode = 0;
            string err;
            TesterClass tc;

            if (args.Length < 1) {
                Console.Error.WriteLine("no args");
                exitCode = 1;
            } else {
                try {
                    tc = new TesterClass();
                    foreach (string anArg in args)
                        tc.readPackage(anArg);
                } catch (Exception ex) {
                    err = TesterClass.decompose(ex);
                    Console.Error.WriteLine(err);
                    Trace.WriteLine(err);
                    exitCode = 2;
                }
            }
            Environment.Exit(exitCode);
        }
    }
}