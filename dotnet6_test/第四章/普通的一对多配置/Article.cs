using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 普通的一对多配置
{
    public class Article
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public List<Comment> comments { get; set; } = new List<Comment>();
    }
}
