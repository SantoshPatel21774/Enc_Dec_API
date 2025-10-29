using Microsoft.Extensions.Logging;

namespace Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly LogFileOptions _options;

        public CustomLoggerProvider(LogFileOptions options)
        {
            _options = options;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(categoryName, _options);
        }

        public void Dispose() { }
    }

}
