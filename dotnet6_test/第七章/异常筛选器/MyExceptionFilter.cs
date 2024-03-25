using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Xml.Serialization;

namespace 异常筛选器
{
    public class MyExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<MyExceptionFilter> logger;
        private readonly IHostEnvironment hostEnvironment;

        public MyExceptionFilter(ILogger<MyExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            Exception exception = context.Exception;
            string message = exception.Message;
            logger.LogError(message, "UnhandledException occured");
            if (hostEnvironment.IsDevelopment())
            {
                message = "未知错误！";
            }
            ObjectResult result = new ObjectResult(new {code = 500,message});
            result.StatusCode = 500; //HTTP状态码
            context.Result = result;
            context.ExceptionHandled = true; //表示异常是否已解决，true的话异常不会再被其他筛选器捕捉
            return Task.CompletedTask;
        }
    }
}
