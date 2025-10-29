using Microsoft.Extensions.Logging;

namespace Logging
{
    public class LogFileOptions
    {
        public string LogFilePath { get; set; } = "Logs";
        public string LogFileName { get; set; } = "app.log";
        public string LogFileDateFormat { get; set; } = "yyyyMMdd";
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;

    }
}
