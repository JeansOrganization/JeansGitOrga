using Microsoft.AspNetCore.Mvc.Filters;

namespace 动作筛选器
{
    public class MyActionFilter1 : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("MyActionFilter1:执行成功");
            var r = await next();
            if(r.Exception == null)
            {
                Console.WriteLine("MyActionFilter1.next:执行成功");
            }
            else
            {
                Console.WriteLine("MyActionFilter1.next:执行失败");
            }
        }
    }
}
