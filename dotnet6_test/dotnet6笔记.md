# dotnet6笔记

# 第二章

## 新旧using

### using新用法
```C#
var connStr = "server=127.0.0.1;Port=3306;user id=Jean;password=123456;database=jeandb";
using var conn = new MySqlConnection(connStr);
conn.Open();
using var command = conn.CreateCommand();
command.CommandText = "select * from fa_bazi_order";
```

### using旧用法
```C#
var connStr = "server=127.0.0.1;Port=3306;user id=Jean;password=123456;database=jeandb";
using (var conn = new MySqlConnection(connStr))
{
    conn.Open();
    using (command = conn.CreateCommand()){
        command.CommandText = "select * from fa_bazi_order";
        DataTable dt = new DataTable();
        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
        {
            adapter.Fill(dt);
        }
    }
}
```

## 记录类型

### record

- record是.NET 5中的一种新特性，可以看作是一种概念上不可变的类。
- records提供了重要的功能，如对象相等性、hashcode和解构。
- record是不可变的类型，括号中声明的属性在构造之后不可变更。
- records具有值语义。当比较两个records的实例时，比较的是这些实例的属性而非引用。(两个创建的实例只要属性相等实例就相等)
- 更适合用来声明一些简单的数据对象，如值对象和DTO

```C# 
## 对象相等性(值语义)
public record struct Person(string firstName, string lastName, int age); ==>指定声明struct
public record Person(string firstName, string lastName); ==>默认声明class
Person p1 =new("Jean","Lee");
Person p2 =new("Jean","Lee");
Console.WriteLine(p1 == p2); ==> true

## record构造括号内的属性初始化后不可修改(init)
p1.firstName = "DEQIANG"; ==>会报错，

## record可以像普通类一样扩展可变更的属性和自定义的方法
public record Person(string firstName, string lastName, int age)
{
    public required string PhoneNumber {get;set;}
    public static IEnumerable GetAll()
    {
        yield return newPerson("Jean","Lee",18) { PhoneNumber ="123456789"};
        yield return newPerson("Lucy","Fei",20) { PhoneNumber ="123456789" };
        yield return newPerson("Luffi","Monkey",20) { PhoneNumber ="123456789" }; ;
    }
    public string GetDisplayName() => $"{firstName} ({lastName})";
};

## record可以通过解构，将对象解构为元组，方便一次性获取record中的初始构造属性值
Person p1 =new("Jean","Lee");
var (firstName,lastName) = p1;
```
  
### with
- 当使用不可变的数据时，with可以从现存的值创建新值来呈现一个新状态(避免重新进行赋值，只修改需要修改的属性)
- 如果只是进行拷贝，不需要修改属性，那么无须指定任何属性修改
```C#
var person = new Person { FirstName = "Mads", LastName = "Nielsen" };
var otherPerson = person with { LastName = "Torgersen" };
var clonePerson = person with {}
```

## 异步编程

### await的基本使用
- await与wait()区别: await是异步等待，wait()是同步等待  await执行时线程返回线程池，执行完成后再重新获取新的线程，wait()始终在同一线程上会阻塞进程
- await需要与async配套使用
- 异步方法不添加await将不等待异步方法的执行，直接继续执行代码，容易出现进程冲突
```C#
string fileName = "d:/test/1.txt";
File.Delete(fileName);
string text = new string('a', 100000000);
File.WriteAllTextAsync(fileName, text);
string s = await File.ReadAllTextAsync(fileName);
Console.WriteLine(s);
/* 
* 调用await 或者.wait()会等待任务Task执行结束才往下执行其他内容
* 不加await 或者.wait()容易会出现线程冲突
*/
```

