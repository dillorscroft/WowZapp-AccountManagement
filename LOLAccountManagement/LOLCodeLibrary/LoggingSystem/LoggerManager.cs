namespace LOLCodeLibrary.LoggingSystem
{
    /// <summary>
    /// manages the logging system, using the ILogger interface
    /// </summary>
    public class LoggerManager
    {
        private ILogger _logger;

        public LoggerManager(ILogger logger)
        {
            this._logger = logger;
        }

        public ILogger GetManager()
        {
            return this._logger;
        }
    }
}