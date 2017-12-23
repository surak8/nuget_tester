using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using NSCommon.Logging;

namespace NSXslTransform {
    public static class driver {

#if true
        const string TRANSFORM = @"transform\MakeNuspec.xslt";
        const string SRC_FILE = "test.nuspec";
        const string RESULT = "test.xml";
#else
        const string TRANSFORM = @"transform\blah.xslt";
        const string SRC_FILE = "test.nuspec";
        const string RESULT = "testit.bat";
#endif
        public static string man_ID { get; private set; }

        [STAThread]
        public static void Main(string[] args) {
            int exitCode = 0;

            exitCode = processArgs(args);
           Environment.Exit(exitCode);
        }

        static int processArgs(string[] args) {
            int ret = 0;
            XslCompiledTransform t;
            XmlWriterSettings xws;
            string infile, outfile, transFile;

            infile = SRC_FILE;
            outfile = RESULT;
            transFile = TRANSFORM;
            man_ID = "test1";
            try {
                //var v=XslCompiledTransform .
                t = new XslCompiledTransform(true);

                XsltArgumentList argsList = new XsltArgumentList();
#if false
                argsList.AddParam("Boss_ID", "", man_ID);
#else
                transFile = "transform\\DefaultPackage.xslt";
                outfile = "default.nuspec";
                argsList.AddParam("idValue", "", "someid");
                argsList.AddParam("descValue", "", "somedesc");
                argsList.AddParam("versionValue", "", "1.0.0.0");
                argsList.AddParam("authorsValue", "", "someauthor");
#endif
                MiniLogger.log(MethodBase.GetCurrentMethod(), "using transform: " + transFile);
                t.Load(transFile);

                MiniLogger.log(MethodBase.GetCurrentMethod(), "transforming " + infile);
                if (argsList != null) {
                    xws = new XmlWriterSettings();
                    xws.Indent = true;
                    xws.IndentChars=new string(' ', 4);
                    xws.OmitXmlDeclaration = true;
                    xws.Encoding = Encoding.ASCII;
                    //xws.
                    using (XmlWriter xw = XmlWriter.Create(outfile, xws)) {
                        t.Transform(infile, argsList, xw);
                    }
                } else
                    t.Transform(infile, outfile);
                MiniLogger.log(MethodBase.GetCurrentMethod(), "transformed into :" + outfile);
            } catch (Exception ex) {
                MiniLogger.log(MethodBase.GetCurrentMethod(), ex);
                //string err = ex.Message;
                //Trace.WriteLine(err);
                //Console.Error.WriteLine(err);
                ret= 1;
            }
            return ret;
        }
    }
}