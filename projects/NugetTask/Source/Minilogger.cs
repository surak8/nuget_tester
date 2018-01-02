using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace NSNugetTask {
    public static class MiniLogger {
        #region logging methods

        public static void log(MethodBase mb) {
            //log("1 " + makeSig(mb, null));
            log(makeSig(mb, null));
        }
        public static void log(MethodBase mb, object[] parmValues) {
            //log("2 " + makeSig(mb, parmValues));
            log(makeSig(mb, parmValues));
        }
        public static void log(MethodBase mb, string msg, object[] parmValues) {
            log(makeSig(mb, parmValues) + ":" + msg);
        }
        public static void log(MethodBase mb, string msg) {
            string sig = makeSig(mb);

            log(sig + (string.IsNullOrEmpty(msg) ? string.Empty : (":" + msg)));
        }

        public static void log(string msg) {
            //if (Console.Error.a)
            Console.Error.WriteLine(msg);
            Console.Error.Flush();
#if TRACE
            if (!Trace.AutoFlush)
                Trace.AutoFlush = true;
            Trace.WriteLine(msg);
            //Trace.Flush();
#endif
        }

        public static string makeSig(MethodBase mb) {
            string mname = mb.Name;

            return mb.ReflectedType.Name +
            ((string.Compare(mname, ConstructorInfo.ConstructorName, true) == 0 ||
            string.Compare(mname, ConstructorInfo.TypeConstructorName, true) == 0) ?
            string.Empty : ("." + mname))
                + genArgs(mb);
        }
        public static string makeSig(MethodBase mb, object[] parmValues) {
            string mname = mb.Name;

            return mb.ReflectedType.Name +
            ((string.Compare(mname, ConstructorInfo.ConstructorName, true) == 0 ||
            string.Compare(mname, ConstructorInfo.TypeConstructorName, true) == 0) ?
            string.Empty : ("." + mname))
                + genArgs(mb, parmValues);
        }

        static string genArgs(MethodBase mb) {
            var parms = mb.GetParameters();

            StringBuilder sb = new StringBuilder();
            int i = 0;

            sb.Append("(");
            foreach (var avar in parms) {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(avar.ParameterType.FullName + " " + avar.Name);
                i++;
            }
            sb.Append(")");
            return sb.ToString();
        }
        static string genArgs(MethodBase mb, object[] parmValues) {
            var parms = mb.GetParameters();

            StringBuilder sb = new StringBuilder();
            int i = 0, plen = parmValues == null ? 0 : parmValues.Length;

            sb.Append("(");
            foreach (var avar in parms) {
                if (i > 0)
                    sb.Append(", ");
                if (parmValues != null && i < plen)
                    sb.Append(avar.Name + " = " + parmValues[i].ToString());
                else
                    sb.Append(avar.ParameterType.FullName + " " + avar.Name);
                i++;
            }
            sb.Append(")");
            return sb.ToString();
        }
        #endregion logging methods
    }
}