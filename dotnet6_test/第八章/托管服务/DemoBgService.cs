
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using 托管服务;

public class DemoBgService : BackgroundService
{
    private readonly ILogger<DemoBgService> logger;
    private readonly IServiceScope serviceScope;

    public DemoBgService(ILogger<DemoBgService> logger,IServiceScopeFactory factory)
    {
        this.logger = logger;
        serviceScope = factory.CreateScope();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            //int i = 0;
            //int num = 1 / i;
            int num = serviceScope.ServiceProvider.GetRequiredService<TestService>().GetNum(5, 4);
            Console.Out.WriteLine("获取到的NUM:" + num);
            await Task.Delay(5000);
            string content = await File.ReadAllTextAsync("C:\\Users\\JeanG\\Desktop\\平台状态.txt");

            await Task.Delay(5000);
            logger.LogInformation(content);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}