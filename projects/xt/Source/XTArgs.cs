using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NSCommon.Logging;

namespace NSXslTransform {
    class XTArgs {

        internal XTArgs(string[] args) {
            int i, n, arglen;
            string arg, argValues, filename, value;
            char achar;
            FileVersionInfo fvi;

            arguments = new Dictionary<string, string>();
            n = args.Length;

            i = 0;
            while (i < n) {
                arg = args[i];
                arglen = arg.Length;
                if (arg[0] == '-' || arg[0] == '/') {
                    if (arglen >= 2) {
                        switch (achar = arg[1]) {
                            case 'a': // xslt arguments
                                if (arglen > 2) argValues = arg.Substring(2);
                                else argValues = args[++i];

                                if (!string.IsNullOrEmpty(argValues))
                                    parseXsltArguments(argValues);
                                break;
                            case 'f': // file
                                if (arglen > 2) filename = arg.Substring(2);
                                else filename = args[++i];
                                if (!File.Exists(filename))
                                    throw new ApplicationException("Invalid assembly specified.");
                                if (!string.IsNullOrEmpty(filename)) {
                                    fvi = FileVersionInfo.GetVersionInfo(filename);
                                    MiniLogger.log(MethodBase.GetCurrentMethod());
                                    if (!string.IsNullOrEmpty(value = fvi.ProductName))
                                        addArgument("idValue", value);
                                    if (!string.IsNullOrEmpty(value = fvi.FileDescription))
                                        addArgument("descValue", value);
                                    if (!string.IsNullOrEmpty(value = fvi.FileVersion))
                                        addArgument("versionValue", value);
                                    if (!string.IsNullOrEmpty(value = fvi.CompanyName))
                                        addArgument("authorsValue", value);
                                }

                                break;
                            case 'O': this.overwrite = true; break;
                            case 'o': // outputfile
                                if (arglen > 2) outputFile = arg.Substring(2);
                                else this.outputFile = args[++i];

                                break;
                            case 'v': verbose = true; break;
                            case 'x':
                                if (arglen > 2) {
                                    transformFile = arg.Substring(2);
                                    i++;
                                } else transformFile = args[++i];

                                break;
                            default: Console.Error.WriteLine("unknown flag:" + achar); break;
                        }
                    } else {
                        MiniLogger.log(MethodBase.GetCurrentMethod(), "short version");
                    }
                } else {
                    this.inputFile = args[i];
                }
                i++;
            }
        }

        void parseXsltArguments(string argValues) {
            //MiniLogger.log(MethodBase.GetCurrentMethod());
            string[] argPairs;
            string tmp, key, value;
            int pos;

            if (!string.IsNullOrEmpty(argValues)) {
                if ((argPairs = argValues.Split(';')) != null && argPairs.Length > 0) {
                    foreach (string apair in argPairs) {
                        tmp = apair.Trim();
                        if ((pos = tmp.IndexOf('=')) > 0) {
                            key = tmp.Substring(0, pos).Trim();
                            value = tmp.Substring(pos + 1).Trim();
                            addArgument(key, value);
                        }
                    }
                }
            }
        }

        void addArgument(string key, string value) {
            if (!this.arguments.ContainsKey(key)) {
                if (verbose)
                    MiniLogger.log("Adding " + key + " = " + value);
                arguments.Add(key, value);
            } else {
                if (overwrite) {
                    if (verbose)
                        MiniLogger.log("Replacing old value of " + key + " from " + arguments[key] + " to " + value);
                    arguments[key] = value;
                    //arguments.Add(key, value);
                } else
                    MiniLogger.log(
                        MethodBase.GetCurrentMethod(),
                        "duplicate value for key: " + key + " [prev=" + arguments[key] + "], new value=" + value);
            }
        }

        public bool error { get; private set; }
        public bool showHelp { get; private set; }
        public IDictionary<string, string> arguments { get; private set; }
        public string inputFile { get; private set; }
        public string outputFile { get; private set; }
        public string transformFile { get; private set; }
        public bool verbose { get; private set; }
        public bool overwrite { get; private set; }
    }
}