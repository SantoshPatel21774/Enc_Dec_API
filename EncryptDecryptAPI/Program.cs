using Encrpt_Decrpt_API;
using DotNetEnv; // Add this using directive at the top of the file

var builder = WebApplication.CreateBuilder(args);

Env.Load();

//builder.Services.Configure<CustomFileLoggerOptions>(
//   builder.Configuration.GetSection("Logging:CustomFileLogger"));

//CustomFileLoggerOptions customFileLoggerOptions = new()
//{
//    LogFilePath = builder.Configuration["Logging:CustomFileLogger:LogFilePath"] ?? string.Empty,
//    LogFileName = builder.Configuration["Logging:CustomFileLogger:LogFileName"] ?? string.Empty,
//    LogFileDateFormat = builder.Configuration["Logging:CustomFileLogger:LogFileFormat"] ?? string.Empty
//};
//builder.Logging.AddProvider(new CustomFileLoggerProvider(customFileLoggerOptions));
builder.Configuration.AddEnvironmentVariables();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services); // calling ConfigureServices method

var app = builder.Build();

startup.Configure(app, builder.Environment);

app.Run();
