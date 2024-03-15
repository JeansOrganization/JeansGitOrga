using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 普通的一对多配置
{
    public class Comment
    {
        public int id { get; set; }
        public string message { get; set; }
        public Article article { get; set; }
        public int articleid { get; set; }
    }
}