### Task.Delay() 和 Thread.Sleep()
- Thread.Sleep 是同步延迟，Task.Delay异步延迟。
- Thread.Sleep 会阻塞线程，Task.Delay不会。
- Thread.Sleep不能取消，Task.Delay可以。
- Task.Delay() 比 Thread.Sleep() 消耗更多的资源，但是Task.Delay()可用于为方法返回Task类型；或者根据CancellationToken取消标记动态取消等待
- Task.Delay() 实质创建一个运行给定时间的任务， Thread.Sleep() 使当前线程休眠给定时间。

```C#
using HttpClient httpClient = new HttpClient();
string h1 = await httpClient.GetStringAsync("http://www.baidu.com");
Console.WriteLine(h1);
Thread.Sleep(3000); //同步延迟:会阻塞进程
string h2 = await httpClient.GetStringAsync("https://www.ptpress.com.cn");
Console.WriteLine(h2);
await Task.Delay(3000); //异步延迟:不会阻塞进程
string h3 = await httpClient.GetStringAsync("https://www.rymooc.com");
Console.WriteLine(h3);
```

### async背后的线程切换
使用await+async异步方法时，当前线程会放置回线程池，等到异步方法执行完成，要继续往下执行时再从线程池里获取新的线程，大概率与之前线程不是同一线程，可用Thread.CurrentThread.ManagedThreadId去判断

### 不用async的异步方法
```C#
string content = await ReadFileAsync(num);
async Task<string> ReadFileAsync(int num)
{
    switch (num)
    {
        case 0:
            return await File.ReadAllTextAsync("d:/test/a.txt");
        case 1:
            return await File.ReadAllTextAsync("d:/test/b.txt");
        default:
            return "没有此选项";
    }
}
Task<string> ReadFileAsync(int num)
{
    switch (num)
    {
        /* 避免了拆箱之后再装箱，直接将Task<string>return出去，由调用方进行await获取数据 */
        case 0:
            return File.ReadAllTextAsync("d:/test/a.txt");
        case 1:
            return File.ReadAllTextAsync("d:/test/b.txt");
        default:
            /* 手动创建的Task对象，Task.FromResult(content) 和 Task.CompletedTask */
            return Task.FromResult("没有此选项");
            // return Task.CompletedTask;
    }
}
```

### Task.WhenAll 和 Task.WhenAny
- Task.WhenAll方法接受一个Task数组作为参数，返回一个新的Task，该Task会在所有传入的Task都完成后完成。
- Task.WhenAny方法也接受一个Task数组作为参数，返回一个新的Task，该Task会在任意一个传入的Task完成后完成。
```C#
/* Task.WhenAll()会等待task全部完成后再完成返回Task<result[]> */
string[] strArr = await Task.WhenAll(task1, task2, task3);
Console.WriteLine("5-thread" + Thread.CurrentThread.ManagedThreadId);
foreach (string str in strArr)
{
    Console.WriteLine(str);
    Console.WriteLine();
}

/* Task.WhenAny()只要任意一个Task执行成功就完成返回Task<Task<result>> */
string result = await await Task.WhenAny(task1, task2, task3);
Console.WriteLine(result);
```

# 第三章

## DI依赖注入
  依赖注入是一种设计模式，它允许对象之间的依赖关系由外部组件管理，而不是在对象内部直接创建它们的依赖项。

### 服务集合IServiceCollection注册的三种方式
- AddTransient: 每次service请求都是获得不同的实例，暂时性模式：暂时性对象始终不同，无论是不是同一个请求（同一个请求里的不同服务）同一个客户端，每次都是创建新的实例
- AddScoped: 对于同一个请求返回同一个实例，不同的请求返回不同的实例，作用域模式：作用域对象在一个客户端请求中是相同的，但在多个客户端请求中是不同的
- AddSingleton: 每次都是获得同一个实例， 单一实例模式：单一实例对象对每个对象和每个请求都是相同的，可以说是不同客户端不同请求都是相同的
```
AddSingleton的生命周期：项目启动-项目关闭 相当于静态类 只会有一个

AddScoped的生命周期：请求开始-请求结束 在这次请求中获取的对象都是同一个

AddTransient的生命周期：请求获取-（GC回收-主动释放） 每一次获取的对象都不是同一个

由于AddScoped对象是在请求的时候创建的，所以不能在AddSingleton对象中使用，甚至也不能在AddTransient对象中使用
```

