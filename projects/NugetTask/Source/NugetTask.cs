//  https://msdn.microsoft.com/en-us/library/t9883dzc.aspx
// http://blog.differentpla.net/blog/2013/02/01/msbuild-tasks-input-parameters-and-itemgroups
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NSCommon.Logging;

namespace NSNugetTask {

    public class GenerateNugetSpec : ITask {
        #region fields
        public static bool verbose = false;
        ITaskHost _ith;
        IBuildEngine _ibe;
        #endregion
        TestTaskItem _singleResult;
#if true
        ITaskItem _multiResult;
#else
        TestTaskItem _multiResult;
#endif


        #region ITask implementation

        public IBuildEngine BuildEngine {
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

        public ITaskHost HostObject {
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

        public bool Execute() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            //TestTaskItem.verbose = true;
            //MyDictionary.verbose = true;
            //MyDictEnum.verbose = true;
            _singleResult = new TestTaskItem("zzzz");
#if true
            _multiResult = new TaskItem("spec");
            _multiResult.SetMetadata("m1", "v1");
            _multiResult.SetMetadata("m2", "v2");
#else
            _multiResult = new TestTaskItem("multi");
            ((ITaskItem) _multiResult).SetMetadata("m1", "v1");
            ((ITaskItem) _multiResult).SetMetadata("m2", "v2");
#endif
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in Execute", null, GetType().FullName, MessageImportance.Low));
            return true;
        }
        #endregion

        string _mp;

        [Required]
        public string MyProperty {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return _mp;
            }
            set {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                _mp = value;
            }
        }

        [Output]
        public ITaskItem singleResult {

            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in results", null, GetType().FullName, MessageImportance.Low));
                return _singleResult;
            }
        }

        [Output]
        public ITaskItem[] results {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in results", null, GetType().FullName, MessageImportance.Low));
                List<TestTaskItem> ret = new List<TestTaskItem>();

                foreach (var avar in ((ITaskItem) _multiResult).MetadataNames) {
                    ret.Add(
                        new TestTaskItem(
                            avar.ToString(),
                            ((ITaskItem) _multiResult).GetMetadata(avar.ToString())));
                }
                return ret.ToArray();
            }
        }
    }
}