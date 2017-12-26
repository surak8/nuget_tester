using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NSTester;

namespace NSTestDriver {
    class CmdLineArgs {
        IDictionary<string, PropertyInfo> propMap;

        internal CmdLineArgs(Type atype) {
            propMap = makePropertyMap(typeof(NugetMetadata));
        }

        public bool doDefault { get; private set; }
        public bool showHelp { get; internal set; }

        internal List<string> parseCmdlineArgs(string[] args) {
            List<string> ret = new List<string>();
            int nargs, arglen;
            string anArg, desc, relNotes;

            desc = relNotes = null;

            if (args != null && (nargs = args.Length) > 0) {
                #region CMDLINE
                for (int i = 0 ; i < nargs ; i++) {
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
                                        case 'h':
                                        case '?': showHelp = true; break;
                                        default: Console.Error.WriteLine("unhandled: " + anArg[j]); break;
                                    }
                                }
                            }
                        }else {
                            Logger.log(MethodBase.GetCurrentMethod());
                        }
                        //Trace.WriteLine("here");
                    }else {
                        ret.Add(anArg);
                    }
                    //         else
                    //if ((pkg = tc.readPackage(anArg)) != null) {
                    //             //Trace.WriteLine("here");
                    //             if (doDefault) {
                    //                 pkg.metadata.iconUrl = null;
                    //                 pkg.metadata.projectUrl = null;
                    //                 pkg.metadata.tags = null;
                    //                 pkg.metadata.licenseUrl = null;
                    //                 pkg.metadata.releaseNotes = null;
                    //                 pkg.metadata.description = null;
                    //                 pkg.metadata.dependencies.Clear();
                    //                 pkg.metadata.frameworkAssemblies.Clear();
                    //                 needsSave = true;
                    //             }
                    //             if (desc != null) {
                    //                 pkg.metadata.description = desc;
                    //                 needsSave = true;
                    //             }
                    //             if (relNotes != null) {
                    //                 pkg.metadata.releaseNotes = relNotes;
                    //                 needsSave = true;
                    //             }
                    //             if (needsSave)
                    //                 tc.savePackage(anArg, pkg);
                    //         }
                }
                #endregion
            }
            return ret;
        }

        internal bool applyChanges(NugetPackage pkg) {
            Logger.log(MethodBase.GetCurrentMethod());
            return false;
        }

        internal void showHelpMessage(TextWriter error) {
            Logger.log(MethodBase.GetCurrentMethod());
        }

        static IDictionary<string, PropertyInfo> makePropertyMap(Type type) {
            IDictionary<string, PropertyInfo> ret = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] ava2r = typeof(NugetMetadata).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            ret = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo pi in ava2r)
                ret.Add(pi.Name, pi);
            return ret;
        }

    }
}