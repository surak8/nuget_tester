//  https://msdn.microsoft.com/en-us/library/t9883dzc.aspx
// http://blog.differentpla.net/blog/2013/02/01/msbuild-tasks-input-parameters-and-itemgroups
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Build.Framework;

namespace NSNugetTask {

    public class GenerateNugetSpec : ITask {
        #region fields
        ITaskHost _ith;
        IBuildEngine _ibe;
        #endregion
        TestTaskItem _singleResult;
        TestTaskItem _multiResult;


        #region ITask implementation

        public IBuildEngine BuildEngine { get { return _ibe; } set { _ibe = value; } }

        public ITaskHost HostObject { get { return _ith; } set { _ith = value; } }

        public bool Execute() {
            TestTaskItem.verbose = true;
            MyDictionary.verbose = true;
            MyDictEnum.verbose = true;
            _singleResult = new TestTaskItem("zzzz");
            _multiResult = new TestTaskItem("multi");
            _multiResult.SetMetadata("m1", "v1");
            _multiResult.SetMetadata("m2", "v2");
            BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in Execute", null, GetType().FullName, MessageImportance.Low));
            return false;
        }
        #endregion

        string _mp;

        [Required]
        public string MyProperty {
            get { return _mp; }
            set { _mp = value; }
        }

        [Output]
        public ITaskItem singleResult {

            get {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in results", null, GetType().FullName, MessageImportance.Low));
                return _singleResult;
            }
        }

        [Output]
        public ITaskItem[] results {
            get {
                BuildEngine.LogMessageEvent(new BuildMessageEventArgs("in results", null, GetType().FullName, MessageImportance.Low));
                List<TestTaskItem> ret = new List<TestTaskItem>();

                //foreach(var )

                foreach(var avar in _multiResult.MetadataNames) {
                    ret.Add(new TestTaskItem(avar.ToString(), _multiResult.GetMetadata(avar.ToString())));
                }
                return ret.ToArray();
                //return _multiResult;
            }
        }
    }
}