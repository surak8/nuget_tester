using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using NSCommon.Logging;
using NSTester;

namespace NSTestDriver {
    public class test_driver {

        public static void Main(string[] args) {
            int exitCode = 0;
            //string err, anArg, desc = null, relNotes = null;
            TesterClass tc;
            NugetPackage pkg;
            CmdLineArgs ca;
            //bool doDefault = false, needsSave = false;
            bool needsSave = false,otherSave;
            int nargs;
            List<string> files;
            string err;

            if ((nargs = args.Length) < 1) {
                Console.Error.WriteLine("no args");
                exitCode = 1;
            } else {

                try {
                    ca = new CmdLineArgs(typeof(NugetMetadata));
                    files = ca.parseCmdlineArgs(args);
                    if (ca.showHelp) {
                        ca.showHelpMessage(Console.Error);
                        //Console.Error.WriteLine("no args");
                        exitCode = 1;
                    } else {
                        if (files.Count < 1) {
                            Console.Error.WriteLine("no args");
                            exitCode = 1;
                        } else {
                            tc = new TesterClass();
                            foreach (string aFile in files) {
                                if ((pkg = tc.readPackage(aFile)) != null) {
                                    if (ca.doDefault) {
                                        pkg.resetValues();
                                        needsSave = true;
                                    }
                                    
                                    otherSave = ca.applyChanges(pkg);
                                    //nnedspkg.applyChanges(ca);
                                }
                                if (needsSave) {
                                    //tc.savePackage(aFile, pkg);
                                    tc.savePackage(
                                        Path.Combine (
                                            Path.GetDirectoryName (aFile ),
                                            Path.GetFileNameWithoutExtension(aFile )+".fix"+
                                            Path.GetExtension (aFile )), 
                                        pkg);
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                    err = MiniLogger.decompose(ex);
                    Console.Error.WriteLine(err);
                    Trace.WriteLine(err);
                    exitCode = 2;
                }
            }
            Environment.Exit(exitCode);
        }

        static void showHelp(IDictionary<string, PropertyInfo> propMap, TextWriter error, int v) {
            //    throw new NotImplementedException();
            //}

            //static void showHelp(IDictionary<string, PropertyInfo> propMap,TextWriter tw) {
            Assembly a = Assembly.GetEntryAssembly();
            AssemblyName an = a.GetName();
            StringBuilder sb = new StringBuilder();

            sb.Append("usage: " + Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]));

        }

    }


    /// <summary>logging class.</summary>
    public static class Logger {

        #region fields
        /// <summary>controls logging-style.</summary>
        public static bool logDebug = false;

        /// <summary>controls logging-style.</summary>
        public static bool logUnique = false;

        /// <summary>messages written.</summary>
        static readonly List<string> msgs = new List<string>();
        #endregion fields

        #region methods
        #region logging-methods
        /// <summary>log a message</summary>
        /// <param name="msg"/>
        /// <seealso cref="Debug"/>
        /// <seealso cref="Trace"/>
        /// <seealso cref="logDebug"/>
        /// <seealso cref="logUnique"/>
        /// <seealso cref="msgs"/>
        public static void log(string msg) {
            if (logUnique) {
                if (msgs.Contains(msg))
                    return;
                msgs.Add(msg);
            }
            if (logDebug)
#if DEBUG
                Debug.Print("[DEBUG] " + msg);
#endif

#if TRACE
            Trace.WriteLine("[TRACE] " + msg);
#endif
        }

        /// <summary>log a message</summary>
        /// <param name="mb"/>
        /// <seealso cref="makeSig"/>
        /// <seealso cref="log(MethodBase,string)"/>
        public static void log(MethodBase mb) {
            log(mb, string.Empty);
        }

        /// <summary>log a message</summary>
        /// <param name="mb"/>
        /// <param name="msg"/>
        /// <seealso cref="makeSig"/>
        /// <seealso cref="log(MethodBase,string)"/>
        public static void log(MethodBase mb, string msg) {
            log(makeSig(mb) + ":" + msg);
        }

        public static void log(MethodBase mb, Exception ex) {
            log(makeSig(mb) + ":" + ex.Message);
        }
        #endregion logging-methods

        #region misc. methods
        /// <summary>create a method-signature.</summary>
        /// <returns></returns>
        public static string makeSig(MethodBase mb) {
            return mb.ReflectedType.Name + "." + mb.Name;
        }
        #endregion misc. methods
        #endregion methods
    }

}