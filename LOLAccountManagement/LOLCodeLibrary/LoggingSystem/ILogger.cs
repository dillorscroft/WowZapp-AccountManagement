namespace LOLCodeLibrary.LoggingSystem
{
    /// <summary>
    /// base logging interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// log message to the implementing output, looking if it needs to do it on a new line or not
        /// </summary>
        /// <param name="message"></param>
        /// <param name="newLine"></param>
        void LogMessage(string message, bool newLine);
    }
}
