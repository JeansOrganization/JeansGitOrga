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

### 并发控制
- 并发控制概念:避免多个用户同时操作资源造成的并发冲突。
- 最好的解决方案：非数据库解决方案。
- 数据库层面的两种策略：悲观锁、乐观锁，更推荐乐观并发控制。
  
### 悲观并发控制
- 悲观并发控制一般采用行锁、表锁等排他锁对资源进行锁定，确保同时只有一个使用者操作被锁定的资源。
- EF Core没有封锁悲观并发控制的使用，需要开发人员编写原生SQL语句来使用悲观并发控制，不用数据库语法不同。
- MySQL方案:select * from T_Houses where Id=1 for update
```C#
Console.WriteLine("请输入您的姓名");
string name = Console.ReadLine();
using MySqlContext ctx = new MySqlContext();
/* 开启事务获取tx事务对象 */
using var tx = await ctx.Database.BeginTransactionAsync();
Console.WriteLine("准备Select " + DateTime.Now.TimeOfDay);
var h1 = await ctx.Houses.FromSqlInterpolated($"select * from T_Houses where Id=1 for update")
    .SingleAsync();
Console.WriteLine("完成Select " + DateTime.Now.TimeOfDay);
if (string.IsNullOrEmpty(h1.Owner))
{
    await Task.Delay(5000);
    h1.Owner = name;
    await ctx.SaveChangesAsync();
    Console.WriteLine("抢到手了");
}
else
{
    if (h1.Owner == name)
    {
        Console.WriteLine("这个房子已经是你的了，不用抢");
    }
    else
    {
        Console.WriteLine($"这个房子已经被{h1.Owner}抢走了");
    }
}
/* 提交事务解开行锁 */
await tx.CommitAsync();
Console.ReadKey();
```

### 乐观并发控制(并发令牌 & 并发令牌+RowVersion)
- 不涉及上锁解锁，性能上相较于悲观并发控制更好。
- 并发令牌只适用单个字段，并发令牌+RowVersion适用多个字段
1. 属性配置时调用并发令牌:```builder.Property(r=>r.Owner).IsConcurrencyToken();``` 
2. 在更新Owner字段时默认会带上旧字段的判断and Owner = oldvalue,故而后续的用户执行更新语句时返回0条被更新，会报DbUpdateConcurrencyException异常
3. 并发令牌+RowVersion:```builder.Property(r=>r.Rowver).IsRowVersion();``` 
4. Sql Server数据库支持每次更新插入都自动更新Rowver列值,在更新其他字段时默认会带上旧字段的判断and Rowver = OldRowverValue
5. 除Sql Server之外的数据库没法自动更新Rowver值，可以使用Guid值手动给RowVer赋值实现
```C#
Console.WriteLine("请输入您的姓名");
string name = Console.ReadLine();
using MyDbContext ctx = new MyDbContext();
var h1 = await ctx.Houses.SingleAsync(h => h.Id == 1);
if (string.IsNullOrEmpty(h1.Owner))
{
    await Task.Delay(5000);
    h1.Owner = name;
    try
    {
        await ctx.SaveChangesAsync();
        Console.WriteLine("抢到手了");
    }
    catch (DbUpdateConcurrencyException ex)
    {
        var entry = ex.Entries.First();
        var dbValues = await entry.GetDatabaseValuesAsync();
        string newOwner = dbValues.GetValue<string>(nameof(House.Owner));
        Console.WriteLine($"并发冲突，被{newOwner}提前抢走了");
    }
}
else
{
    if (h1.Owner == name)
    {
        Console.WriteLine("这个房子已经是你的了，不用抢");
    }
    else
    {
        Console.WriteLine($"这个房子已经被{h1.Owner}抢走了");
    }
}
Console.ReadLine();
```

# 第六章
## WebAPI
### WebAPI简介
- WebAPI(Web Application Programming Interface 网络应用程序接口):是一个可以对接各种客户端（浏览器、移动设备），构建 http 服务的框架。是 .net 技术体系下分布式开发的首选技术。与 WebService 和 WCF 相比较，更加的轻量级，传输效率更高。

