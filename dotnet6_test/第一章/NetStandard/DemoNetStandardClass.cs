using System;
using System.IO;

namespace NetStandard类库
{
    public class DemoNetStandardClass
    {
        public static void test()
        {
            Console.WriteLine(typeof(FileStream).Assembly.Location);
            Console.ReadKey();
        }
    }
}
