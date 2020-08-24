using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestComm.Helper
{
    public class LogHelper
    {
        private static Logger Loger { get; set; }

        public LogHelper()
        {
            Loger = LogManager.GetCurrentClassLogger();
        }

        public static void WriteError(string message)
        {
            Loger.Log(LogLevel.Error, message);
        }

        public static void WiteInfo(string message)
        {
            Loger.Log(LogLevel.Info, message);
        }
    }
}
