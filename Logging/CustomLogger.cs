using Microsoft.Extensions.Logging;

namespace Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string _name;
        private readonly LogFileOptions _options;

        public CustomLogger(string name, LogFileOptions options)
        {
            _name = name;
            _options= options;

            if (!string.IsNullOrWhiteSpace(_options.LogFilePath))
            {
                // Ensure the folder exists
                if (!Directory.Exists(_options.LogFilePath))
                {
                    Directory.CreateDirectory(_options.LogFilePath);
                }
            }
        }
        IDisposable? ILogger.BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            var fileNameWithDateTime = _options.LogFileName + "_" + DateTime.Now.ToString(_options.LogFileDateFormat) + ".txt";
            var filePath = string.Empty;
            if (!string.IsNullOrWhiteSpace(_options.LogFilePath))
                filePath = Path.Combine(_options.LogFilePath, fileNameWithDateTime);

            File.AppendAllText(filePath, $"{DateTime.Now} [{logLevel}] {_name}: {message}{Environment.NewLine}");
        }
    }

}
