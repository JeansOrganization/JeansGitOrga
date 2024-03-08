using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 记录类型
{
    public class Test
    {
        public string first;
        public string second;
        public string third;

        public Test(string first,string second,string third) : this(second,third)
        {
            Console.WriteLine("first:" + first);
        }

        public Test(string second,string third)
        {
            Console.WriteLine("second:" + second);
            Console.WriteLine("third:" + third);
        }
    }
}
