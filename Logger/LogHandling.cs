using System;
using System.Configuration;
using System.IO;

namespace Logger
{
    public class LogHandling
    {
        public void LogHandler(string message)
        {
            string str = ConfigurationManager.AppSettings["LogWritePath"];

            File.AppendAllText(str, "Log Entry...");
            File.AppendAllText(str, "Date and Time" + DateTime.Now.ToString());
            File.AppendAllText(str, message);
        }
    }
}
