using Microsoft.Extensions.Logging;
using SY.Windows.xxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SN.Windows.xxx
{
    public class Demo
    {

        public readonly ILogger<Demo> logger;
        public Demo(ILogger<Demo> logger)
        {
            this.logger = logger;
        }

        public void demo()
        {
            logger.LogTrace("Demo:这是一条Trace消息");
            logger.LogDebug("Demo:这是一条Debug消息");
            string age = "abc";
            logger.LogInformation("Demo:用户输入的年龄：{0}", age);
            logger.LogWarning("Demo:这是一条Warning消息");
            logger.LogError("Demo:这是一条错误消息");
            try
            {
                int i = int.Parse(age);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Demo:解析字符串为int失败");
            }
        }
    }
}
