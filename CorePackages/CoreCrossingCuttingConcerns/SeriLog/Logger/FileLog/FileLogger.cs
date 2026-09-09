using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.SeriLog.ConfigurationModels.FileLog;
using CoreCrossingCuttingConcerns.SeriLog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;


namespace CoreCrossingCuttingConcerns.SeriLog.Logger.FileLog
{
    public class FileLogger : LoggerServiceBase
    {
        private readonly IConfiguration _configuration;

        public FileLogger(IConfiguration configuration)
        {
            _configuration = configuration;

            FileLogConfiguration fileLogConfiguration = _configuration.GetSection("SeriLogConfiguration:FileLogConfiguration").Get<FileLogConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            string logFilePath = Directory.GetCurrentDirectory() + fileLogConfiguration.FilePath + ".txt";

            string? logDir = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDir))
                Directory.CreateDirectory(logDir);

            _logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.File(
            logFilePath, rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: null,
            fileSizeLimitBytes: 5000000,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
            ).CreateLogger();
        }
    }
}