### WebAPI风格
- 面向过程(RPC): 形如"URL/api/Controller/Action"，不关心请求方式
- Rest风格(RestFul)：根据http的语义来决定请求哪个接口，比如Get获取、Post新增、Put整体更新、delete删除、patch局部更新等
1. 优点：(1).见名知义，不同的http谓词表示不同的操作 (2). 通过状态码反应服务器处理结果
2. 缺点：(1).需要思考不同的操作到底用哪个谓词，不适合业务复杂的系统 (2).http状态码有限，无法应对所有情况 (3).有些客户端不支持put和delete请求
  
### Controller编写注意事项
- 请求特性:当Controller中存在public修饰的没有相应的[http*]的方法时,Swagger会报错，可以将public改成private,或者加上注解:[ApiExplorerSettings(IgnoreApi = true)]
- 方法的返回值:
1. Web API中Action方法的返回值如果是普通数据类型，那么返回值就会默认被序列化为Json格式。
2. Web API中的Action方法的返回值同样支持IActionResult类型，不包含类型信息，因此Swagger等无法推断出类型，所以推荐用ActionResult<T>，它支持类型转换，从而用起来更简单。
- 方法参数接收:
1. [FromRoute(Name="名字")],捕捉的值会被自动赋值给Action中同名的参数；如果名字不一致，可以用[FromRoute(Name="名字")]
2. [FromQuery]来获取QueryString中的值。如果名字一致，只要为参数添加[FromQuery]即可；而如果名字不一致，[FromQuery(Name = 名字)]
3. [FromForm] 从Content-Type为multipart/form-data的请求中获取数据的[FromForm]
4. [FromHeader]从请求报文头中获取值的[FromHeader]

# 第七章
## 服务注入
### 服务注入实现
- 在program类服务池里注册服务:builder.Services.AddScoped<MyService>();
- 构造函数引入与Action方法引入[FromServices] MyService myService
```C#
/* 构造函数引入 */
public readonly MyService myService;
public TestController(MyService myService)
{
    this.myService = myService;
}

/* Action方法引入 */
[HttpGet]
public ActionResult<int> GetFilesCount([FromServices]MyService myService, int x)
{
    int result = myService.GetFilesCount();
    return result;
}
```

### 第三方nuget包:Zack.Commons
- 适合跨项目依赖注入的情况
- 在需要依赖注入的类库里，新建类(命名随意，可以*Initializer命名),继承包内接口IModuleInitializer
- 在待实现接口方法Initialize里利用传入的参数 ```IServiceCollection services```,对当前项目里的服务类进行注册(其他类库同理)
- 之后在WebAPI项目里program.cs里通过 ```ReflectionHelper.GetAllReferencedAssemblies()``` 获取所有引用实例
- 使用获取的所有引用实例初始化DI容器 ```builder.Services.RunModuleInitializers(assemblies)``` ;

```C#
public class ServiceInitialize : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<SchoolService>();
    }
}

/* 获取所有引用实例，通过services运行这些实例的ModuleInitializers (program.cs) */
var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
builder.Services.RunModuleInitializers(assemblies);
```

## 缓存

### 缓存简介
- 缓存是一种性价比极高的程序性能优化方式，像数据库索引或者其他一些简单有效的优化功能本质上也是属于缓存
- 缓存顾名思义就是把一些数据按照某种缓存机制存储在内存或者硬盘中进行临时存放
- 缓存效率极高，访问速度极快，大部分请求都是获取缓存中的数据直接访问，在缓存获取不到数据时才去数据库进行获取，大大降低数据库的访问压力
- 缓存中的数据在更新到释放的这段时间如果数据库发生更新却没有对缓存做特殊处理，则两者之间容易出现数据差
- 多级缓存:在整个数据流转的链路中，每个结点可能都会有属于自己的缓存 比如:浏览器-网关服务器-web服务器-数据库服务器，在网关服务器存在缓存的情况下，有可能请求会在网关服务器就直接带着数据返回

### 客户端缓存
- 客户端缓存可以通过ResponseCache来实现，ResponseCache是一个特性，可以应用于控制器的动作方法或整个控制器(ResponseCache 属性仅在 HTTPGET 请求中生效)
- ResponseCache的属性:
1. Duration:指定缓存的过期时间(以秒为单位)
2. Location:通过设置 Location 属性，指定缓存的位置，可用的选项包括 Any、Client、和 None (Location = ResponseCacheLocation.Client)
3. NoStore:可以使用 NoStore = true 将缓存禁用，有时需要显式地禁用缓存，以确保每次请求都会从服务器获取最新的数据或内容
4. VaryByHeader:通过设置 VaryByHeader 属性，指定响应缓存应根据哪些请求标头进行变化

