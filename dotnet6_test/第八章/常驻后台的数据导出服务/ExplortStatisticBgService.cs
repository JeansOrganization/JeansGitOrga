
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text;

namespace 常驻后台的数据导出服务
{
    public class ExplortStatisticBgService : BackgroundService
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<ExplortStatisticBgService> logger;
        private readonly IServiceScope serviceScope;
        private readonly IdDbContext dbContext;

        public ExplortStatisticBgService(ILogger<ExplortStatisticBgService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.serviceScope = factory.CreateScope();
            this.userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            this.dbContext = serviceScope.ServiceProvider.GetRequiredService<IdDbContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoExecuteAsync();
                    await Task.Delay(10000);
                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex.Message);
                    await Task.Delay(1000);
                }
            }
        }

        private async Task DoExecuteAsync()
        {
            var list = dbContext.Users.GroupBy(r => r.CreateTime.Date).Select(r => new { createDate = r.Key, count = r.Count() });
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Date:" + DateTime.Now.ToString());
            foreach (var li in list)
            {
                sb.Append($"时间:{li.createDate}").AppendLine($"数量:{li.count}");
            }
            logger.LogInformation( sb.ToString() );
            string content = await File.ReadAllTextAsync("E:\\Users\\Jean\\Desktop\\test.txt");
            content = content + "\r\n\r\n" + sb.ToString();
            await File.WriteAllTextAsync("E:\\Users\\Jean\\Desktop\\test.txt", content);
        }

        public override void Dispose()
        {
            base.Dispose();
            serviceScope.Dispose();
        }
    }
}
