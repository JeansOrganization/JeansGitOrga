
using Guid主键;

/* Guid */
//MyDbContext dbcontext = new MyDbContext();
//Teacher teacher = new Teacher() { name = "Lee", indate = DateTime.Now, salary = 7500 };
//Console.WriteLine(teacher.id);
//dbcontext.Teachers.Add(teacher);
//Console.WriteLine(teacher.id);
//await dbcontext.SaveChangesAsync();
//Console.WriteLine(dbcontext.Teachers.FirstOrDefault()?.id);

/* long自增 */
MyDbContext dbcontext = new MyDbContext();
Student student = new Student() { name = "Lee", indate = DateTime.Now };
Console.WriteLine(student.id);
dbcontext.Students.Add(student);
Console.WriteLine(student.id);
await dbcontext.SaveChangesAsync();
Console.WriteLine(dbcontext.Students.FirstOrDefault()?.id);