```C#
/* ResponseCache的属性 */
[ApiController]
[Route("[controller]/[Action]")]
// [ResponseCache(Duration = 15, NoStore = true, Location = ResponseCacheLocation.Client)]
public class TestController : ControllerBase
{
    //设置客户端缓存，间隔15秒释放并重新设置缓存
    [ResponseCache(Duration = 15)]
    [HttpGet]
    public ActionResult<DateTime> GetDateNow()
    {
        return DateTime.Now;
    }
    
    //ResponseCache设置缓存仅对HttpGet行为有效，所以调用GetDateNowPost并不会触发缓存机制
    [ResponseCache(Duration = 15)]
    [HttpPost]
    public ActionResult<DateTime> GetDateNowPost()
    {
        return DateTime.Now;
    }
}

```

### 服务器端缓存
- 通过缓存中间件配置实现
1. 在服务池里添加ResponseCaching:builder.Services.AddResponseCaching();
2. 在builder.Services.AddControllers方法内往options.CacheProfiles添加keyvalue对，value是CacheProfile对象，用于配置各种属性
3. 在builder建立获得app后调用app.UseResponseCaching()，就可以在Controller或者Action使用key值获取配置属性了:[ResponseCache(CacheProfileName = "")]
- 如果涉及到跨域的配置，设置服务器缓存的代码一定要放在配置跨域的代码后面(UseResponseCaching放在MapControllers前面，必须放在UseCors后面)
- 服务器缓存和客户端缓存区别:不同客户端的客户端缓存内容不同，但服务端缓存内容相同
1. 在未开启服务端缓存且开启客户端缓存的情况下,每个客户端的限制时间内的首次调用都是直接调用的服务器Action,后续则是直接调用的客户端缓存
2. 在同时开启了服务端缓存和客户端缓存的情况下,在限制时间内第一个调用Action的客户端是直接调用服务器Action,后续则是直接调用的服务端缓存
```C#
/* 缓存中间件配置 */
//program.cs
builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("jeanCache", new CacheProfile()
    {
        Duration = 60
    });
});
app.UseCors();
app.UseResponseCaching(); //UseResponseCaching放在MapControllers前面，必须放在UseCors后面
app.MapControllers();
//Controller
[ResponseCache(CacheProfileName = "jeanCache",Duration = 60)]
public class DemoController : ControllerBase
{
    [HttpGet]
    public ActionResult<DateTime> GetDateNow()
    {
        return DateTime.Now;
    }
}
```

### 内存缓存
- 启用内存缓存: ```builder.Services.AddMemoryCache();``` ，启用方法里注册了IMemoryCache服务，在需要使用的类里可以通过构造引入
- IMemoryCache里有GetOrCreateAsync、TryGetValue、Get、Set等方法，可以通过存储获取键值对实现内存缓存
- 优势:解决了服务器缓存和客户端缓存能够被用户屏蔽的弊端，与内存交互效率极高
```C#
/* program.cs */
builder.Services.AddMemoryCache();//相当于注入了IMemoryCache服务

/* 依赖注入 */
public class TestController : ControllerBase
{
    private readonly IMemoryCache memoryCache;
    public TestController(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }
}
```
#### 解决缓存与数据库数据不一致问题
- 说明:可能会出现缓存中数据与数据库数据不一致问题
- 解决:
1. 在数据更新时及时修改缓存内容，优点是很及时，但是写代码很麻烦
2. 设置过期时间，假设设置过期时间5秒，那么5秒后就更新缓存内存。设置很简单，但是依然会偶然存在缓存不一致问题。默认过期时间是永远，除非服务器重启
- 过期时间策略:
1. 绝对过期时间:AbsoluteExpirationRelativeToNow，可以设置一个有效期，有效期一过则清除缓存
2. 滑动过期时间:SlidingExpiration，可以设置一个有效期，如果在有效期内有人获取了该缓存，则有效期重置
3. 绝对过期时间与滑动过期时间混用:同时满足了绝对过期时间与滑动过期时间的规则
- 总结:大多数情况下用绝对过期时间策略，设置一个相对较短的过期时间,小部分情况使用混合策略，单独滑动过期时间策略几乎没有
```C#
/* memoryCache.GetOrCreateAsync */
var book = await memoryCache.GetOrCreateAsync(key, async e =>
{
    logger.LogInformation($"XXXXXXXXXX缓存中不存在，开始查找数据库中id为{id}的书");
    //绝对过期时间
    e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
    //滑动过期时间
    e.SlidingExpiration = TimeSpan.FromSeconds(10);
    return await bookService.GetBookByIdAsync(id);
});

/* memoryCache.Set */
memoryCache.Set(key,book, new MemoryCacheEntryOptions()
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20),
    SlidingExpiration = TimeSpan.FromSeconds(10)
});
```

