using Microsoft.EntityFrameworkCore;
using 普通的一对多配置;

/*Student s1 = new Student { Name = "tom" };
Student s2 = new Student { Name = "lily" };
Student s3 = new Student { Name = "lucy" };
Student s4 = new Student { Name = "tim" };
Student s5 = new Student { Name = "lina" };
Teacher t1 = new Teacher { Name = "杨中科" };
Teacher t2 = new Teacher { Name = "罗翔" };
Teacher t3 = new Teacher { Name = "刘晓艳" };
t1.Students.Add(s1);
t1.Students.Add(s2);
t1.Students.Add(s3);
t2.Students.Add(s1);
t2.Students.Add(s3);
t2.Students.Add(s5);
t3.Students.Add(s2);
t3.Students.Add(s4);
using MySqlContext ctx = new MySqlContext();
ctx.AddRange(t1, t2, t3);
//ctx.AddRange(s1, s2, s3, s4, s5);
await ctx.SaveChangesAsync();*/

using MySqlContext ctx = new MySqlContext();
foreach (var t in ctx.Teachers.Include(t => t.Students))
{
    Console.WriteLine($"老师{t.Name}");
    foreach (var s in t.Students)
    {
        Console.WriteLine($"---{s.Name}");
    }
}