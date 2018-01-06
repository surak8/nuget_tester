//  https://msdn.microsoft.com/en-us/library/t9883dzc.aspx
// http://blog.differentpla.net/blog/2013/02/01/msbuild-tasks-input-parameters-and-itemgroups
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NSCommon.Logging;

namespace NSNugetTask {
    public class GenerateNugetSpec : ITask {
        #region fields
        public static bool verbose = false;
        ITaskHost _ith;
        IBuildEngine _ibe;
        string _asmPath;
        string _specPath;
        ITaskItem _outputpath;
        readonly List<ITaskItem> _data = new List<ITaskItem>();
        bool _result;
        #endregion
        static XmlWriterSettings _settings;

        #region ctor
        public GenerateNugetSpec() {
            _result = false;
        }
        #endregion

        #region properties
        [Required]
        public string AssemblyPath {
            get { return _asmPath; }
            set { _asmPath = value; }
        }

        [Required]
        public string NuspecPath {
            get { return _specPath; }
            set { _specPath = value; }
        }

        [Output]
        public bool BuildResult { get { return _result; } }

        [Output]
        public ITaskItem NuspecResult { get { return _outputpath; } }

        [Output]
        public ITaskItem[] NuspecData { get { return _data.ToArray(); } }

        #endregion

        static XmlWriterSettings settings {
            get {

                if (_settings == null) {
                    _settings = new XmlWriterSettings();
                    _settings.Encoding = System.Text.Encoding.ASCII;
                    _settings.Indent = true;
                    _settings.IndentChars = new string(' ', 4);
                    _settings.OmitXmlDeclaration = true;
                }
                return _settings;
            }
        }

        #region ITask implementation

        IBuildEngine ITask.BuildEngine {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return _ibe;
            }
            set {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                _ibe = value;
            }
        }

