using System;
using System.IO;

namespace LOLCodeLibrary.LoggingSystem
{
    /// <summary>
    /// Logger implementation - logs to a text file, creating one file per day and appending to it all the time.
    /// The log files are in the System Temp folder
    /// </summary>
    public class DebugTextLogger : ILogger
    {
        public void LogMessage(string message, bool newLine)
        {
            try
            {
                string path = Path.GetTempPath();
                string logName = "log_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".log";
                string filePath = Path.Combine(path, logName);

                StreamWriter file = new StreamWriter(filePath, true);
                if (newLine)
                    file.WriteLine(DateTime.Now.ToString() + " : " + message);
                else
                    file.Write(DateTime.Now.ToString() + " : " + message);

                file.Close();
            }
            catch
            {
            }
        }
    }
}