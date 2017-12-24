using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using NSCommon.Logging;

// -xtransform\makenuspec.xslt -o test.xml test.nuspec -a boss_id=test -o test.nuspec -f tester.dll
namespace NSXslTransform {
    public static class driver {
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

          static void showUsage(int exitCode) {
            Console.Error.WriteLine("usage: " + Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]) +
                " [-v] [-a key=value]... [-f assemblyPathToUse] [-o outputfile] -x transform-file input-file");
            Environment.Exit(exitCode);
        }
        #endregion

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
                }
                return _xws;
            }
        }
        #endregion

        static int processArgs(XTArgs args) {
            int ret = 0;
            XslCompiledTransform t;
            XsltArgumentList argsList;
            string ext;

            try {
                t = new XslCompiledTransform(true);

                argsList = new XsltArgumentList();
                foreach (string akey in args.arguments.Keys)
                    argsList.AddParam(akey, string.Empty, args.arguments[akey]);

                MiniLogger.log(MethodBase.GetCurrentMethod(), "using transform: " + args.transformFile);
                t.Load(args.transformFile);

                MiniLogger.log(MethodBase.GetCurrentMethod(), "transforming " + args.inputFile);

                if (string.IsNullOrEmpty(args.outputFile)) {
                    t.Transform(args.inputFile, argsList, Console.Out);
                } else {
                    ext = Path.GetExtension(args.outputFile);
                    if (string.Compare(ext, ".xml", true) == 0 ||
                        string.Compare(ext, ".nuspec", true) == 0) {
                        using (XmlWriter xw = XmlWriter.Create(args.outputFile, settings)) {
                            t.Transform(args.inputFile, argsList, xw);
                        }
                    } else {
                        using (StreamWriter sw = new StreamWriter(args.outputFile)) {
                            t.Transform(args.inputFile, argsList, sw);
                            //sw.WriteLine(Environment.NewLine);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(args.outputFile))
                    MiniLogger.log(MethodBase.GetCurrentMethod(), Environment.NewLine + "transformed into :" +
                        (string.IsNullOrEmpty(args.outputFile) ? "<stdout>" : args.outputFile));
            } catch (Exception ex) {
                MiniLogger.log(MethodBase.GetCurrentMethod(), ex);
                ret = 1;
            }
            return ret;
        }
    }
}