## 配置系统

### 从json文件中读取配置
- 需引用包:Microsoft.Extensions.Configuration、Microsoft.Extensions.Configuration.Json
- 通过ConfigurationBuilder引入json配置文件创建配置根节点configRoot(json文件需设置成较新时复制)
- 获取数据方式
  1. configRoot["name"]
  2. configRoot.GetSection("proxy:address").Value
- 将根节点获取到的某个对象节点绑定成对象 (需要引入Microsoft.Extensions.Configuration.Binder包)
```C#
/* 通过ConfigurationBuilder引入json配置文件创建配置根节点configRoot (需要引入Microsoft.Extensions.Configuration.Json包) */
/* AddJsonFile() 中optional:false表示文件不可选，如果找寻不到文件会报错，reloadOnChange表示更改后是否实时更新 */
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("config.json",optional:false,reloadOnChange:false);
IConfigurationRoot configRoot = configBuilder.Build();

/* 通过根节点获取某个子节点的数据 第一层configRoot["name"]  多层configRoot.GetSection("proxy:address") */
Console.WriteLine("\n------①------");
string name = configRoot["name"];
var address0 = configRoot["proxy:address"];
var address = configRoot.GetSection("proxy:address").Value; //多层规范写法
var port = configRoot.GetSection("proxy:port").Value;
Console.WriteLine(name + " " + address0 + " " + address + " " + port);

/* 将根节点获取到的某个对象节点绑定成对象 (需要引入Microsoft.Extensions.Configuration.Binder包) */
Console.WriteLine("\n------②------");
Config config = configRoot.GetSection("proxy").Get<Config>();
Console.WriteLine("config.address:" + config.address + "," +
    "config.port:" + config.port + "," +
    "config.username:" + config.username + "," +
    "config.password:" + config.password);
```

### 从命令行引入配置
- 需引用包:Microsoft.Extensions.Configuration.CommandLine
- 在项目根目录启动命令行,输入***.exe key=value key1:key2=value2
```C#
/* 在项目根目录启动命令行,输入***.exe key=value key1:key2=value2 */
/* 从命令行读取配置.exe server=127.0.0.1 proxy:username=jean
    输出:server:127.0.0.1,username:jean */
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.AddCommandLine(args).Build();
var server = config["server"];
var username = config.GetSection("proxy:username").Value;
Console.WriteLine($"server:{server},username:{username}");
```

### 从环境变量引入配置
- 需引用包:Microsoft.Extensions.Configuration.Environmentvariables
- AddEnvironmentVariables(可选填前缀prefix,用于区分自己设置的环境变量与其他环境变量)
```C#
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.AddEnvironmentVariables(prefix: "jean_").Build();
string name = config["name"];
string address = config.GetSection("proxy:address").Value;
int[] ids = config.GetSection("proxy:ids").Get<int[]>();
Console.WriteLine($"name:{name},address:{address}");
foreach(int id in ids)
{
    Console.Write(id+",");
}
```


### 配置结合依赖注入
- 需引用包:Microsoft.Extensions.Configuration.Binder、Microsoft.Extensions.Options
- services.AddOptions() 注册服务IOptions<T>,IOptionsSnapshot<T>,IOptionsMonitor<T>,IOptionsFactory<T>,IOptionsMonitorCache<T>
  1. IOptions<T>:在配置改变后，我们不能读取新的值，必须重启程序。
  2. IOptionsMonitor<T>:配置改变后，可以读到新的值。
  3. IOptionsSnapshot<T>:配置改变后，可以读到新的值，与上者不同的是，上者在同一范围内会保持一致性。(常用)
