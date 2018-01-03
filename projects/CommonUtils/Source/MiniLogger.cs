using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace NSCommon.Logging {
    public static class MiniLogger {
        public static string decompose(Exception ex) {
            StringBuilder sb = new StringBuilder();
            Exception ex2 = ex;

            while (ex2 != null) {
                sb.AppendLine("[" + ex2.GetType().FullName + "]");
                sb.AppendLine(ex2.Message + Environment.NewLine);
                ex2 = ex2.InnerException;
            }
            return sb.ToString();
        }

        public static void log(MethodBase mb, Exception ex) {
            log(makeSig(mb) + ":" + decompose(ex));
        }

        public static void log(MethodBase mb, string msg) {
            log(makeSig(mb) + ":" + msg);
        }

        public static void log(MethodBase mb,string fmt,params object[] args) {
            log(makeSig(mb)+":"+string.Format(fmt,args));
        }

        public static void log(MethodBase mb) {
            log(makeSig(mb));
        }

        public static void log(string msg) {
            Console.Error.WriteLine(msg);
#if TRACE
            Trace.WriteLine(msg);
#endif
        }

        public static string makeSig(MethodBase mb) {
            return mb.ReflectedType.Name + "." + mb.Name;
        }
    }
}