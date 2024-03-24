using Microsoft.AspNetCore.Mvc.Filters;

namespace 动作筛选器
{
    public class MyActionFilter2 : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("MyActionFilter2:执行成功");
            var r = await next();
            if(r.Exception == null)
            {
                Console.WriteLine("MyActionFilter2.next:执行成功");
            }
            else
            {
                Console.WriteLine("MyActionFilter2.next:执行失败");
            }
        }
    }
}