- services.Configure<T> 通过lambda表达式绑定配置对象
```C#
/* AddOptions() 注册服务IOptions<T>,IOptionsSnapshot<T>,IOptionsMonitor<T>,IOptionsFactory<T>,IOptionsMonitorCache<T> 
 (需引入Microsoft.Extensions.Options包)*/
/* Configure<T> 通过lambda表达式绑定配置对象(需引入Microsoft.Extensions.Configuration.Binder) */
ServiceCollection services = new ServiceCollection();
services.AddOptions()
    .Configure<DbSetting>(e => configRoot.GetSection("DB").Bind(e))
    .Configure<SmtpSetting>(e => configRoot.GetSection("Smtp").Bind(e));

namespace 选项方式读取配置
{
    public class Demo
    {
        public readonly IOptionsSnapshot<DbSetting> DbSetting;
        public readonly IOptionsSnapshot<SmtpSetting> SmtpSetting;
        public Demo(IOptionsSnapshot<DbSetting> DbSetting, IOptionsSnapshot<SmtpSetting> SmtpSetting)
        {
            this.DbSetting = DbSetting;
            this.SmtpSetting = SmtpSetting;
        }
    }
}
```

## 日志系统

### 基本使用
- 需引用包:Microsoft.Extensions.Logging、Microsoft.Extensions.Logging.*(提供者)
- 日志级别(LogLevel):Trace< Debug< Information< Warning< Error< Critical
- 日志提供者(LoggingProvider):把日志输出到哪里。控制台、文件、数据库等
- 第三方日志组件(如：NLog、Serialog、Log4net)
```C#
/* 日志注入 */
public readonly ILogger<Test> logger;
public Test(ILogger<Test> logger)
{
    this.logger = logger;
}

/* 获取sppsettings.json配置 */
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
var config = configBuilder.AddJsonFile("appsettings.json", false, true).Build();

ServiceCollection services = new ServiceCollection();
services.AddLogging(logBuilder =>
{
    /* 添加日志提供者 */
    logBuilder.AddConsole();
    /* 将Logging配置绑定到Logger中，就可以通过配置操作过滤器、日志等级 */
    logBuilder.AddConfiguration(config.GetSection("Logging"));
    /* 设置允许展示的最小的日志等级 */
    logBuilder.SetMinimumLevel(LogLevel.Trace);
    /* provider校验日志提供者的名称，category校验日志发起方的命名空间，logLevel校验日志等级 */
    logBuilder.AddFilter((provider,category,logLevel) => {
        if(provider.Contains("Console") &&
            category.Contains("Test") &&
            logLevel >= LogLevel.Information)
        {
            return true;
        }
        return false;
    });
});
```
```JSON
/* appsettings.json */
{
  "Logging": {
    "Console": {
      "LogLevel": {
        "Default": "Trace",
        "SY.Windows.xxx.Test": "Debug",
        "SN.Windows.xxx.Demo": "Error"
      }
    },
    "LogLevel": {
      "Default": "Information",
    }
  }
}
```

