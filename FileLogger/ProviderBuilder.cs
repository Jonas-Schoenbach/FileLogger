using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileLogger
{
    public class ProviderBuilder
    {
        public ILoggerProvider FileLoggerProvider { get; set; }
        
        public ProviderBuilder(string pathName, string fileName)
        {
            var fileConfigureNamedOptions = new ConfigureNamedOptions<FileLoggerOptions>
            (
                string.Empty,
                options =>
                {
                    options.FilePath = pathName;
                    options.FileName = fileName;
                }
            );

            var fileOptionsFactory = new OptionsFactory<FileLoggerOptions>
            (
                new[] {fileConfigureNamedOptions},
                Enumerable.Empty<IPostConfigureOptions<FileLoggerOptions>>()
            );

            var fileOptionsManager = new OptionsManager<FileLoggerOptions>(fileOptionsFactory);
            
            FileLoggerProvider =  new FileLoggerProvider(fileOptionsManager);
        }
    }
}