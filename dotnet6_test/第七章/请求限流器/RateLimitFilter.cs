using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

public class RateLimitFilter : IAsyncActionFilter
{
    private readonly IMemoryCache memoryCache;

    public RateLimitFilter(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string? ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        string key = "LastVisitTick_" + ipAddress;
        long? ticks = memoryCache.Get<long?>(key);
        long curTicks = Environment.TickCount64;
        if (ticks == null || curTicks - ticks > 1000)
        {
            memoryCache.Set(key, curTicks, TimeSpan.FromSeconds(10));
            return next();
        }

        context.Result = new ContentResult { StatusCode = 429 };
        return Task.CompletedTask;
    }
}