### Nlog(第三方日志组件)
- 需引用包:NLog.Extensions.Logging
- 配置nlog.config 再调用AddNLog()
```C#
ServiceCollection services = new ServiceCollection();
services.AddLogging(logBuilder =>
{
    //logBuilder.AddConsole();
    logBuilder.AddNLog();
});

/* nlog.config */
<?xml version="1.0" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

	<!-- logger:发起方fullName message:日志消息 basedir:项目根路径 shortdate:年月日时分秒 -->
	<targets>
		<target name="file" xsi:type="File"
            layout="【${level}】${longdate} ${logger} ${message}${exception:format=ToString}"
            fileName="${basedir}/logs/${shortdate}-NLog.txt"
            keepFileOpen="true"
            encoding="utf-8" />
		<target name="jsonfile" xsi:type="File" fileName="${basedir}/logs/${shortdate}-${level}-NLog.json">
			<layout xsi:type="JsonLayout">
				<attribute name="time" layout="${date:format=O}" />
				<attribute name="message" layout="${message}" />
				<attribute name="logger" layout="${logger}"/>
				<attribute name="level" layout="${level}"/>
			</layout>
		</target>
		<target name="console" xsi:type="Console"
            layout="【${level}】${longdate} ${logger} ${message}${exception:format=ToString}"/>
	</targets>

	<!-- level:Trace、Debug、Info、Warn、Error、Fatal -->
	<!-- name为fullName的筛选 final="true"表示如果符合这条规则，则不需要再往后匹配规则 writeTo对应target的name -->
	<rules>
		<logger name="SY.Windows.*" minlevel="Debug" writeTo="jsonfile" />
		<logger name="SY.Windows.Demo.*" minlevel="Debug" maxlevel="Fatal" writeTo="file" final="true"/>
		<logger name="SY.Windows.Test.*" minlevel="Debug" writeTo="console"/>
	</rules>
</nlog>
```

# 第四章

## EFcore
Entity Framework Core(简称EF Core) 是.NET Core中的ORM (object relational mapping,对象关系映射) 框架。它可以让开发者面向对象的方式进行数据库操作。
ORM和ADO.NET是不一样的，ORM只是对ADO.NET的封装

### 数据迁移准备
- 需引用包Microsoft.EntityFrameworkCore.SqlServer(不同数据库需要安装不同的包)、Microsoft.EntityFrameworkCore.Tools
- 创建实体类<T>以及创建<T>EntityConfig(实现IEntityTypeConfiguration<T>)，用于配置实体类与数据库的对应关系，如不存在配置类则按默认处理
```C#
public class BookEntityConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("T_Book");
        builder.Property(r => r.id).HasComment("主键").IsRequired();
        builder.Property(r => r.name).HasMaxLength(50).HasComment("书名").IsRequired();
        builder.Property(r => r.price).HasComment("价格").HasDefaultValue(0);
    }
}
public class Book
{
    public int id { get; set; }
    public string name { get; set; }
    public double price { get; set; }
}
```
- 创建一个继承DbContext的*DbContext类
```C#
public class MyDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /* 配置所关联的数据库信息，如不存在则新建 */
        string connStr = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jeandb2";
        optionsBuilder.UseMySql(connStr, ServerVersion.Parse("8.2.0"));//new MySqlServerVersion(new Version(8.2.0)));

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
```

### 日志记录
- 简单的日志记录 LogTo
```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    string connStr = "server=127.0.0.1;port=3306;user=Jean;password=123456;database=jeandb2";
    optionsBuilder.UseMySql(connStr, ServerVersion.Parse("8.2.0"));
    //optionsBuilder.LogTo(Console.WriteLine);
    /* Console.WriteLine、Debug.WriteLine */
    optionsBuilder.LogTo(msg => _logStream.WriteLine(msg.Replace("`","")) , 
        new[] { DbLoggerCategory.Database.Command.Name },
        LogLevel.Information);
}
```

### EF Core两种配置方式
- Data Annotation(数据注解):通过.NET提供的Attribute对实体类，属性等进行标注的方式来实现实体类配置
- Fluent API:通过实现IEntityTypeConfiguration<T>来创建表配置文件<T>Config,实现接口方法Configure在里面配置表的各种配置
- 看起来很容易发现使用Data Annotation方法更简单，只需要在实体上加入Attribute即可，不用像Fluent API一样写单独的配置类，但是Fluent API是官方推荐的用法，原因如下:
1. Fluent API能够更好的进行职责划分，所有和数据库相关的内容都放在配置类中。
2. Fluent API和Data Annotation可以同时使用，但是Fluent API优先级高于Data Annotation
```C#
/* Data Annotation */
[Table("T_Teacher")]//配置表名
public class Teacher
{
    public Guid id { get; set; } //id默认Guid类型
    [MaxLength(40)]
    public string name { get; set; }
    [DataType("int")] 
    public double salary { get; set; }
    [Column("inputDate")] //配置列名
    public DateTime indate { get; set; }
}
/* Fluent API */
public class TeacherConfig : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("T_Teacher");
        builder.Property(r=>r.Name).HasMaxLength(256).IsRequired();
        //builder.Property("Name").HasMaxLength(256).IsRequired();
    }
}
```

### long自增与Guid的区别
```
long自增:
优点：自增long类型的使用非常简单，所有主流数据库都内置了对自增列的支持。新插入的数据都会由数据库自动赋值一个新增的、不重复的主键。而且占用磁盘空间小，可读性强。
缺点：自增long类型的主键在数据库迁移以及分布式系统（如分库分表，数据库集群）中使用非常麻烦，而且在高并发插入的时候性能比较差。
注:由于自增列的值一般是由数据库自动生成的，因此无法提前获得新增数据行的主键值，我们需要把数据保存到数据库后才能获得主键值

