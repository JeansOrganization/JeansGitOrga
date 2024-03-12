using Microsoft.Extensions.Logging;

namespace SY.Windows.Test
{
    public class Test
    {
        public readonly ILogger<Test> logger;
        public Test(ILogger<Test> logger)
        {
            this.logger = logger;
        }

        public void test()
        {
            logger.LogTrace("这是一条Trace消息");
            logger.LogDebug("这是一条Debug消息");
            string age = "abc";
            logger.LogInformation("用户输入的年龄：{0}", age);
            logger.LogWarning("这是一条Warning消息");
            logger.LogError("这是一条Error消息");
            try
            {
                int i = int.Parse(age);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "解析字符串为int失败");
            }
            logger.LogCritical("这是一条Critical消息");
        }
    }
}
