using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.SeriLog.ConfigurationModels.FileLog;
using CoreCrossingCuttingConcerns.SeriLog.ConfigurationModels.MSSql;
using CoreCrossingCuttingConcerns.SeriLog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;

namespace CoreCrossingCuttingConcerns.SeriLog.Logger.MSSql
{
    public class MSSqlLogger:LoggerServiceBase
    {
        private readonly IConfiguration _configuration;

        public MSSqlLogger(IConfiguration configuration)
        {
            _configuration = configuration;

            MSSqlLogConfiguration mSSqlLogConfiguration = _configuration.GetSection("SeriLogConfiguration:MsSqlLogConfiguration").Get<MSSqlLogConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            MSSqlServerSinkOptions mSSqlServerSinkOptions = new()
            {
                TableName = mSSqlLogConfiguration.TableName,
                AutoCreateSqlTable=mSSqlLogConfiguration.AutoCreatedSqlTable
            };

            ColumnOptions columnOptions = new();

            global::Serilog.Core.Logger seriLogConfig = new LoggerConfiguration().WriteTo
                .MSSqlServer(mSSqlLogConfiguration.ConnectionString, mSSqlServerSinkOptions, columnOptions: columnOptions).MinimumLevel.Verbose()
                .CreateLogger();

            _logger = seriLogConfig;
        }
    }
}
