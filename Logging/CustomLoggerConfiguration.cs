using Microsoft.Extensions.Logging;

namespace Logging
{
    public class CustomLoggerConfiguration
    {
        public LogLevel MinLevel { get; set; } = LogLevel.Information;
        public string LogFilePath { get; set; } = "Logs/log.txt";
        // Add other configuration properties as needed (e.g., file path, connection string)
    }
}
