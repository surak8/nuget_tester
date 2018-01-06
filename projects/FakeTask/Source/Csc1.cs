using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Build.Framework;

namespace NSFake {
    public class Csc1 : ITask {
        #region fields
        IBuildEngine _ibe;
        ITaskHost _ith; 
        List<ITaskItem> _srcs;
        List<ITaskItem> _res;
        List<ITaskItem> _refs;
        string _targetType;
        bool _emitDebug;
        string _debugType;
        string _outAsm;
        bool _buildSuccess;
        #endregion

        #region ctor
        public Csc1() {
            MiniLogger.log(MethodBase.GetCurrentMethod());
            _srcs = new List<ITaskItem>();
            _res = new List<ITaskItem>();
            _refs = new List<ITaskItem>();
        }
        #endregion

        #region ITask implementation
        IBuildEngine ITask.BuildEngine { get { return _ibe; } set { _ibe = value; } }

        ITaskHost ITask.HostObject { get { return _ith; } set { _ith = value; } }

        bool ITask.Execute() {
            ((ITask) this).BuildEngine.LogMessageEvent(new BuildMessageEventArgs("message", "keyword", "sender", MessageImportance.Low, DateTime.Now));
            _buildSuccess = true;
            if (this.BuildSucceeded) {
                MiniLogger.log(MethodBase.GetCurrentMethod());
            }
            return true;
        }
        #endregion ITask implementation


        public string TargetType { get { return _targetType; } set { _targetType = value; } }

        public bool EmitDebugInformation { get { return _emitDebug; } set { _emitDebug = value; } }

        public string DebugType { get { return _debugType; } set { _debugType = value; } }

        public ITaskItem[] Sources {
            get { return _srcs.ToArray(); }
            set {
                _srcs.Clear();
                if (value != null)
                    _srcs.AddRange(value);
            }
        }

        public ITaskItem[] Resources {
            get { return _res.ToArray(); }
            set { _res.Clear(); if (value != null) _res.AddRange(value); }
        }

        public ITaskItem[] References {
            get { return _refs.ToArray(); }
            set { _refs.Clear(); if (value != null) _refs.AddRange(value); }
        }

        [Output]
        public string OutputAssembly { get { return _outAsm; } set { _outAsm = value; } }

        [Output]
        public bool BuildSucceeded { get { return _buildSuccess; }   }

    }
}