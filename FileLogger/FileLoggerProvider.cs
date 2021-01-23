using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileLogger
{

    /// <summary>
    /// A provider of <see cref="FileLogger"/> instances.
    /// </summary>
    [ProviderAlias("File")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public FileLoggerOptions Options;

        /// <summary>
        /// Creates an instance of <see cref="FileLoggerProvider"/>.
        /// </summary>
        /// <param name="options">The options to create <see cref="FileLogger"/> instances with.</param>
        public FileLoggerProvider(IOptions<FileLoggerOptions> options)
        {
            Options = options.Value;

            if (!Directory.Exists(Options.FilePath))
            {
                Directory.CreateDirectory(Options.FilePath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}