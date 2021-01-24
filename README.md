# FileLogger
A logging provider that uses the .Net Core Logging API to add the possibility of logging to files

## Usage

You can either create the file logger like this anywhere in your code:

```C#
var loggerFactory = new LoggerFactory();

var folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs");
var fileName = _configuration["Logging:FileLogger:FileName"]
    .Replace("{date}", DateTime.Now.ToString("dd-MM-yyyy"));

loggerFactory.AddProvider(new FileLogger.ProviderBuilder(folderPath, fileName).FileLoggerProvider);

var fileLogger = loggerFactory.CreateLogger<Startup>();
```

Or like this in the ``CreateHostBuilder(string[] args)``-Method:
```C#
private static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args);

    var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    var fileLoggerPath = Path.Combine(assemblyPath, "Logs");
    
    hostBuilder.ConfigureLogging
    (
        (context, logging) =>
        {
            logging.ClearProviders();
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            logging.FileLogger(options =>
            {
                options.FilePath = fileLoggerPath;
                options.FileName = context.Configuration["Logging:FileLogger:FileName"]
                    .Replace("{date}", DateTime.Now.ToString("dd-MM-yyyy"));
            });
            logging.AddDebug();
            logging.AddConsole();
        }
    );
    
    hostBuilder.ConfigureWebHostDefaults
    (
        webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        }
    );

    return hostBuilder;
}
```

## AppSettings Example

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning"
    },
    "FileLogger": {
      "FileName": "log_{date}.log"
    }
  }
}
```
