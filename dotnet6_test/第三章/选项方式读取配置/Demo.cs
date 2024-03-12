using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 选项方式读取配置
{
    public class Demo
    {
        public readonly IOptionsSnapshot<DbSetting> DbSetting;
        public readonly IOptionsSnapshot<SmtpSetting> SmtpSetting;
        public Demo(IOptionsSnapshot<DbSetting> DbSetting, IOptionsSnapshot<SmtpSetting> SmtpSetting)
        {
            this.DbSetting = DbSetting;
            this.SmtpSetting = SmtpSetting;
        }

        public void test()
        {
            var dbSetting = DbSetting.Value;
            Console.WriteLine($"{dbSetting.DbType},{dbSetting.ConnectionString}");
            var smtpSetting = SmtpSetting.Value;
            Console.WriteLine($"{smtpSetting.address},{smtpSetting.username},{smtpSetting.password}");
        }


    }
}
