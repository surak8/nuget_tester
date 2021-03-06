using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NSCommon.Logging;

namespace NSNugetTask {
    public class MyDictionary : IDictionary {
        #region fields
        public static bool verbose = false;
        IDictionary<string, string> _map = new Dictionary<string, string>();
        readonly object _lock = new object();
        #endregion

        #region ctor
        public MyDictionary(IDictionary<string, string> amap) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());

            if (amap != null) {
                foreach (string aKey in amap.Keys)
                    _map.Add(aKey, amap[aKey]);
            }
        }
        #endregion

        object IDictionary.this[object key] {
            get {
                string akey = key.ToString(), ret;

                if (!_map.ContainsKey(akey)) {
                    ret = null;
                } else {
                    ret = _map[akey];
                }
                if (verbose)
                    MiniLogger.log(
                    MethodBase.GetCurrentMethod(),
                    ": key=" + akey + " returning: " + (string.IsNullOrEmpty(ret) ? "NULL" : ret));
                return ret;
            }

            set {
                string akey = key.ToString();
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}", new object[] { akey, value.ToString() });
                if (_map.ContainsKey(akey))
                    _map[akey] = value.ToString();
                else
                    _map.Add(akey, value.ToString());
            }
        }

        int ICollection.Count {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod(), "returning " + _map.Count);
                return _map.Count;
            }
        }

        bool IDictionary.IsFixedSize {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return true;
            }
        }

        bool IDictionary.IsReadOnly {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return true;
            }
        }

        bool ICollection.IsSynchronized {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return true;
            }
        }

        ICollection IDictionary.Keys {
            get {
                int n = _map.Count, i;
                string[] ret = new string[n];

                if (n > 0) {
                    i = 0;
                    foreach (string aKey in _map.Keys)
                        ret[i++] = aKey;
                }
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod(), "returning " + ret);
                return ret;
            }
        }

        object ICollection.SyncRoot {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                return _lock;
            }
        }

        ICollection IDictionary.Values {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                int n = _map.Count, i;
                string[] ret = new string[n];

                if (n > 0) {
                    i = 0;
                    foreach (string aKey in _map.Keys)
                        ret[i++] = _map[aKey];
                }
                return ret;
            }
        }

        void IDictionary.Add(object key, object value) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "{0}={1}", new object[] { key, value });
            _map.Add(key.ToString(), value.ToString());
        }

        void IDictionary.Clear() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            _map.Clear();
        }

        bool IDictionary.Contains(object key) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "Key={0}", new object[] { key });
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "[{0}]={1}", new object[] { array, index });
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            MiniLogger.log(MethodBase.GetCurrentMethod());
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            return new MyDictEnum(_map);
        }

        void IDictionary.Remove(object key) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "Key={0}", new object[] { key });
            throw new NotImplementedException();
        }
    }

    public class MyDictEnum : IDictionaryEnumerator {
        public static bool verbose = false;
        IDictionary<string, string> _map;
        List<string> _keys;
        int _nitems;
        int _nindex;


        internal MyDictEnum(IDictionary<string, string> aMap) {
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod());
            _map = aMap;
            _keys = new List<string>();
            if (_map != null)
                foreach (string akey in _map.Keys)
                    _keys.Add(akey);
            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(), "have " + _keys.Count + " keys.");

            ((IDictionaryEnumerator) this).Reset();
        }

        object IEnumerator.Current {
            get {
                string aKey;

                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                aKey = _keys[_nindex];
                return new DictionaryEntry(aKey, _map[aKey]);
            }
        }

        DictionaryEntry IDictionaryEnumerator.Entry {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                throw new NotImplementedException();
            }
        }

        object IDictionaryEnumerator.Key {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                throw new NotImplementedException();
            }
        }

        object IDictionaryEnumerator.Value {
            get {
                if (verbose)
                    MiniLogger.log(MethodBase.GetCurrentMethod());
                throw new NotImplementedException();
            }
        }

        bool IEnumerator.MoveNext() {
            bool ret;

            ret = _nitems < 0 ? false : (++_nindex < _nitems);

            if (verbose)
                MiniLogger.log(MethodBase.GetCurrentMethod(),
                    "returning: " + ret +
                    " [ITEMS=" + _nindex + ", INDEX=" + _nindex + "]");
            return ret;
        }

        void IEnumerator.Reset() {
            _nitems = _map.Count;
            _nindex = -1;

            if (verbose)
                MiniLogger.log(
                    MethodBase.GetCurrentMethod(),
                    "ITEMS=" + _nitems + ", INDEX=" + _nindex);
        }
    }
}