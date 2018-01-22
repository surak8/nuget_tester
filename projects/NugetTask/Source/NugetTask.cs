
//  https://msdn.microsoft.com/en-us/library/t9883dzc.aspx
// http://blog.differentpla.net/blog/2013/02/01/msbuild-tasks-input-parameters-and-itemgroups
#define USE_TEST_TASK_ITEM

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
        static XmlWriterSettings _settings;

        readonly List<ITaskItem> _data = new List<ITaskItem>();
        readonly List<ITaskItem> _depends = new List<ITaskItem>();
        readonly List<ITaskItem> _files = new List<ITaskItem>();
        ITaskHost _ith;
        IBuildEngine _ibe;
        ITaskItem _outputpath;
        ITaskItem _pkg;
        bool _result;
        string _asmPath;
        string _specPath;
        #endregion

        #region ctor
        public GenerateNugetSpec() {
            _result = false;
        }
        #endregion

        #region properties
        #region required input properties
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
        #endregion

        #region output properties
        [Output]
        public bool BuildResult { get { return _result; } }

        [Output]
        public ITaskItem NuspecResult { get { return _outputpath; } }

        [Output]
        public ITaskItem[] NuspecData { get { return _data.ToArray(); } }

        [Output]
        public ITaskItem NuspecPackage { get { return _pkg; } }
        #endregion

        public ITaskItem[] PackageDepends {
            set {
                _depends.Clear();
                if (value != null)
                    _depends.AddRange(value);
            }
        }

        public ITaskItem[] PackageFiles {
            set {
                _files.Clear();
                if (value != null)
                    _files.AddRange(value);
            }
        }

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
        #endregion


        #region ITask implementation

        #region ITask properties
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

        #endregion ITask properties

        #region ITask methods
        bool ITask.Execute() {
            string outFile, outSpecPath, dir = null;
            FileVersionInfo fvi;
            List<ITaskItem> content;

            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            if (string.IsNullOrEmpty(NuspecPath)) {
                showError("Property 'NuspecPath' is null!");
                return false;
            }
            outSpecPath = Path.GetDirectoryName(Path.GetFullPath(NuspecPath));
            showMessage("NuspecPath=" + outSpecPath);
            try {
                dir = Path.GetDirectoryName(outSpecPath);
                showMessage("Check NuspecPath=" + dir);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                    showMessage("Creating NuspecPath-dir =" + dir);
                }
            } catch (Exception ex) {
                showError("error creating directory '" + dir + "'" + Environment.NewLine + MiniLogger.decompose(ex));
                return false;
            }
            showMessage(MessageImportance.Low, "spec-path: " + outSpecPath);

            if (!string.IsNullOrEmpty(AssemblyPath)) {
                if (File.Exists(AssemblyPath)) {
                    fvi = FileVersionInfo.GetVersionInfo(AssemblyPath);
#if true
                    outFile = Path.GetFullPath(
                        Path.Combine(
                            outSpecPath,
                            Path.GetFileNameWithoutExtension(AssemblyPath) + ".nuspec"));
#else
                    outFile = Path.GetFullPath(Path.Combine(
                      Path.GetDirectoryName(AssemblyPath),
                      Path.GetFileNameWithoutExtension(AssemblyPath) + ".nuspec"));
#endif
                    showMessage("OUTFILE=" + outFile);
                    if (generateSpecfile(outFile, fvi, AssemblyPath, out _pkg, out content)) {
                        try {
#if USE_TEST_TASK_ITEM
                            _outputpath = new TestTaskItem(outFile);
#else
                            _outputpath = new TaskItem(outFile);
#endif
                            _result = true;
                            populateData("NuspecData", content);

                        } catch (ArgumentException ae) {
                            showError(ae.Message);

                        } catch (Exception ex) {
                            showError(MiniLogger.decompose(ex));
                        }
                    }
                    Trace.WriteLine("here");
                } else {
                    this.showError(AssemblyPath + " does not exist in " + Directory.GetCurrentDirectory());
                }
            } else {
                ((ITask) this).BuildEngine.LogErrorEvent(
                    new BuildErrorEventArgs("subcat", "code", "file", -1, -1, -1, -1, "empty", "help", "sender", System.DateTime.Now));
            }
            return BuildResult;
        }

        #endregion ITask methods
        #endregion

        #region methods
        void populateData(string elementName, List<ITaskItem> content) {
            ITaskItem ti;
            string key;

#if USE_TEST_TASK_ITEM
            ti = new TestTaskItem(elementName);
#else
            ti = new TaskItem(elementName);
#endif
            foreach (ITaskItem iti in content) {
                key = iti.ItemSpec;
                ti.SetMetadata(key, iti.GetMetadata(key));
            }
            _data.Clear();
            _data.Add(ti);
        }

        void showError(string v) {
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

        bool generateSpecfile(string fname, FileVersionInfo fvi, string inPath, out ITaskItem pkg1, out List<ITaskItem> content) {
            bool ret = true;
            string versionValue, idValue, authorsValue, descValue, copyrightValue, ownerValue,pkgValue;

            pkg1 = null;
            content = new List<ITaskItem>();
            versionValue = fvi.ProductVersion;
            versionValue = new Version(fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart).ToString();
            idValue = fvi.ProductName;
            idValue = Path.GetFileNameWithoutExtension(inPath);
            copyrightValue = fvi.LegalCopyright;
            authorsValue = "blah";
            descValue = fvi.Comments;
            ownerValue = fvi.CompanyName;
            pkgValue = Path.Combine(inPath, idValue + versionValue + ".nupkg");
            pkgValue = Path.Combine(
                Path.GetDirectoryName (fname), 
                idValue + "." + versionValue + ".nupkg");

#if USE_TEST_TASK_ITEM
           pkg1 = new TestTaskItem(pkgValue);
#else
            pkg1 = new TaskItem(pkgValue);
#endif
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
                    xw.WriteStartElement("metadata");
                    xw.WriteElementString("id", idValue);
                    xw.WriteElementString("version", versionValue);
                    xw.WriteElementString("authors", authorsValue);
                    xw.WriteElementString("owners", fvi.CompanyName);
                    xw.WriteElementString("description", descValue);
                    xw.WriteElementString("copyright", copyrightValue);
                    addDependencies(xw, this._depends.ToArray());
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

        void addDependencies(XmlWriter xw, ITaskItem[] deps) {
            xw.WriteStartElement("dependencies");
            if (deps != null)
                foreach (ITaskItem idep in deps)
                    addDependency(xw, idep);
            xw.WriteEndElement();
        }

        void addDependency(XmlWriter xw, ITaskItem idep) {
            xw.WriteStartElement("dependency");
            xw.WriteAttributeString("id", idep.ItemSpec);
            xw.WriteAttributeString("version", idep.GetMetadata("Version"));
            xw.WriteEndElement();
        }

        void showMessage(string message) {
            showMessage(MessageImportance.High, message);
        }

        void showMessage(MessageImportance mi, string message) {
            ((ITask) this).BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message, "keyword", "sender", mi, DateTime.Now));
        }
#endregion
    }
}