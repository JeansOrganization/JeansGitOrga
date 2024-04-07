using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nlog.AspNetCore;
using System;

namespace Nlog.AspNetCore
{
    public class NLogHelper : INLogHelper
    {
        //public static Logger logger { get; private set; }

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly ILogger<NLogHelper> logger;

        public NLogHelper(IHttpContextAccessor httpContextAccessor, ILogger<NLogHelper> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        public void LogError(Exception ex)
        {
            LogMessage logMessage = new LogMessage();
            logMessage.IpAddress = httpContextAccessor.HttpContext.Request.Host.Host;
            if (ex.InnerException != null)
                logMessage.LogInfo = ex.InnerException.Message;
            else
                logMessage.LogInfo = ex.Message;
            logMessage.StackTrace = ex.StackTrace;
            logMessage.OperationTime = DateTime.Now;
            logMessage.OperationName = "admin";
            logger.LogInformation(LogFormat.ErrorFormat(logMessage));
        }
    }
}