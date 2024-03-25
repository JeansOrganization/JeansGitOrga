
using Dynamic.Json;
using Newtonsoft.Json;

namespace 中间件入门案例
{
    public class CheckMiddleware
    {
        private readonly RequestDelegate next;
        /* 必须有一个构造函数带RequestDelegate参数 */
        public CheckMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /* 必须有一个Invoke或者InvokeAsync方法带context参数，且InvokeAsync方法返回值得是Task */
        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync("CheckMiddleware-start");
            var password = context.Request.Query["password"];
            if (password == "123")
            {
                if (context.Request.HasJsonContentType())
                {
                    var stream = context.Request.BodyReader.AsStream();
                    var jsonObj = DJson.Parse(stream);
                    context.Items["BodyJson"] = jsonObj;
                }
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
            await context.Response.WriteAsync("CheckMiddleware-end");
        }
    }
}