        ITaskHost ITask.HostObject {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return _ith;
            }
            set {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                _ith = value;
            }
        }

        bool ITask.Execute() {
            string outFile;
            FileVersionInfo fvi;
            List<ITaskItem> content;

            verbose = true;
            TestTaskItem.verbose = true;
            MyDictEnum.verbose = true;
            MyDictionary.verbose = true;

            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            ((ITask) this).BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in Execute", null, GetType().FullName, MessageImportance.Low));
            if (!string.IsNullOrEmpty(AssemblyPath)) {
                if (File.Exists(AssemblyPath)) {
                    fvi = FileVersionInfo.GetVersionInfo(AssemblyPath);
                    outFile = Path.GetFullPath(Path.Combine(
                      Path.GetDirectoryName(AssemblyPath),
                      Path.GetFileNameWithoutExtension(AssemblyPath) + ".nuspec"));
                    showMessage("OUTFILE=" + outFile);
                    if (generateSpecfile(outFile, fvi, out content, AssemblyPath)) {
                        // successful
                        // poulate the metadata!.
                        // BuildResult (bool) 
                        // NuspecResult (file-path)
                        // NuspecData (metadata collection of contents
                        try {
#if false
                            _outputpath = new TestTaskItem(outFile);
#else
                            _outputpath = new TaskItem(outFile);
                            //                        if (_outputpath.MetadataNames.con)
                            //_outputpath.RemoveMetadata("FullPath");
#endif
                            _outputpath.SetMetadata("FullPath1", outFile);
                            _outputpath.SetMetadata("FileName1", Path.GetFileName(outFile));
                            _result = true;
                            _data.Clear();
                            _data.AddRange(content);
                        } catch (ArgumentException ae) {
                            showError(ae.Message);

                        } catch (Exception ex) {
                            showError(MiniLogger.decompose(ex));
                        }
                    }
                    Trace.WriteLine("here");
                } else {
                    ((ITask) this).BuildEngine.LogErrorEvent(
                        new BuildErrorEventArgs("subcat", "code", "file", -1, -1, -1, -1, "not exist", "help", "sender", System.DateTime.Now));
                }
            } else {
                ((ITask) this).BuildEngine.LogErrorEvent(
                    new BuildErrorEventArgs("subcat", "code", "file", -1, -1, -1, -1, "empty", "help", "sender", System.DateTime.Now));
            }
            return BuildResult;
        }

        void showError(string v) {
            //BuildEngine be = (ITask) this.BuildEngine;
            int line, col;
            string file;

            line = ((ITask) this).BuildEngine.LineNumberOfTaskNode;
            col = ((ITask) this).BuildEngine.ColumnNumberOfTaskNode;
            file = Path.GetFileName(((ITask) this).BuildEngine.ProjectFileOfTaskNode);

            ((ITask) this).BuildEngine.LogErrorEvent(
                new BuildErrorEventArgs(
                    null, null,
                    file,
                    line, col,
                    line, col,
                    v, "help", "sender", DateTime.Now));
        }

        bool generateSpecfile(string fname, FileVersionInfo fvi, out List<ITaskItem> content, string inPath) {
            bool ret = true;
            content = new List<ITaskItem>();
            /*
  <package >
    <metadata>
      <id>test</id>
      <version>1.0.3</version>
      <authors>Colt Manufacturing LLC.</authors>
      <owners>Colt Manufacturing LLC.</owners>
      <licenseUrl>http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE</licenseUrl>
      <projectUrl>http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE</projectUrl>
      <iconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</iconUrl>
      <requireLicenseAcceptance>false</requireLicenseAcceptance>
      <description>description of tmp.</description>
      <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
      <copyright>Copyright 2018</copyright>
      <tags>Tag1 Tag2</tags>
      <dependencies>
        <dependency id="SampleDependency" version="1.0" />
      </dependencies>
    </metadata>
  </packag                     */

            string versionValue, idValue, authorsValue, descValue, copyrightValue, ownerValue;

            versionValue = fvi.ProductVersion;
            versionValue = new Version(fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart).ToString();
            idValue = fvi.ProductName;
            idValue = Path.GetFileNameWithoutExtension(inPath);
            copyrightValue = fvi.LegalCopyright;
            authorsValue = "blah";
            descValue = fvi.Comments;
            ownerValue = fvi.CompanyName;
            content.AddRange(new ITaskItem[] {
                makeTaskItem("id",idValue),
                makeTaskItem("version",versionValue),
                makeTaskItem("authors",authorsValue),
                makeTaskItem("owners",ownerValue),
                makeTaskItem("description",descValue),//
                makeTaskItem("copyright",copyrightValue),
            });
            ITaskItem iti = makeItem("NuspecFile", inPath);
            try {
                using (XmlWriter xw = XmlWriter.Create(fname, settings)) {
                    xw.WriteStartDocument();

                    xw.WriteStartElement("package");
                    //writeNode(xw,"metadata")
                    xw.WriteStartElement("metadata");
                    xw.WriteElementString("id", idValue);
                    xw.WriteElementString("version", versionValue);
                    xw.WriteElementString("authors", authorsValue);
                    xw.WriteElementString("owners", fvi.CompanyName);
                    xw.WriteElementString("description", descValue);
                    xw.WriteElementString("copyright", copyrightValue);
                    addDependencies(xw);
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                    addFiles(xw, new ITaskItem[] { iti });

                    xw.WriteEndDocument();
                }
            } catch (Exception ex) {
                ret = false;
            }
            return ret;
        }

        ITaskItem makeItem(string v, string inPath) {
            TaskItem ret = new TaskItem(v);

            ret.SetMetadata(v, inPath);
            return ret;
        }

        ITaskItem makeTaskItem(string v, string idValue) {
            TaskItem ret = new TaskItem(v);

            ret.SetMetadata(v, idValue);
            return ret;
        }

        void addFiles(XmlWriter xw, ITaskItem[] items) {
            string key;

            xw.WriteStartElement("files");
            if (items != null && items.Length > 0) {
                foreach (ITaskItem iti in items) {
                    if (!string.IsNullOrEmpty(key = iti.ItemSpec) &&
                        string.Compare(key, "NuspecFile", true) == 0) {
                        xw.WriteStartElement("file");
                        xw.WriteAttributeString("src", iti.GetMetadata(key));
                        xw.WriteAttributeString("target", "lib\\net4.0");
                        xw.WriteEndElement();
                    }
                }
            }
            xw.WriteEndElement();
        }

        void addDependencies(XmlWriter xw) {
            xw.WriteStartElement("dependencies");
            xw.WriteStartElement("dependency");
            xw.WriteAttributeString("id", "some_name");
            xw.WriteAttributeString("version", "1.0.0");
            xw.WriteEndElement();
        }

        #endregion
        void showMessage(string message) {
            ((ITask) this).BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message, "keyword", "sender", MessageImportance.High, DateTime.Now));
        }
    }
}