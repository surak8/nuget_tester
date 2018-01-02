using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Build.Framework;
using NSCommon.Logging;

namespace NSNugetTask {
    public class TestTaskItem : ITaskItem {

        #region fields
        public static bool verbose = false;
        IDictionary<string, string> _map;
        string _itemSpec;
        #endregion

        #region ctors
        internal TestTaskItem() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            _map = new Dictionary<string, string>();
        }

        public TestTaskItem(string msg) : this() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), msg,new object[] { msg });
            _itemSpec = msg;
        }

        internal TestTaskItem(string key, string avalue) : this() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), new object[] { key,avalue });
            this.SetMetadata(key, avalue);
        }
        #endregion


        public string ItemSpec {
            get {
                //MiniLogger.log(MethodBase.GetCurrentMethod());
                return _itemSpec;
            }
            set {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod(), value);
                _itemSpec = value;
            }
        }

        public int MetadataCount {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod(), "returning " + _map.Count);
                return _map.Count;
                //throw new NotImplementedException();
            }
        }

        public ICollection MetadataNames {
            get {
                List<string> ret = new List<string>();
                foreach (var avar in _map.Keys)
                    ret.Add(avar);
                if (verbose)
                    MiniLogger.log(
                        MethodBase.GetCurrentMethod(),
                        string.Join(";", ret.ToArray()));
                return ret;
            }
        }

        public IDictionary CloneCustomMetadata() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            return new MyDictionary(_map);
        }

        public void CopyMetadataTo(ITaskItem destinationItem) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            throw new NotImplementedException();
        }

        public string GetMetadata(string metadataName) {
            string ret;

            ret = _map[metadataName];
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(),
                    "[" + metadataName + "] : " + ret);
            return _map[metadataName];
        }

        public void RemoveMetadata(string metadataName) {
            bool bfound = _map.ContainsKey(metadataName);

            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "[" + metadataName + "] is " + bfound);
            if (_map.ContainsKey(metadataName))
                _map.Remove(metadataName);
            //throw new NotImplementedException();
        }

        public void SetMetadata(string metadataName, string metadataValue) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(),
                    new object[] { metadataName, metadataValue });
                    //"[," + metadataName + "] = " + metadataValue);
            if (_map.ContainsKey(metadataName))
                _map[metadataName] = metadataValue;
            else
                _map.Add(metadataName, metadataValue);
        }
    }
}