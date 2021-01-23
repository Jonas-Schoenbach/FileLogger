using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;

namespace FileLogger
{
    public class FileLogger : ILogger
    {
        private readonly FileLoggerProvider _fileLoggerProvider;

        public FileLogger([NotNull] FileLoggerProvider fileLoggerProvider)
        {
            _fileLoggerProvider = fileLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            var path = Path.Combine(_fileLoggerProvider.Options.FilePath, _fileLoggerProvider.Options.FileName);

            var optionalException = exception != null ? exception.StackTrace : "";

            var logRecord = $"[{logLevel}] {formatter(state, exception)} {optionalException}";

            using (var streamWriter = new StreamWriter(path, true))
            {
                streamWriter.WriteLine(logRecord);
            }
        }
    }
}