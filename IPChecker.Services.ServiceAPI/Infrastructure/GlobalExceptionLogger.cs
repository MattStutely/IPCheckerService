using System.Web.Http.ExceptionHandling;
using log4net;

namespace IPChecker.Services.ServiceAPI.Infrastructure
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (GlobalExceptionLogger));

        public override void LogCore(ExceptionLoggerContext context)
        {
            _log.Error("A global error occurred", context.Exception);
        }
    }
}
