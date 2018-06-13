using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using NSCommon.Logging;

// -xtransform\makenuspec.xslt -o test.xml test.nuspec -a boss_id=test -o test.nuspec -f tester.dll
// -v -f tester.dll -x transform\DefaultPackage.xslt xt.xml -o default.nuspec -a idValue=riktest
// -f tester.dll -x transform\defaultpackage.xslt -v
// -O -v -f tester.dll -x transform\DefaultPackage.xslt xt.xml -o default.nuspec -a idValue=riktest
// -v -x transform\DefaultPackage.xslt test.txt
// -v -x transform\DefaultPackage.xslt test.xml -a idValue=zzz;descValue=desc;authorsValue=a;versionValue=v


namespace NSXslTransform {
    public static class driver {
        #region fields
        static XmlWriterSettings _xws;
        #endregion

        #region properties
        static XmlWriterSettings settings {
            get {
                if (_xws == null) {
                    _xws = new XmlWriterSettings();
                    _xws.Indent = true;
                    _xws.IndentChars = new string(' ', 4);
                    _xws.OmitXmlDeclaration = true;
                    _xws.Encoding = Encoding.ASCII;
                    //_xws.ConformanceLevel = ConformanceLevel.Auto;
                }
                return _xws;
            }
        }
        #endregion

        #region methods
        #region main-line methods
        [STAThread]
        public static void Main(string[] args) {
            int exitCode = 0;
            XTArgs xtargs = new XTArgs(args);

            try {
                if (string.IsNullOrEmpty(xtargs.inputFile)) {
                    Console.Error.WriteLine("missing input-file");
                    showUsage(1);
                    //throw new ApplicationException("input-file is empty!");
                }
                if (string.IsNullOrEmpty(xtargs.transformFile)) {
                    Console.Error.WriteLine("missing transformation-file");
                    showUsage(1);
                    //throw new ApplicationException("transformationb-file is empty!");
                }
                exitCode = processArgs(xtargs);
            } catch (ApplicationException ae) {
                MiniLogger.log(MethodBase.GetCurrentMethod(), ae);
            } catch (Exception ex) {
                MiniLogger.log(MethodBase.GetCurrentMethod(), ex);
            }
            Environment.Exit(exitCode);
        }
        #endregion

        static int processArgs(XTArgs args) {
            int ret = 0;
            XslCompiledTransform t;
            XsltArgumentList argsList;

            try {
                t = new XslCompiledTransform(true);

                argsList = new XsltArgumentList();
                foreach (string akey in args.arguments.Keys)
                    argsList.AddParam(akey, string.Empty, args.arguments[akey]);

                if (args.verbose)
                    MiniLogger.log("[VERBOSE] " + "using transform: " + args.transformFile);
                t.Load(args.transformFile);

                if (args.verbose)
                    MiniLogger.log("[VERBOSE] " + "transforming " + args.inputFile);

                XmlWriter xw = null;

                if (string.IsNullOrEmpty(args.outputFile)) {
                    settings.CloseOutput = false;
                    settings.ConformanceLevel = ConformanceLevel.Auto;
                    xw = XmlWriter.Create(Console.Out, settings);
                } else {
                    xw = XmlWriter.Create(args.outputFile, settings);
                }
                if (xw != null) {
                    t.Transform(args.inputFile, argsList, xw);
                    xw.Close();
                    xw.Dispose();
                    xw = null;
                }
                if (args.verbose) {
                    if (string.IsNullOrEmpty(args.outputFile))
                        Console.Out.WriteLine();
                    MiniLogger.log("[VERBOSE] " + "transformed into :" +
                            (string.IsNullOrEmpty(args.outputFile) ? "<stdout>" : args.outputFile));
                }
            } catch (XmlException xe) {
                ret = 1;
                throw new ApplicationException("XML Exception ", xe);
            } catch (DirectoryNotFoundException dnfe) {
                ret = 1;
                throw new ApplicationException("Directory not found", dnfe);

            } catch (Exception ex) {
                MiniLogger.log(MethodBase.GetCurrentMethod(), ex);
                ret = 1;
            }
            return ret;
        }

        static void showUsage(int exitCode) {
            Console.Error.WriteLine("usage: " + Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]) +
                " [-Ov] [-a key=value]... [-f assemblyPathToUse] [-o outputfile] -x transform-file input-file");
            Console.Error.WriteLine("-O\tOverrite derived arguments from -a command.");
            Console.Error.WriteLine("-v\tverbose processing");
            Environment.Exit(exitCode);
        }
        #endregion
    }
}