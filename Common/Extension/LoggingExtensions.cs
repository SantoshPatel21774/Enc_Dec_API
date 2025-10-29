using Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Extension
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder AddCustomFileLogger(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var config = app.ApplicationServices.GetService<IConfiguration>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            if (config == null || loggerFactory == null) return app;

            try
            {
                // bind options from configuration (section "LogFile")
                LogFileOptions options = new();
                configuration.GetSection("LogFile").Bind(options);

                loggerFactory.AddProvider(new CustomLoggerProvider(new LogFileOptions
                {
                    LogFilePath = string.IsNullOrWhiteSpace(options.LogFilePath) ? "Logs" : options.LogFilePath,
                    LogFileName = string.IsNullOrWhiteSpace(options.LogFileName) ? "app.log" : options.LogFileName,
                    LogFileDateFormat = string.IsNullOrWhiteSpace(options.LogFileDateFormat) ? "yyyyMMdd" : options.LogFileDateFormat,
                    MinimumLogLevel = LogLevel.Information
                }));

            }
            catch (Exception ex)
            {
                // swallow or log via Console to avoid crashing
                Console.WriteLine($"Failed to initialize CustomLoggerProvider: {ex.Message}");
            }

            return app;

        }
    }
}
