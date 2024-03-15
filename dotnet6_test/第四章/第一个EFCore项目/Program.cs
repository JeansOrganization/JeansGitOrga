
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using 第一个EFCore项目;

using MyDbContext myDbContext = new MyDbContext();

#region 新增数据

//#region 删除所有数据

//myDbContext.Teachers.RemoveRange(myDbContext.Teachers);
//myDbContext.Students.RemoveRange(myDbContext.Students);
//myDbContext.Books.RemoveRange(myDbContext.Books);
//await myDbContext.SaveChangesAsync();

//#endregion

//List<Teacher> teacherList = new List<Teacher>()
//{
//    new Teacher(){name="TeacherZhang",salary=6500.00,indate=DateTime.Parse("2020-07-01")},
//    new Teacher(){name="TeacherHuang",salary=7500.00,indate=DateTime.Parse("2021-07-01")},
//    new Teacher(){name="TeacherXie",salary=10500.00,indate=DateTime.Parse("2020-07-01")},
//    new Teacher(){name="TeacherCai",salary=2500.00,indate=DateTime.Parse("2021-07-01")},
//    new Teacher(){name="TeacherFan",salary=6500.00,indate=DateTime.Parse("2020-07-01")}
//};
//List<Student> studentList = new List<Student>()
//{
//    new Student(){name="Jean",grade=6,indate=DateTime.Parse("2020-09-01")},
//    new Student(){name="Ting",grade=6,indate=DateTime.Parse("2020-09-01")},
//    new Student(){name="Qian",grade=6,indate=DateTime.Parse("2020-09-01")},
//    new Student(){name="Hao",grade=5,indate=DateTime.Parse("2021-09-01")},
//    new Student(){name="Yi",grade=5,indate=DateTime.Parse("2021-09-01")}
//};

//List<Book> bookList = new List<Book>()
//{
//    new Book(){name="钢铁是如何练成的",price=75.5},
//    new Book(){name="心理学",price=80.5},
//    new Book(){name="C++编程设计",price=80.5},
//    new Book(){name="数据结构与算法",price=80.5},
//    new Book(){name="计算机原理",price=70.6}
//};
//myDbContext.Teachers.AddRange(teacherList);
//myDbContext.Students.AddRange(studentList);
//myDbContext.Books.AddRange(bookList);
//await myDbContext.SaveChangesAsync();
#endregion

#region 查询数据

//var teachers = myDbContext.Teachers.Where(r => r.salary > 6500);
//Console.WriteLine("--教师信息--");
//foreach (var teacher in teachers)
//{
//    Console.WriteLine($"{{姓名:{teacher.name},薪资:￥{teacher.salary},入职时间:{teacher.indate.ToString("yyyy年MM月dd日")}}}");
//}
//Console.WriteLine("--教师信息--");

//var students = myDbContext.Students.Where(r => r.grade > 5);
//Console.WriteLine("--学生信息--");
//foreach (var student in students)
//{
//    Console.WriteLine($"{{姓名:{student.name},年级:{student.grade},入学时间:{student.indate.ToString("yyyy年MM月dd日")}}}");
//}
//Console.WriteLine("--学生信息--");

//var book = myDbContext.Books.Single(r => r.name.Contains("钢铁是"));
//Console.WriteLine("--书本信息--");
//Console.WriteLine($"{{书名:{book.name},价格:￥{book.price}}}");
//Console.WriteLine("--书本信息--");

#region 特色查询

Console.WriteLine("--教师薪资统计--");
var group = myDbContext.Teachers.GroupBy(r => r.salary).OrderBy(t => t.Key)
    .Select(r => new { salary = r.Key, count = r.Count(), namestr = string.Join(',', r.Select(r => r.name)) });
foreach (var item in group)
{
    Console.WriteLine($"{{薪资:{item.salary},人数:{item.count},姓名:[{item.namestr}]}}");
}
Console.WriteLine("--教师薪资统计--");
#endregion

#endregion

#region 更改数据

//myDbContext.Books.SingleOrDefault(r => r.name.Contains("钢铁是如何")).price += 300;
//await myDbContext.Students.ForEachAsync(r => r.grade += 1);
//await myDbContext.Teachers.ForEachAsync(r => r.salary += 1000);
//await myDbContext.SaveChangesAsync();

#endregion

#region 删除数据

//myDbContext.Teachers.RemoveRange(myDbContext.Teachers.Where(r => r.salary > 10000));
//myDbContext.Students.RemoveRange(myDbContext.Students.Where(r => r.grade == 6));
//myDbContext.Books.RemoveRange(myDbContext.Books.Where(r => r.price > 400));

//await myDbContext.SaveChangesAsync();

#endregion


