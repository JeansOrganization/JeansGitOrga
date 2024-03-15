
using Microsoft.EntityFrameworkCore;
using 自引用的组织结构树;

using MySqlContext context = new MySqlContext();
//var u1 = new User() { Name = "校长" };
//var u2 = new User() { Name = "副校长1", Patent = u1 };
//var u3 = new User() { Name = "副校长2", Patent = u1 };
//var u4 = new User() { Name = "老师1", Patent = u2 };
//var u5 = new User() { Name = "老师2", Patent = u2  };
//var u6 = new User() { Name = "老师3", Patent = u3  };
//var u7 = new User() { Name = "老师4", Patent = u3  };
//var u8 = new User() { Name = "会计1", Patent = u2  };
//var u9 = new User() { Name = "会计2", Patent = u3 };
//context.Users.Add(u1);
//context.Users.Add(u2);
//context.Users.Add(u3);
//context.Users.Add(u4);
//context.Users.Add(u5);
//context.Users.Add(u6);
//context.Users.Add(u7);
//context.Users.Add(u8);
//context.Users.Add(u9);
//await context.SaveChangesAsync();

//var user = context.Users.Include(r => r.Childrens).ThenInclude(r => r.Childrens).Single(r => r.Id == 2);
//ConsoleUserAll(user);

//void ConsoleUserAll(User user, int level = 1)
//{
//    Console.WriteLine($"LEVEL{level}:{user.Name}");
//    if (user.Childrens?.Count > 0)
//    {
//        foreach (var child in user.Childrens)
//        {
//            ConsoleUserAll(child, level + 1);
//        }
//    }
//}
var user = context.Users.FirstOrDefault(r => r.ParentId == null);
ConsoleUserAll(user, context);

void ConsoleUserAll(User user, MySqlContext context, int level = 1)
{
    Console.WriteLine($"LEVEL{level}:{user.Name}");
    //由于IQueryable在遍历时是以IReader一条一条读取的，数据库链接占用较长，此时再递归调用再进行遍历会报错[This MySqlConnection is already in use]
    //var childrens = context.Users.Where(r => r.ParentId == user.Id);
    var childrens = context.Users.Where(r => r.ParentId == user.Id).ToList();
    foreach (var child in childrens)
    {
        ConsoleUserAll(child, context, level + 1);
    }
}