Guid:
优点：简单，高并发，全局唯一
缺点：磁盘占用空间大
注:由于Guid算法生成的值是不连续的，因此不能把Guid类型的主键设置为聚集索引，因为聚集索引是按顺序保存主键的，在插入Guid类型主键的时候，它将导致新插入的每天数据都要经历查找合适位置的过程，当数据量特别大的时候性能就很特别糟糕。Guid id的值在加入context时就已经生成，不需要保存数据库之后
```

### 关系配置
```C#
public void Configure(EntityTypeBuilder<Comment> builder)
{
    /* 一对多 */
    builder.HasOne<Article>(r => r.article).WithMany(t => t.comments).IsRequired()
        .HasForeignKey(r=>r.articleid);
    /* 一对一 */
    builder.HasOne(r => r.Delivery).WithOne(t => t.Order).HasForeignKey<Delivery>(r=>r.OrderId).IsRequired();
    /* 多对多 需要使用.UsingEntity()配置中间表，无需创建中间表实体类 */
    builder.HasMany(r => r.Teachers).WithMany(r => r.Students).UsingEntity("T_TeacherToStudent");
    /* 单向导航 */
		builder.HasOne<User>(l => l.Requester).WithMany();
		builder.HasOne<User>(l => l.Approver).WithMany();
    /* 自引用组织结构树 */
    builder.HasOne<User>(r => r.Patent).WithMany(r=>r.Childrens).HasForeignKey(r=>r.ParentId);
}

/* 关联查询时，需要用.Include()将需要关联的数据包含进来 */
var Articles = ct.Articles.Include(r=>r.comments).Where(r => r.comments.Any(t => t.message.Contains("棒")));
foreach (var a in Articles)
{
    Console.WriteLine(a.title + ":" + a.content);
    foreach (var c in a.comments)
    {
        Console.WriteLine(c.message);
    }
}

/* 需注意 */
void ConsoleUserAll(User user, MySqlContext context, int level = 1)
{
    Console.WriteLine($"LEVEL{level}:{user.Name}");
    /* 由于IQueryable在遍历时是以IReader一条一条读取的，数据库链接占用较长，此时再递归调用再进行遍历会报错[This MySqlConnection is already in use] */
    //var childrens = context.Users.Where(r => r.ParentId == user.Id);
    var childrens = context.Users.Where(r => r.ParentId == user.Id).ToList();
    foreach (var child in childrens)
    {
        ConsoleUserAll(child, context, level + 1);
    }
}

