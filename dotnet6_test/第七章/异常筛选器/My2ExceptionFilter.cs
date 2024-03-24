using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Xml.Serialization;

namespace 异常筛选器
{
    public class My2ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Exception exception = context.Exception;
            string message = exception.Source + ":" + exception.Message;
            ObjectResult result = new ObjectResult(new {code = 500,message});
            result.StatusCode = 500; //HTTP状态码
            context.Result = result;
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
