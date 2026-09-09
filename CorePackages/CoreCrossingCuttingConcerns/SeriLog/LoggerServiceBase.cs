using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreCrossingCuttingConcerns.SeriLog
{
    public abstract class LoggerServiceBase
    {
        protected ILogger _logger;

        public LoggerServiceBase()
        {
            _logger = null;
        }

        public LoggerServiceBase(ILogger logger)
        {
            _logger = logger;
        }

        public void Verbose(string message)=>_logger.Verbose(message);
        public void Debug(string message)=>_logger.Debug(message);
        public void Fatal(string message)=>_logger.Fatal(message);
        public void Information(string message)=>_logger.Information(message);
        public void Warning(string message)=>_logger.Warning(message);
        public void Error(string message)=>_logger.Error(message);


    }
}