```

# 第五章

## EFCore

### IQueryable底层读取数据
- IQueryable遍历时，是调用DataReader获取数据,使用ToArray()、ToArrayAsync()、ToList()、ToListAsync()等方法时调用DataTable
- 使用DataReader优点与缺点
1. 优点:节省客户端内存
2. 缺点:处理慢的话会长时间占用数据库链接

### 自定义sql语句执行方法
- 非查询语句:context.Database.ExecuteSqlInterpolated(),入参传入formatString对象
- 查询语句:context.Books.FromSqlInterpolated(),入参传入formatString对象
- 任意语句:context.Database.GetDbConnection(),获取数据库连接，采用传统方式获取数据或者更新数据
```C#
#region ExecuteSqlInterpolatedAsync(控制台插入程序)
while (true)
{
	string name="";
   while (name.Equals(""))
	{
       Console.WriteLine("请输入您的姓名(按Enter确认):");
       name = Console.ReadLine()?.Trim();
		if(name.Equals("")) Console.WriteLine("姓名不能为空！！");
   }

   string bookName = "";
   while (bookName.Equals(""))
   {
       Console.WriteLine("请输入您要售卖的书(按Enter确认):");
       bookName = Console.ReadLine()?.Trim();
       if (bookName.Equals("")) Console.WriteLine("书名不能为空！！");
   }

   string priceStr = "";
   double price = 0;
   while (!double.TryParse(priceStr, out price))
   {
       Console.WriteLine("请输入您要售卖的价格(按Enter确认):");
       priceStr = Console.ReadLine()?.Trim(); 
       if (!double.TryParse(priceStr, out price))
       {
           Console.WriteLine("价格输入不规范，请重新输入!!!");
       }
   }

	ConsoleKeyInfo? keyInfo = null;
	while(keyInfo?.Key != ConsoleKey.F && keyInfo?.Key != ConsoleKey.Enter)
   {
       Console.WriteLine($"请确认您的售卖信息是否无误:[书名:{bookName},作者名:{name},售卖价格:{price}],无误请按Enter确认,否则按F重新输入");
       keyInfo = Console.ReadKey();
		if(keyInfo?.Key != ConsoleKey.F && keyInfo?.Key != ConsoleKey.Enter)
		{
           Console.WriteLine("请输入正确的按键进行选择！！");
       }
   }

	if(keyInfo?.Key == ConsoleKey.F)
	{
       Console.WriteLine();
       continue;
	}
	if(keyInfo?.Key == ConsoleKey.Enter)
	{
		int times = await context.Database.ExecuteSqlInterpolatedAsync
			($"INSERT INTO t_book (ID, Title, PubTime, Price, AuthorName) VALUES ({Guid.NewGuid()},{bookName}, SYSDATE(), {price}, {name})");
       if(times == 1) Console.WriteLine("插入成功！！"); else { Console.WriteLine("插入失败！！"); }
		break;
   }
}
#endregion

#region FromSqlInterpolated(控制台查询程序)

Console.WriteLine("请输入作者姓名搜索书籍(支持模糊搜索):");
string name = Console.ReadLine();
name = $"%{name}%";
IQueryable<Book> books = context.Books.FromSqlInterpolated($"select* from t_book a where a.AuthorName like {name}");
foreach (var book in books)
/* FromSqlInterpolated数据再加工 */
//foreach (var book in books.Take(2))
{
   Console.WriteLine($"ID:{book},书名:{book.Title},作者:{book.AuthorName},价格:{book.Price}元");
}

#endregion

#region 获取connect连接(执行任意语句)
using MySqlConnection conn = (MySqlConnection)context.Database.GetDbConnection();
if(conn.State != System.Data.ConnectionState.Open) conn.Open();
using var command = conn.CreateCommand();
command.CommandText = "select * from t_book";
#region DataTable
DataTable dt = new DataTable();
MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
dataAdapter.Fill(dt);
foreach(DataRow dr in dt.Rows)
{
   foreach (DataColumn dc in dt.Columns)
   {
       Console.Write(dc.ColumnName + ":" + dr[dc.ColumnName] + ",");
   }
   Console.WriteLine();
}
#endregion

#region DataReader
//using var reader = command.ExecuteReader();
//while (reader.Read())
//{
//    Console.WriteLine($"{reader.GetValue(0)}:{reader.GetValue(1)}");
//}
#endregion

