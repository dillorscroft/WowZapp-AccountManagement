using System;

namespace LOLCodeLibrary.LoggingSystem
{
    /// <summary>
    /// Logger implementation - outputs to Console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string message, bool newLine)
        {
            if (newLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }
    }
}