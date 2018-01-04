using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NSCommon.Logging;

namespace NSRead_meta {
    class RMArgs {
        internal RMArgs(string[] args) {
            int i=0, n=args==null?0:args.Length, arglen;
            string arg;
            char achar;
            FileVersionInfo fvi;

            while (i < n) {
                arg = args[i];
                arglen = arg.Length;
                if (arg[0] == '-' || arg[0] == '/') {
                    if (arglen >= 2) {
                        switch (achar = arg[1]) {
                            case 'o': // outputfile
                                if (arglen > 2) outputFile = arg.Substring(2);
                                else this.outputFile = args[++i];
                                if (verbose)
                                    Trace.WriteLine("[VERBOSE] " + "outfile: " + this.outputFile);
                                break;
                            case 'v': verbose = true; break;
                            case '?':
                            case 'h': this.showHelp = true; break;
                            default: Console.Error.WriteLine("unknown flag:" + achar); break;
                        }
                    } else {
                        MiniLogger.log(MethodBase.GetCurrentMethod(), "short version");
                    }
                } else {
                    this.inputFile = args[i];
                    if (this.verbose && !string.IsNullOrEmpty(this.inputFile))
                        Trace.WriteLine("[VERBOSE] " + "inputFile: " + this.inputFile);
                    if (File.Exists(this.inputFile)) {
                        if (this.verbose )
                            Trace.WriteLine ("[VERBOSE] " + "processing: " +this.inputFile );
                        fvi = FileVersionInfo.GetVersionInfo(this.inputFile);
                        //Version fileVersion, productVersion;
                        description = fvi.Comments;             // AssemblyDescription
                        if (verbose && !string.IsNullOrEmpty(description))
                            Trace.WriteLine("[VERBOSE] " + "description: " + description);


                        if (string.IsNullOrEmpty(fvi.FileVersion))
                            throw new ApplicationException("file-version is null!");
                        else
                            fileVersion = new Version(fvi.ProductVersion);
                        if (string.IsNullOrEmpty(fvi.ProductVersion))
                            throw new ApplicationException("product-version is null!");
                        else
                            productVersion = new Version(fvi.ProductVersion);
                        copyright = fvi.LegalCopyright;         // AssemblyCopyright
                        if (verbose && !string.IsNullOrEmpty(copyright))
                            Trace.WriteLine("[VERBOSE] " + "copyright: " + copyright);

                        company = fvi.CompanyName;              // AssemblyCompany
                        if (verbose && !string.IsNullOrEmpty(company))
                            Trace.WriteLine("[VERBOSE] " + "company: " + company);

                        fileDescription = fvi.FileDescription;  // AssemblyTitle
                        if (verbose && !string.IsNullOrEmpty(fileDescription))
                            Trace.WriteLine("[VERBOSE] " + "fileDescription: " + fileDescription);

                        product = fvi.ProductName;              // AssemblyProduct
                        if (verbose && !string.IsNullOrEmpty(product))
                            Trace.WriteLine("[VERBOSE] " + "product: " + product);

                    }
                }
                i++;
            }
        }

        public string company { get; private set; }
        public string copyright { get; private set; }
        public string description { get; private set; }
        public string fileDescription { get; private set; }
        public Version fileVersion { get; private set; }
        public string inputFile { get; private set; }
        public string outputFile { get; private set; }
        public string product { get; private set; }
        public Version productVersion { get; private set; }
        public bool showHelp { get; private set; }
        public bool verbose { get; private set; }
    }
}