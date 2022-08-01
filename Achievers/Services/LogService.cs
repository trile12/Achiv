using log4net;
using Newtonsoft.Json;
using System;

namespace Achievers.Services
{
    public class LogService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(string name, string message)
        {
            log.DebugFormat("{0} - {1}", name, message);
        }

        public static void Debug(string name, object message)
        {
            log.DebugFormat("{0} - {1}", name, JsonConvert.SerializeObject(message));
        }

        public static void Error(string name, Exception ex)
        {
            log.Error(name, ex);
        }
    }
}