using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Diagnostics;

namespace Common
{
    public enum LogLevel { FATAL = 0, ERROR, WARN, INFO, DEBUG,};
    class utility
    {
        static log4net.ILog errorlog = log4net.LogManager.GetLogger("ErrorAppender");
        static log4net.ILog debuglog = log4net.LogManager.GetLogger("DebugAppender");

        public static string GetProcessInfo()
        {
            Process CurrentProcess = Process.GetCurrentProcess();
            string sPID = CurrentProcess.Id.ToString();//PID
            string sCPU = ((Double)(CurrentProcess.TotalProcessorTime.TotalMilliseconds - CurrentProcess.UserProcessorTime.TotalMilliseconds)).ToString();//CPU
            string sMem = (CurrentProcess.WorkingSet64 / 1024 / 1024).ToString() + "M (" + (CurrentProcess.WorkingSet64 / 1024).ToString() + "KB)";//占用内存
            string sThread = CurrentProcess.Threads.Count.ToString();//线程       

            return string.Format("当前进程信息,PID={0},CPU={1},MEM={2},Thread={3}", sPID, sCPU, sMem, sThread); 
        }
        public static void WriteDebugLog(string sLog, LogLevel nLevel= LogLevel.DEBUG)
        {
            //默认这个是在debug状态下，一般情况下，可以在代码中打日志的时候，调整这个状态
            switch (nLevel)
            {
                case LogLevel.FATAL:
                    debuglog.Fatal(sLog);
                    break;

                case LogLevel.ERROR:
                    debuglog.Error(sLog);
                    break;

                case LogLevel.WARN:
                    debuglog.Warn(sLog);
                    break;

                case LogLevel.INFO:
                    debuglog.Info(sLog);
                    break;

                default:
                    debuglog.Debug(sLog);
                    break;
            }
        }
        public static Int64 GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        public static string GetNowTime()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff");
        }

        public static void WriteErrorLog(string sLog, LogLevel nLevel = LogLevel.ERROR)
        {
            switch(nLevel)
            {
                case LogLevel.FATAL:
                    errorlog.Fatal(sLog);
                    break;

                case LogLevel.ERROR:
                    errorlog.Error(sLog);
                    break;

                case LogLevel.WARN:
                    errorlog.Warn(sLog);
                    break;

                case LogLevel.INFO:
                    errorlog.Info(sLog);
                    break;

                default:
                    break;
            }
        }
    }
}
