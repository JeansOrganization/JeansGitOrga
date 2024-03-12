using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SY.Windows.xxx
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
            logger.LogTrace("Test:这是一条Trace消息");
            logger.LogDebug("Test:这是一条Debug消息");
            string age = "abc";
            logger.LogInformation("Test:用户输入的年龄：{0}", age);
            logger.LogWarning("Test:这是一条Warning消息");
            logger.LogError("Test:这是一条错误消息");
            try
            {
                int i = int.Parse(age);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Test:解析字符串为int失败");
            }
        }
    }
}
