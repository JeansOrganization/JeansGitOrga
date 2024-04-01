using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore对实体属性操作的秘密
{
    public class Dog
    {
        public long Id { get; set; }
        private string name;
        public string Name
        {
            get
            {
                Console.WriteLine("Getter被调用");
                return name;
            }
            set
            {
                this.name = value;
                Console.WriteLine("Setter被调用");
            }
        }
    }
}
