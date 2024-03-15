
using EFCore实体配置DataAnnotations;

MyDbContext dbcontext = new MyDbContext();
dbcontext.Teachers.Add(new Teacher() { name="Lee",indate=DateTime.Now,salary=7500});
await dbcontext.SaveChangesAsync();