#endregion
```

### AsNoTracking 与 EntityState
- .AsNoTracking()会将查询的数据状态设置为EntityState.Detached,可用于查询不需要更新或者删除的数据
- EntityState: EntityState.Added、EntityState.Deleted、EntityState.Detached、EntityState.Unchanged、EntityState.Modified
```C#
/*AsNoTracking*/
Book[] books = context.Books.AsNoTracking().Take(3).ToArray();
Book b1 = books[0];
b1.Title = "abc";
EntityEntry entry1 = context.Entry(b1);
Console.WriteLine(entry1.State);

/* EntityState */
Book b1 = context.Books.First(b => b.AuthorName=="Jean");
b1.Title = "Jean2333";
EntityEntry entry1 = context.Entry(b1);
Console.WriteLine(entry1.State);
context.SaveChanges();
Console.WriteLine(entry1.State);

/* 通过改变EntityState.Detached数据的state值,实现不查询就能操作数据(不推荐) */
Book b1 = new Book { Id = Guid.Parse("08dc44b6-cea9-4f9f-8c47-ce4bcde76dc7") };
b1.Title = "新书名";
var entry1 = context.Entry(b1);
entry1.Property("Title").IsModified = true;
Console.WriteLine(entry1.DebugView.LongView);
context.SaveChanges();
/*Book b1 = new Book { Id = Guid.Parse("08dc44b6-cea9-4f9f-8c47-ce4bcde76dc7") };
context.Entry(b1).State = EntityState.Deleted;
context.SaveChanges();*/

```








# 额外记录

## 构造函数继承

### Constructor:this()
- 调用Constructor构造函数时会先调用this()指向的同类构造函数
### Constructor:base()
- 调用Constructor构造函数时会先调用base()指向的父类构造函数

## 关键词
### yield
yield 关键字的用途是把指令推迟到程序实际需要的时候再执行，这个特性允许我们更细致地控制集合每个元素产生的时机，提高内存使用效率
- yield return:在迭代中一个一个返回待处理的值
- yield break：标识迭代中断，与yield return配合使用，当某个特定条件下，跳出循环，仅返回条件之前遍历的数据
- 可返回类型为 IAsyncEnumerable<T> 的异步迭代器
- 不能使用yield的情况：
    1. yield return 不能套在 try-catch 中
    2. yield break 不能放在 finally 中；
    3. yield 不能用在带有 in、ref 或 out 参数的方法
    4. yield 不能用在 Lambda 表达式和匿名方法
    5. yield 不能用在包含不安全的块（unsafe）的方法

```C#
/* yield return */
static IEnumerable<int> ProduceEvenNumbers(int upto)
{
    for (int i = 0; i <= upto; i += 2)
    {
        ConsoleExt.Write($"{i}-ProduceEvenNumbers");
        yield return i;
        ConsoleExt.Write($"{i}-ProduceEvenNumbers-yielded");
    }
    ConsoleExt.Write($"--ProduceEvenNumbers-循环结束");
}

/* yield break */
static IEnumerable<int> TakeWhilePositive(IEnumerable<int> numbers)
{
    foreach (int n in numbers)
    {
        if (n > 0) // 遇到负数就中断循环
        {
            yield return n;
        }
        else
        {
            yield break;
        }
    }
}

/* 返回类型为 IAsyncEnumerable<T> 的异步迭代器 */
public static async Task Main()
{
    await foreach (int n in GenerateNumbersAsync(5))
    {
        ConsoleExt.Write(n);
    }
    Console.ReadLine();
}
static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
{
    for (int i = 0; i < count; i++)
    {
        yield return await ProduceNumberAsync(i);
    }
}
static async Task<int> ProduceNumberAsync(int seed)
{
    await Task.Delay(1000);
    return 2 * seed;
}
```



- ......