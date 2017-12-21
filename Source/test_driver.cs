using System;
using System.Diagnostics;
using NSTester;

namespace NSTestDriver {
    public class test_driver {

        public static void Main(string[] args) {
            int exitCode = 0;
            string err,anArg,desc=null,relNotes=null;
            TesterClass tc;
            bool doDefault = false,needsSave=false;
            int arglen,nargs;
            NugetPackage pkg;

            if ((nargs=args.Length ) < 1) {
                Console.Error.WriteLine("no args");
                exitCode = 1;
            } else {
                try {
                    tc = new TesterClass();
                    for (int i=0 ;i<nargs ;i++) {
                        anArg = args[i];
                    //foreach (string anArg in args) {
                        if (anArg[0] == '-' || anArg[1] == '/') {
                            arglen = anArg.Length;
                            if (arglen > 1) {
                                if (string.Compare(anArg.Substring(1), "description", true) == 0) {
                                    //Trace.WriteLine("here");
                                    if (i + 1 > nargs)
                                        throw new ApplicationException("ugh, outside range");
                                    desc = args[i + 1];
                                    i++;
                                } else if (string.Compare(anArg.Substring(1), "releasenotes", true) == 0) {
                                    //Trace.WriteLine("here");
                                    if (i + 1 > nargs)
                                        throw new ApplicationException("ugh, outside range");
                                    relNotes = args[i + 1];
                                    i++;
                                } else {
                                    for (int j = 1 ; j < arglen ; j++) {
                                        switch (anArg[j]) {
                                            case 'd': doDefault = true; break;
                                            default: Console.Error.WriteLine("unhandled: " + anArg[j]); break;
                                        }
                                    }
                                }
                            }
                            //Trace.WriteLine("here");
                        } else

               if ((pkg = tc.readPackage(anArg)) != null) {
                            //Trace.WriteLine("here");
                            if (doDefault) {
                                pkg.metadata.iconUrl = null;
                                pkg.metadata.projectUrl = null;
                                pkg.metadata.tags = null;
                                pkg.metadata.licenseUrl = null;
                                pkg.metadata.releaseNotes = null;
                                pkg.metadata.description = null;
                                pkg.metadata.dependencies.Clear();
                                pkg.metadata.frameworkAssemblies.Clear();
                                needsSave = true;
                            }
                            if (desc != null) {
                                pkg.metadata.description = desc;
                                needsSave = true;
                            }
                            if (relNotes != null) {
                                pkg.metadata.releaseNotes = relNotes;
                                needsSave = true;
                            }
                            if (needsSave )
                                tc.savePackage(anArg, pkg);
                        }
                    }
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