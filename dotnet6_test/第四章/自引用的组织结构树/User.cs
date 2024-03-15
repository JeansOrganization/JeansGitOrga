using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自引用的组织结构树
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public User? Patent { get; set; }
        public List<User>? Childrens { get; set; } = new List<User>();
        public long? ParentId { get; set; }
    }
}