#### 缓存穿透
- 问题:如果缓存中没有书籍缓存结果，将会调用数据库进行查询。如果在数据库中也没有该书籍信息，则将会得到一个空结果并将其写入缓存，在下一次查询该书籍时依然不会从缓存中获取数据而是继续进入数据库。这种情况被称为“缓存穿透”。
- 解决:可以将数据库找不到值这个结果存储到缓存，可以判断从缓存取出的值从而得知数据库没有值，也可以使用TryGetValue()直接返回布尔值判断是否获取到数据，或者GetOrCreateAsync()获取不到数据直接调用函数存入缓存

#### 缓存雪崩
- 问题:当有大量缓存同时过期时，容易造成数据库访问量剧增，导致数据库过载、应用服务器响应时间延迟等一系列问题。
- 可能存在原因:大量缓存设置了一样的有效期，除此之外还有硬件故障或宕机、热点数据、缓存服务端内存泄漏、分布式锁失效等等
- 解决:让数据库的访问量尽量保持平缓的趋势，如给缓存设置一定范围内随机的有效期 (Random.Shared)
```C#
//绝对过期时间
e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Random.Shared.Next(30, 40));
//滑动过期时间
e.SlidingExpiration = TimeSpan.FromSeconds(Random.Shared.Next(5,10));
```

#### 封装MemoryCache帮助类
- 需求:限制不允许传入IEnumerable、IQueryable延迟加载的数据，容易报错;实现随机缓存时间功能
```C#
public TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int expireSeconds = 60) where TResult : ICollection
{
    //ValidateValueType<TResult>(); //采用泛型约束
    if(!memoryCache.TryGetValue(cacheKey,out TResult? result))
    {
        using var entry = memoryCache.CreateEntry(cacheKey);
        result = valueFactory(entry);
        InitCacheEntry(entry, expireSeconds);
        entry.Value = result;
    }
    return result;
}

private void InitCacheEntry(ICacheEntry entry, int expireSeconds)
{
    TimeSpan timespan = TimeSpan.FromSeconds(Random.Shared.Next(expireSeconds, expireSeconds * 2));
    entry.AbsoluteExpirationRelativeToNow = timespan;
}
```

### 分布式缓存
- 缓存不再放置于web服务器中，而是存放在公共的缓存服务器中，所有服务器都可以共用缓存服务器中的缓存，此时缓存服务器被称为:[集中分布式缓存服务器]
- 分布式缓存与内存缓存优缺点:
1. 内存缓存:简单高效，但是不适用于多集群的项目，每个集群的内存缓存都存放在各自的集群上，没法共用，在访问量大的时候容易导致数据库超载
2. 分布式缓存:在集群节点数量非常多，访问量非常大时，适合使用分布式缓存，在其他情况下还是内存缓存占优
- 目前主流缓存服务器有:
1. Redis(推荐):缓存性能比起Memcached稍差，但是高可用、多集群等方面非常强大，且不局限于缓存，Redis还有其他好用功能
2. Memcached:缓存专用，性能极高，但是多集群、高可用等方面比较弱，而且有“缓存键的最大长度为250字节”等限制  nuget包:EnyimMemcachedCore
3. 数据库分布式缓存服务器:性能弱，几乎不使用

#### 分布式缓存-Redis




# 简书地址:https://www.jianshu.com/p/9f09fe043564





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