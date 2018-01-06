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
        static int _ictr = 0;
        readonly int _ninstance = ++_ictr;

        public int instanceCounter { get { return _ictr; } }

        #region ctors
        internal TestTaskItem() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            _map = new Dictionary<string, string>();
        }

        public TestTaskItem(string msg) : this() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), msg, new object[] { msg });
            _itemSpec = msg;
        }

        internal TestTaskItem(string key, string avalue) : this() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}", new object[] { key, avalue });
            ((ITaskItem) this).ItemSpec = key;
            ((ITaskItem) this).SetMetadata(key, avalue);
        }

        #endregion


        //public string ItemSpec {
        //    get {
        //        string ret = string.IsNullOrEmpty(_itemSpec) ? "NULL_ITEMSPEC" : _itemSpec;
        //        if (verbose)
        //            MiniLogger.log(MethodBase.GetCurrentMethod(), "return: " + ret + (string.IsNullOrEmpty(_itemSpec) ? ("THIS=" + this.ToString()) : string.Empty));
        //        //MiniLogger.log(MethodBase.GetCurrentMethod());
        //        return ret;
        //    }
        //    set {
        //        if (verbose)
        //            MiniLogger.log(MethodBase.GetCurrentMethod(), value);
        //        _itemSpec = value;
        //    }
        //}

        //public int MetadataCount {
        //    get {
        //        if (verbose)
        //            MiniLogger.log(MethodBase.GetCurrentMethod(), "returning " + _map.Count);
        //        return _map.Count;
        //        //throw new NotImplementedException();
        //    }
        //}

        //public ICollection MetadataNames {
        //    get {
        //        List<string> ret = new List<string>();
        //        foreach (var avar in _map.Keys)
        //            ret.Add(avar);
        //        if (verbose)
        //            MiniLogger.log(
        //                MethodBase.GetCurrentMethod(),
        //                string.Join(";", ret.ToArray()));
        //        return ret;
        //    }
        //}

        //public IDictionary CloneCustomMetadata() {
        //    if (verbose)
        //        MiniLogger.log(MethodBase.GetCurrentMethod());
        //    return new MyDictionary(_map);
        //}

        //public void CopyMetadataTo(ITaskItem destinationItem) {
        //    if (verbose)
        //        MiniLogger.log(MethodBase.GetCurrentMethod());
        //    throw new NotImplementedException();
        //}

        //public string GetMetadata(string metadataName) {
        //    string ret;

        //    ret = _map[metadataName];
        //    if (verbose)
        //        MiniLogger.log(MethodBase.GetCurrentMethod(),
        //            "[" + metadataName + "] : " + ret);
        //    return _map[metadataName];
        //}

        //public void RemoveMetadata(string metadataName) {
        //    bool bfound = _map.ContainsKey(metadataName);

        //    if (verbose)
        //        MiniLogger.log(MethodBase.GetCurrentMethod(), "[" + metadataName + "] is " + bfound);
        //    if (_map.ContainsKey(metadataName))
        //        _map.Remove(metadataName);
        //    //throw new NotImplementedException();
        //}

        //public void SetMetadata(string metadataName, string metadataValue) {
        //    if (verbose)
        //        MiniLogger.log(MethodBase.GetCurrentMethod(),
        //            "{0}={1}",
        //            new object[] { metadataName, metadataValue });
        //            //"[," + metadataName + "] = " + metadataValue);
        //    if (_map.ContainsKey(metadataName))
        //        _map[metadataName] = metadataValue;
        //    else
        //        _map.Add(metadataName, metadataValue);
        //}



        #region ITaskItem implementation

        #region ITaskItem properties
        string ITaskItem.ItemSpec {
            get {
                if (string.IsNullOrEmpty (_itemSpec))
                    MiniLogger.log(MethodBase.GetCurrentMethod(),"UGH, it's NULL!");
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod(),"returning: "+_itemSpec);
                return _itemSpec;
            }
            set {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                _itemSpec = value;
            }
        }

        ICollection ITaskItem.MetadataNames {
            get {
                List<string> ret = new List<string>();

                foreach (var avar in _map.Keys)
                    ret.Add(avar);
                if (verbose)
                    MiniLogger.log(
                        MethodBase.GetCurrentMethod(),
                        "returning: " + string.Join(";", ret.ToArray()));
                return ret;
            }
        }

        int ITaskItem.MetadataCount {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return _map.Count;
            }
        }

        #endregion ITaskItem properties

        #region ITaskItem methods
        string ITaskItem.GetMetadata(string metadataName) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}", new object[] { metadataName });
            string ret;

            ret = _map[metadataName];
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(),
                    "[" + metadataName + "] : " + ret);
            return _map[metadataName];
        }

        void ITaskItem.SetMetadata(string metadataName, string metadataValue) {
            //if (verbose)
            //    MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}", new object[] { metadataName, metadataValue });
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(),
                    "{0}={1}",
                    new object[] { metadataName, metadataValue });
            //"[," + metadataName + "] = " + metadataValue);
            if (_map.ContainsKey(metadataName))
                _map[metadataName] = metadataValue;
            else
                _map.Add(metadataName, metadataValue);
        }

        void ITaskItem.RemoveMetadata(string metadataName) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}", new object[] { metadataName });
            throw new NotImplementedException();
        }

        void ITaskItem.CopyMetadataTo(ITaskItem destinationItem) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}", new object[] { destinationItem });
            throw new NotImplementedException();
        }

        IDictionary ITaskItem.CloneCustomMetadata() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}");
            //if (verbose)
            //    MiniLogger.log(MethodBase.GetCurrentMethod());
            return new MyDictionary(_map);
        }
        #endregion ITaskItem methods

        #endregion ITaskItem implementation
    }
}