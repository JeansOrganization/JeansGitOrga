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
- nuget包:Microsoft.Extensions.Caching.StackExchangeRedis
- 使用 ```Services.AddStackExchangeRedisCache``` 注册redis,在参数options里配置连接属性Configuration及键值前缀InstanceName
- 在需要使用redis缓存的类里使用构造注入IDistributedCache
```C#
/* program.cs */
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "127.0.0.1:6379,password=123456,defaultDatabase=0,connectTimeout=5000,syncTimeout=1000";
    options.InstanceName = "jean_";
});

/* 构造注入IDistributedCache缓存 */
private readonly IDistributedCache distributedCache;
public TestController(IDistributedCache distributedCache)
{
    this.distributedCache = distributedCache;
}

/* 缓存使用 */
public TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, TResult?> valueFactory, int expireSeconds = 60)
{
    string? resultString = cache.GetString(cacheKey);
    /* Redis如果遇到null数据存入缓存会将null转成字符串"null"存入缓存,
        * 取用时反序列化会将"null"转化成null,从而校验为null的话返回[没获取到数据],
        * 可以用来区分是缓存中不存在该数据还是缓存中存入[数据库不存在该数据]两种情况 */
    if (resultString == null)
    {
        var options = new DistributedCacheEntryOptions();
        var result = valueFactory(options);
        InitOptionsExpireSeconds(options, expireSeconds);
        cache.SetString(cacheKey, JsonSerializer.Serialize(result), options);
        return result;
    }
    else
    {
        //Refresh()方法可以手动刷新该键对应值的滑动过期时间，调用GetString()方法获取键值时也会触发相同的重置效果
        cache.Refresh(cacheKey);
        return JsonSerializer.Deserialize<TResult>(resultString);
    }
}

```

## 配置系统集成

### ASP.Net Core默认添加的配置提供者
1. 加载现有的IConfiguration
2. 加载项目根目录下的appsettings.json
3. 加载项目根目录下的appsettings.{Environment}.json
4. 当程序运行在开发环境下，程序会加载"用户机密"配置
5. 加载环境变量中的配置
6. 加载命令行中的配置
- 如果有同名的配置，后加载的会覆盖前面加载的

### 运行环境配置
- 运行环境:由环境变量ASPNETCORE_ENVIRONMENT决定,能选择性的读取特定的appsettings.{Environment}.json配置 
- appsettings.{Environment}.json: Development(开发环境)、Production(生产环境)、 Staging(待发布测试环境)、Testing(测试环境)
- 读取方法:app.Environment.IsDevelopment()、app.Environment.EnvironmentName

### 配置系统综合案例
- 将主要的配置放置到专门存储配置的数据库，借用第三方库[Zack.AnyDBConfigProvider]实现从数据库获取配置到Configuration
- 存储配置数据库的sql语句存储在用户机密配置中
```C#
builder.Host.ConfigureAppConfiguration((_, configure) =>
{
    //获取所在环境对应的sql连接字符串
    string connStr = builder.Configuration.GetConnectionString("MySql");
    //连接数据库并获取相应配置到Configuration
    configure.AddDbConfiguration(() => new MySqlConnection(connStr));
});
/* 从数据库获取的配置 */
builder.Services.AddStackExchangeRedisCache(options =>
{
    
    options.Configuration = builder.Configuration.GetSection("Redis:configuration").Value;
    options.InstanceName = builder.Configuration.GetSection("Redis:instanceName").Value;
});
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();
builder.Services.AddScoped<BookService>();
```

## 多层EFCore项目
### 多层项目EFCore的使用
- 给实体类单独建一个项目，多个实体类可以建一个或者多个项目，再编写具体的配置信息、上下文DbContext
- DbContext里不再配置数据库连接，而是增加入参为DbContextOptions<当前上下文>的构造函数供WebAPI启动端进行注册(services.AddDbContext)并配置数据库连接

- 如果有CoreFirst的需求，则需要在实体项目里增加一个类实现IDesignTimeDbContextFactory，在实现方法里构造DbContextOptionsBuilder配置数据库连接
- 最后将builder.options作为入参传入实体类上下文构造函数入参内，则代码完成，将启动项目和包管理的默认项目都更改为EFCore项目进行Add-migration和update-database就可以进行CoreFirst了
```C#
/* 上下文构造函数 */
public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options):base(options)
    {
        
    }
}
/* EFCore临时启动类 */
public class BookDbContextFactory : IDesignTimeDbContextFactory<BookDbContext>
{
    public BookDbContext CreateDbContext(string[] args)
    {
        var builder =  new DbContextOptionsBuilder<BookDbContext>();
        builder.UseMySql("server=127.0.0.1;port=3306;user=Jean;password=123456;database=JeanTest", ServerVersion.Parse("8.2.0"));
        var myDbContext = new BookDbContext(builder.Options);
        return myDbContext;
    }
}
/* 上下文注入(带Options) */
builder.Services.AddDbContext<BookDbContext>(options =>
{
   string connStr = builder.Configuration.GetConnectionString("MySqlConnStr");
   string version = builder.Configuration.GetConnectionString("Version");
   options.UseMySql(connStr, ServerVersion.Parse(version));
});
```

## Filter(筛选器or过滤器)
- ASP.NET Core中的Filter的五种类型:Authorization filter(权限筛选器)、Resource filter(资源筛选器)、Action filter(动作筛选器)、Exception filter(异常过滤器)、Result filter(结果筛选器)。
- 所有筛选器一般有同步和异步两个版本，比如IActionFilter、IAsyncActionFilter接口
### Exception Filter(异常筛选器)
- 新建一个类实现接口 IAsyncExceptionFilter(异步) OR IExceptionFilter(同步) 
- 实现 OnExceptionAsync OR OnException 方法,只要项目里面抛出异常就会被捕捉执行该方法
- 在program.cs通过 builder.Services.Configure<MvcOptions>(options=>{options.Filters.Add<My2ExceptionFilter>();}) 注入
- 可以有多个异常筛选器,多个筛选器捕捉顺序按注入顺序的反序执行,谁后注入谁先执行
```C#
/* 筛选器 */
public class MyExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        Exception exception = context.Exception;
        string message = exception.Message;
        ObjectResult result = new ObjectResult(new {code = 500,message});
        result.StatusCode = 500; //HTTP状态码
        context.Result = result;
        context.ExceptionHandled = true; //表示异常是否已解决，true的话异常不会再被其他筛选器捕捉
        return Task.CompletedTask;
    }
}
/* 筛选器注入(后注入的先执行) */
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<My2ExceptionFilter>();
    options.Filters.Add<MyExceptionFilter>();
});
```

### Action Filter(动作筛选器)
- 新建一个类实现接口 IAsyncActionFilter(异步) OR IActionFilter(同步) 
- 实现 OnActionExecutionAsync OR OnActionExecution 方法,在调用Action之前会调用该方法
- 同时如果不执行 await next(),则不会再往后面的ActionFilter或者Action执行,而是直接退出,如果执行next()则会执行下一个ActionFilter或者Action
- 在program.cs通过 builder.Services.Configure<MvcOptions>(options=>{options.Filters.Add<MyActionFilter>();}) 注入
- 可以有多个动作筛选器,多个筛选器捕捉顺序按注入顺序的正序执行,谁先注入谁先执行,Action执行完后按反序执行next()
```C#
/* 筛选器 */
public class MyActionFilter1 : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("MyActionFilter1:执行成功");
        var r = await next();
        if(r.Exception == null)
        {
            Console.WriteLine("MyActionFilter1.next:执行成功");
        }
        else
        {
            Console.WriteLine("MyActionFilter1.next:执行失败");
        }
    }
}
/* 筛选器注入 */
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<MyActionFilter1>();
    options.Filters.Add<MyActionFilter2>();
});
```

### 自动启用事务的筛选器
- 通过构造TransactionScope实例实现,其中共有两个方法:Complete()、Dispose()
- 可通过特性Attribute判断哪些方法需要进行事务控制
```C#
/* 自动启用事务筛选器 */
public class TransactionScopeFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool hasNotTransactionalAttribute = false;
        if (context.ActionDescriptor is ControllerActionDescriptor)
        {
            hasNotTransactionalAttribute = ((ControllerActionDescriptor)context.ActionDescriptor).
                MethodInfo.IsDefined(typeof(NotTransactionalAttribute));
        }
        if (hasNotTransactionalAttribute)
        {
            await next();
            return;
        }
        using var transactionScope =  new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var result = await next();
        if(result.Exception == null)
        {
            transactionScope.Complete();
        }
    }
}
/* 自定义特性,标注的对象不进行事务控制 */
[AttributeUsage(AttributeTargets.Method)]//作用目标:Method
public class NotTransactionalAttribute : Attribute
{
}
```

### 请求限流器
- 实现1秒钟内只允许同个IP访问一次
```C#
public class RateLimitFilter : IAsyncActionFilter
{
    private readonly IMemoryCache memoryCache;
    public RateLimitFilter(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string? ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        string key = "LastVisitTick_" + ipAddress;
        long? ticks = memoryCache.Get<long?>(key);
        long curTicks = Environment.TickCount64;
        if (ticks == null || curTicks - ticks > 1000)
        {
            memoryCache.Set(key, curTicks, TimeSpan.FromSeconds(10));
            return next();
        }
        context.Result = new ContentResult { StatusCode = 429 };
        return Task.CompletedTask;
    }
}
```

## 中间件
### 中间件基础写法
```C#
/* program.cs */
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Map("/test", async pipeBuilder =>
{
    pipeBuilder.UseMiddleware<CheckMiddleware>();
    pipeBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("1-start</br>");
        await next.Invoke();
        await context.Response.WriteAsync("1-end</br>");
    });
    pipeBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("2-start</br>");
        await next.Invoke();
        await context.Response.WriteAsync("2-end</br>");
    });
    pipeBuilder.Run(async context =>
    {
        var obj = context.Items["BodyJson"];
        await context.Response.WriteAsync(obj + "Run</br>");
        
    });
});
app.Run();
/* 中间类的写法 */
public class CheckMiddleware
{
    private readonly RequestDelegate next;
    /* 必须有一个构造函数带RequestDelegate参数 */
    public CheckMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /* 必须有一个Invoke或者InvokeAsync方法带context参数，且InvokeAsync方法返回值得是Task */
    public async Task InvokeAsync(HttpContext context)
    {
        await context.Response.WriteAsync("CheckMiddleware-start");
        var password = context.Request.Query["password"];
        if (password == "123")
        {
            if (context.Request.HasJsonContentType())
            {
                var stream = context.Request.BodyReader.AsStream();
                var jsonObj = DJson.Parse(stream);
                context.Items["BodyJson"] = jsonObj;
            }
            await next.Invoke(context);
        }
        else
        {
            context.Response.StatusCode = 401;
        }
        await context.Response.WriteAsync("CheckMiddleware-end");
    }
}
```

### Middleware(中间件) 与 Filter(筛选器) 的区别
- 中间件是ASP.NET Core这个基础提供的功能，而Filter是ASP.NET Core MVC中提供的功能。ASP.NET Core MVC是由MVC中间件提供的框架，而Filter属于MVC中间件提供的功能。
- 中间件可以处理所有的请求，而Filter只能处理对控制器的请求；中间件运行在一个更底层、更抽象的级别，因此在中间件中无法处理MVC中间件特有的概念。
- 中间件和Filter可以完成很多相似的功能。“未处理异常中间件”和“未处理异常Filter”；“请求限流中间件”和“请求限流Filter”的区别。
- 优先选择使用中间件；但是如果这个组件只针对MVC或者需要调用一些MVC相关的类的时候，我们就只能选择Filter。


# 第八章

## Identity标识框架
- 标识（Identity）框架：采用基于角色的访问控制（Role-Based Access Control，简称RBAC）策略，内置了对用户、角色等表的管理以及相关的接口，支持外部登录、2FA等。
- 标识框架使用EF Core对数据库进行操作，因此标识框架支持几乎所有数据库。

### 标识框架的使用
- Nuget包:Microsoft.AspNetCore.Identity.EntityFrameworkCore
- IdentityUser<TKey>、IdentityRole<TKey>，TKey代表主键的类型。我们一般编写继承自IdentityUser<TKey>、IdentityRole<TKey>等的自定义类，可以增加自定义属性
- 创建继承自IdentityDbContext的类
- 可以通过IdDbContext类来操作数据库，不过框架中提供了RoleManager、UserManager等类来简化对数据库的操作。
```C#
/* User类继承IdentityUser<long> */
public class User : IdentityUser<long>
{
}
/* Role类继承IdentityRole<long> */
public class Role : IdentityRole<long>
{
}
/* MyDbContext继承IdentityDbContext<User,Role,long>,采用构造函数带DbContextOptions<MyDbContext>参数写法,可使用服务注入,在program.cs配置连接字符串 */
public class MyDbContext : IdentityDbContext<User,Role,long>
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
/* 关于program.cs里的配置及服务注入 */
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseMySql("server=localhost;port=3306;user=Jean;password=123456;database=JeanTest", ServerVersion.Parse("8.2.0"));
});
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});
var identityBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
identityBuilder.AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders().
AddUserManager<UserManager<User>>().AddRoleManager<RoleManager<Role>>();
```

### UserManager与RoleManager的使用
```C#
/* 初始化管理员方法 */
public async Task<ActionResult> InitAdminAndUser()
{
    var roleExist = await roleManager.RoleExistsAsync("admin");
    if (!roleExist)
    {
        Role role = new Role() { Name = "admin" };
        var r = await roleManager.CreateAsync(role);
        if (!r.Succeeded) return BadRequest(r.Errors);
    }
    var user = await userManager.FindByNameAsync("jean");
    if(user == null)
    {
        user = new User() { UserName = "jean", Email = "771058475@qq.com",EmailConfirmed = true };
        var r = await userManager.CreateAsync(user,"123456");
        if (!r.Succeeded) return BadRequest(r.Errors);
        var result = await userManager.AddToRoleAsync(user, "admin");
        if (!result.Succeeded) return BadRequest(result.Errors);
    }
    return Ok();
}
/* 登录方法 */
public async Task<ActionResult<string>> Login(LoginRequest req)
{
    string username = req.username;
    string password = req.password;
    var user = await userManager.FindByNameAsync(username);
    if (user == null) return NotFound("用户不存在:" + username);
    if (await userManager.IsLockedOutAsync(user)) return BadRequest("正在锁定中，请稍后重试");
    var checkPassword = await userManager.CheckPasswordAsync(user, password);
    if (!checkPassword)
    {
        var r = await userManager.AccessFailedAsync(user);
        return BadRequest("密码错误!!");
    }
    else
    {
        await userManager.ResetAccessFailedCountAsync(user);
        return "登录成功";
    }
}
/* 通过旧密码修改密码 */
public async Task<ActionResult> ResetPassword(ResetPasswordByOldRequest req)
{
    string username = req.username;
    string password = req.password;
    string oldpassword = req.oldpassword;
    var r = await Login(new LoginRequest(username, oldpassword));
    if (r.Value == "登录成功")
    {
        var user = await userManager.FindByNameAsync(username);
        string token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok("修改成功");
    }
    return BadRequest("密码验证错误");
}
```

## JWT
- JSON Web Token（简称JWT），是一个开放标准（RFC 7519），它定义了一种紧凑且自包含的方式，用于在各方之间作为JSON对象安全地传输信息。
- 适用场景:
1. 授权：适用于单点登录。例：当用户登录成功时，服务器会返回一个token给当前登录用户，且用户登录后访问系统的各个模块都应携带该token进行请求，当token过期时，则不允许用户访问系统模块。
2. 信息交换：jwt是各端（客户端、服务器端）之间安全地传输信息的好方法。因为可以对JWT进行签名（例如，使用公钥/私钥对），发送方与接收方可通过约定好的加密秘钥进行数据的解析。

### JWT令牌结构
- header(头部):头部是令牌的第一部分，通常由两部分组成：令牌的类型（即JWT）和令牌所使用的签名算法，如SHA256、HMAC等。
- payload(有效载荷):有效载荷是令牌的第二部分，其中包含声明。声明是有关实体(通常是用户)和其他数据的声明。主要有以下三种类型： registered(注册的), public(公开的)和 private claims(私有的声明)。
1. 注册声明（非强制性声明）：主要包含iss（jwt发布者）、sub（面向的用户）、aud（接收方）、exp（过期时间）、iat（jwt签发时间）、jti（jwt身份标识）、nbf（在某个时间点前的token不可用）
2. 公开的声明：使用JWT的人员可以随意定义上述声明
3. 私有声明：提供方和接收方共同定义的声明
- signature(签名):签名是令牌的第三部分，由header和payload进行64位编码后再使用加密算法加密
```C#
/* header(头部) */
//编码后:eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
{
  "alg": "HS256",
  "typ": "JWT"
}

/* payload(有效载荷) */
//编码后:eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ
{
  "sub": "1234567890",
  "name": "John Doe",
  "admin": true
}

/* signature(签名) */
//编码后:eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
HMACSHA256(base64UrlEncode(header) + "." +base64UrlEncode(payload),secret);//secret为自定义的密码，如进行SHA256加密后的密码,一般长度大于等于16

/* 可在JWT官网(https://jwt.io/#debugger-io)中进行数据的解析 */
```

### JWT的基本使用-生成、校验、解析Token
```
<!-- 重要对象 -->
JwtSecurityToken:代表一个jwt token，可以直接用此对象生成token字符串，也可以使用token字符串创建此对象
SecurityToken:JwtSecurityToken的基类，包含基础数据
JwtSecurityTokenHandler:创建、校验token，返回ClaimsPrincipal
CanReadToken():确定字符串是否是格式良好的Json Web令牌(JWT)
ReadJwtToken(string token):token字符串转为JwtSecurityToken对象
ValidateToken(string token、TokenValidationParameters parameter，out SecurityToken validatedToken):校验token，返回ClaimsIdentity，
```

#### 方式一(推荐):引用NuGet包：System.IdentityModel.Tokens.Jwt
```C#
static void Main(string[] args)
{
    //引用System.IdentityModel.Tokens.Jwt
    DateTime utcNow = DateTime.UtcNow;
    string key = "f47b558d-7654-458c-99f2-13b190ef0199";
    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

    var claims = new List<Claim>() {
        new Claim("ID","1"),
        new Claim("Name","fan")
    };
    JwtSecurityToken jwtToken = new JwtSecurityToken(
        issuer: "fan",
        audience: "audi~~!",
        claims: claims,
        notBefore: utcNow,
        expires: utcNow.AddYears(1),
        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

    //生成token方式1
    string token1 = new JwtSecurityTokenHandler().WriteToken(jwtToken);
    //A Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor that contains details of contents of the token.

    var tokenDescriptor = new SecurityTokenDescriptor // 创建一个 Token 的原始对象
    {
        Issuer = "fan",
        Audience = "audi",
        Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "")
                }),
        Expires = DateTime.Now.AddMinutes(60),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256)
    };
    //生成token方式2
    SecurityToken securityToken = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
    var token2 = new JwtSecurityTokenHandler().WriteToken(securityToken);

    //校验token
    var validateParameter = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "fan",
        ValidAudience = "audi~~!",
        IssuerSigningKey = securityKey,
    };
    //不校验，直接解析token
    //jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token1);
    try
    {
        //校验并解析token
        var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token1, validateParameter, out SecurityToken validatedToken);//validatedToken:解密后的对象
        var jwtPayload = ((JwtSecurityToken)validatedToken).Payload.SerializeToJson(); //获取payload中的数据 
        
    }
    catch (SecurityTokenExpiredException)
    {
        //表示过期
    }
    catch (SecurityTokenException)
    {
        //表示token错误
    }
}
```

#### 方式二:引用Nuget包：JWT
```C#
/// <summary>
/// 创建token
/// </summary>
/// <returns></returns>
public static string CreateJwtToken(IDictionary<string, object> payload, string secret, IDictionary<string, object> extraHeaders = null)
{
    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
    IJsonSerializer serializer = new JsonNetSerializer();
    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
    var token = encoder.Encode(payload, secret);
    return token;
}
/// <summary>
/// 校验解析token
/// </summary>
/// <returns></returns>
public static string ValidateJwtToken(string token, string secret)
{
    try
    {
        IJsonSerializer serializer = new JsonNetSerializer();
        IDateTimeProvider provider = new UtcDateTimeProvider();
        IJwtValidator validator = new JwtValidator(serializer, provider);
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtAlgorithm alg = new HMACSHA256Algorithm();
        IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
        var json = decoder.Decode(token, secret, true);
        //校验通过，返回解密后的字符串
        return json;
    }
    catch (TokenExpiredException)
    {
        //表示过期
        return "expired";
    }
    catch (SignatureVerificationException)
    {
        //表示验证不通过
        return "invalid";
    }
    catch (Exception)
    {
        return "error";
    }
}

//-------------客户端调用---------------
public static void Main(string[] args)
{
    var sign = "123";
    var extraHeaders = new Dictionary<string, object>
                {
                                    { "myName", "limaru" },
            };
    //过期时间(可以不设置，下面表示签名后 10秒过期)
    double exp = (DateTime.UtcNow.AddSeconds(10) - new DateTime(1970, 1, 1)).TotalSeconds;
    var payload = new Dictionary<string, object>
                {
                                    { "userId", "001" },
                    { "userAccount", "fan" },
                    { "exp",exp }
                                };
    var token = CreateJwtToken(payload, sign, extraHeaders);
    var text = ValidateJwtToken(token, sign);
    Console.ReadKey();
}

```

#### 方式三:手写jwt算法
```C#
/* 
JWT组成
样式："xxxxxxxxxxxx.xxxxxxxxxxxxx.xxxxxxxxxxxxxxxx"由三部分组成.
(1).Header头部：{"alg":"HS256","typ":"JWT"}基本组成,也可以自己添加别的内容,然后对最后的内容进行Base64编码.
(2).Payload负载：iss、sub、aud、exp、nbf、iat、jti基本参数,也可以自己添加别的内容,然后对最后的内容进行Base64编码.
(3).Signature签名：将Base64后的Header和Payload通过.组合起来，然后利用Hmacsha256+密钥进行加密。 
*/
#region Base64编码
/// <summary>
/// Base64编码
/// </summary>
/// <param name="text">待编码的文本字符串</param>
/// <returns>编码的文本字符串</returns>
public string Base64UrlEncode(string text)
{
    var plainTextBytes = Encoding.UTF8.GetBytes(text);
    var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    return base64;
}
#endregion

#region Base64解码
/// <summary>
/// Base64解码
/// </summary>
/// <param name="base64UrlStr"></param>
/// <returns></returns>

public string Base64UrlDecode(string base64UrlStr)
{
    base64UrlStr = base64UrlStr.Replace('-', '+').Replace('_', '/');
    switch (base64UrlStr.Length % 4)
    {
        case 2:
            base64UrlStr += "==";
            break;
        case 3:
            base64UrlStr += "=";
            break;
    }
    var bytes = Convert.FromBase64String(base64UrlStr);
    return Encoding.UTF8.GetString(bytes);
}
#endregion Base64编码和解码

/// <summary>
/// 手写JWT算法
/// </summary>
public bool TestJwt1()
{
    string secretKey = Configuration["SecretKey"];
    //1.加密
    //1.1 表头的处理
    string headerBase64Url = this.Base64UrlEncode("{\"alg\":\"HS256\",\"typ\":\"JWT\"}");
    //1.2 PayLoad的处理
    var jwtPayLoad = new
    {
        expire = DateTime.Now.AddMinutes(15),
        userId = "00000000001",
        userAccount = "admin"
    };
    string payloadBase64Url = this.Base64UrlEncode(JsonConvert.SerializeObject(jwtPayLoad));
    //1.3 Sign的处理
    string sign = $"{headerBase64Url}.{payloadBase64Url}".HMACSHA256(secretKey);
    //1.4 最终的jwt字符串
    string jwtStr = $"{headerBase64Url}.{payloadBase64Url}.{sign}";

    //2.校验token是否正确
    bool result;   //True表示通过，False表示未通过
    //2.1. 获取token中的PayLoad中的值,并做过期校验
    JwtData myData = JsonConvert.DeserializeObject<JwtData>(this.Base64UrlDecode(jwtStr.Split('.')[1]));  //这一步已经获取到了payload中的值，并进行转换了
    var nowTime = DateTime.Now;
    if (nowTime > myData.expire)
    {
        //表示token过期，校验未通过
        result = false;
        return result;
    }
    else
    {
        //2.2 做准确性校验
        var items = jwtStr.Split('.');
        var oldSign = items[2];
        string newSign = $"{items[0]}.{items[1]}".HMACSHA256(secretKey);
        result = oldSign == newSign;  //true表示检验通过，false表示检验未通过
        return result;
    }
}
```

### JWT封装
- Nuget包:Microsoft.AspNetCore.Authentication.JwtBearer
- 配置JWT节点，创建SigningKey(密钥)、ExpireSeconds(过期间隔/秒)两个配置项，新建JwtOptions配置类接收配置
- 服务调用services.AddAuthentication().AddJwtBearer()方法，添加默认授权机制并配置令牌校验，在app.UseAuthorization()之前执行app.UseAuthentication()
- 在LoginAction里验证登录成功则生成Token，在其他需要进行登录验证控制器类或者Action方法上添加特性[Authorize],令牌自动解码数据会存放到this.User里
- 当控制类添加[Authorize]后，默认控制器下全部Action都需要验证，此时加上[AllowAnonymous]就可以不需要验证
```C#
/* Swagger添加Token输入 */
builder.Services.AddSwaggerGen(c =>
{
    var scheme = new OpenApiSecurityScheme()
    {
        Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    };
    c.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement();
    requirement[scheme] = new List<string>();
    c.AddSecurityRequirement(requirement);
});

/* JWT服务注入 */
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JWT"));
//默认授权机制名称JwtBearerDefaults.AuthenticationScheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtSetting = builder.Configuration.GetSection("JWT").Get<JwtSetting>();
    var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SigningKey!));
    //配置令牌校验
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = secKey,
        ValidateIssuerSigningKey = true
    };
});

/* appsettings.json添加JWT配置 */
{
  "JWT": {
    "SigningKey": "LIJIAJINGISABADBOY",
    "ExpireSeconds": "86400"
  }
}

/* Authorize配置是否需要验证token */
[ApiController]
[Route("[controller]/[Action]")]
[Authorize]
public class Demo2Controller : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult Hello()
    {
        string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string userName = this.User.FindFirst(ClaimTypes.Name)!.Value;
        IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
        string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
        return Ok($"id={id},userName={userName},roleNames ={roleNames}");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Hello2()
    {
        string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string userName = this.User.FindFirst(ClaimTypes.Name)!.Value;
        IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
        string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
        return Ok($"id={id},userName={userName},roleNames ={roleNames}");
    }
}
```

## 托管服务
- 托管服务是一种在应用程序启动时运行的服务，通常用于初始化和配置应用程序资源
### ASP.NET Core中的托管服务
- 在 ASP.NET Core 中，后台任务是作为托管服务（Hosted Service）实现的。 想要实现后台任务可以实现IHostedService接口或者直接继承BackgroundService抽象类
- 通常托管服务更多用来做定时任务，结合Timer计时器可以实现后台定时任务。这种方案比较适合简单的场景，对于任务的定时规则设置也很单一，也不用去监控任务的执行状态
- 对于更加复杂的应用场景，也可以使用功能更加强大的任务调度框架，来实现定时任务。这样的框架有:QuartZ.NET、HangFire

### ASP.NET Core中的托管服务的基本使用
- IHostedService接口(托管服务接口):
1. StartAsync(CancellationToken):当应用程序主机准备好启动服务时触发该方法
2. StopAsync(CancellationToken)：当应用程序主机执行正常关闭时触发该方法
- BackgroundService类:实现了IHostedService接口的抽象类,其中有一个抽象方法 ExecuteAsync(CancellationToken),只继承BackgroundService抽象类就满足大部分要求了
- 注册托管服务:builder.Services.AddHostedService<MyHostedService>();
- 由于托管服务是通过单例注入，所以没法在构造函数注入其他(scope)范围服务,可以通过注入IServiceScopeFactory类来创建IServiceScope再获取其他的范围服务
```C#
/* 托管服务类+伪定时 */
public class ExplortStatisticBgService : BackgroundService
{
    private readonly UserManager<User> userManager;
    private readonly ILogger<ExplortStatisticBgService> logger;
    private readonly IServiceScope serviceScope;
    private readonly IdDbContext dbContext;

    public ExplortStatisticBgService(ILogger<ExplortStatisticBgService> logger, IServiceScopeFactory factory)
    {
        this.logger = logger;
        this.serviceScope = factory.CreateScope();
        //通过注入的IServiceScopeFactory来获取其他服务实例
        this.userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        this.dbContext = serviceScope.ServiceProvider.GetRequiredService<IdDbContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {   
            /* 托管服务中发生未处理异常的时候，程序就会自动停止并退出，所以需要在trycatch内部进行任务的布置 */
            try
            {
                await DoExecuteAsync();
                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                await Task.Delay(1000);
            }
        }
    }
    //需要重写Dispose方法在释放时将一些非托管资源释放
    public override void Dispose()
    {
        base.Dispose();
        serviceScope.Dispose();
    }
}
/* 单例服务注入 */
builder.Services.AddHostedService<ExplortStatisticBgService>();
```

https://blog.csdn.net/xxxcAxx/article/details/128391524
https://blog.csdn.net/ousetuhou/article/details/135081996

## 数据校验

### 内置数据校验(DataAnnotations)
- 命名空间System.ComponentModel.DataAnnotations内置数据校验支持
- 直接在模型类属性上使用[*]即可进行数据校验:[Required]、[EmailAddress] 、[RegularExpression]、CustomValidationAttribute(自定义验证)、IValidatableObject接口
- 内置的校验机制的问题：校验规则都是和模型类耦合在一起，违反“单一职责原则”；很多常用的校验都需要编写自定义校验规则
```C#
/* 内置数据校验 */
public class Login1Request
{
    [Required]
    [EmailAddress]
    [RegularExpression("^.*@(qq|163)\\.com$", ErrorMessage = "只支持QQ和163邮箱")]
    public string Email { get; set; }
    [Required]
    [StringLength(16,MinimumLength = 8)]
    public string password { get; set; }
    [Compare(nameof(password),ErrorMessage = "两次密码必须相同")]
    public string password2 { get; set; }
}
```

### FluentValidation数据校验
- Nuget包:FluentValidation.AspNetCore
- 通过创建继承了AbstractValidator<Model>的ModelValidator,在其构造函数里编写校验规则RuleFor()
- 如需在校验类内注入其他服务，直接在构造函数注入即可
```C#
/* 模型校验类 */
public class Login2RequestValidator : AbstractValidator<Login2Request>
{
    public Login2RequestValidator(IdDbContext ctx)
    {
        //每个校验规则后面可跟WithMessage()用来定义报错信息
        RuleFor(x => x.username)
            .NotNull().WithMessage("用户名不能为空")
            .Must(r => ctx.Users.Any(t => t.UserName == r)).WithMessage("不存在于数据库中！");
        RuleFor(x => x.Email)
            .NotNull().WithMessage("邮箱不能为空")
            .EmailAddress().WithMessage("不是正确的邮箱地址")
            .Must(r => r.EndsWith("qq.com") || r.EndsWith("163.com")).WithMessage("您的邮箱不是qq邮箱或者网易邮箱");
        RuleFor(x => x.password)
            .NotNull().WithMessage("密码不能为空")
            .Length(8, 16).WithMessage("密码必须是8到16位");
        RuleFor(x=>x.password2)
            .Equal(r=>r.password).WithMessage("密码必须一致");
    }
}
/* 服务注册(貌似弃用) */
builder.Services.AddFluentValidation(fv =>
{
    Assembly assembly = Assembly.GetExecutingAssembly();
    fv.RegisterValidatorsFromAssembly(assembly);
});
```

## SignalR
### 双向通信技术 WebSocket 和 SignalR
- WebSocket:是一种在单个 TCP 连接上提供全双工通信的协议。它通过在客户端和服务器之间建立持久连接，实现了双向通信，可以在客户端和服务器之间发送消息，而不需要频繁地创建和关闭连接。
1. 双向通信：WebSocket 允许客户端和服务器之间实现双向通信，这使得实时应用程序开发变得更加简单和高效。
2. 持久连接：WebSocket 在初始握手后，保持持久连接，避免了每次通信都需要重新建立连接的开销。
3. 较低的开销：相比传统的 HTTP 请求，WebSocket 通信的开销较低，因为不需要频繁地建立和关闭连接。

- SignalR:是由 Microsoft 开发的 ASP.NET Core 框架中的一个库，用于实现实时双向通信。它使用了多种传输方式（包括 WebSocket、Server-Sent Events（SSE）、长轮询等），以确保在不同环境下都能提供实时通信的能力。
1. 跨平台支持：SignalR 可以在多种客户端和服务器环境中运行，包括 Web、移动设备和桌面应用程序。
2. 自适应传输：SignalR 使用多种传输方式，自动选择最佳传输方式以适应不同的客户端和服务器环境。
3. 高级 API：SignalR 提供了高级的 API，使开发实时应用程序更加简单和方便。它允许你定义服务器到客户端和客户端到服务器的消息传递方式，同时处理连接管理和错误处理。
4. 扩展性：SignalR 支持水平扩展，可以在多个服务器节点上处理实时通信，从而满足大规模应用程序的需求。

- 总结:
1. WebSocket 是一种底层的协议，它提供了持久化的、双向的通信，可以在单个 TCP 连接上实现全双工通信。
2. SignalR 是一个高级的实时通信框架，它建立在 WebSocket 和其他传输方式之上，提供了更高级的 API 和跨平台支持，使实时应用程序的开发更加方便。

### SignalR架构
- Hub：Hub 是 SignalR 的核心组件，它是一个连接服务器和客户端的接口。Hub 提供了一种简单的方式来定义服务器端的方法和事件，这些方法和事件可以被客户端调用
- 连接器：连接器是 SignalR 的底层组件，它负责建立和管理服务器和客户端之间的连接。连接器可以处理各种协议，如 HTTP 和 WebSocket。
- 消息传递：SignalR 使用消息传递来在服务器和客户端之间进行通信。消息可以是文本、JSON 或其他格式。
- 传输协议：SignalR 支持多种传输协议，如 HTTP 长轮询、WebSocket 和 Server-Sent Events。传输协议的选择取决于客户端和服务器支持的协议。

### SignalR的基本使用
- 服务端:
1. 创建一个类继承Hub类，在类里编写一个反参是Task的方法，方法名为客户端调用Send()里的方法名
2. 需要在服务池里注册SignalR:builder.Services.AddSignalR()
3. 需要调用app.MapHub<ChatRoomHub>(“/Hubs/ChatRoomHub”)，字符串表示客户端创建链接时所使用的URL后缀
- 客户端:
1. 需要安装 @microsoft/signalr:npm install @microsoft/signalr
2. 在客户端挂载时先创建connection连接,URL格式:https://{ip地址}:{端口号}{后端所配置的后缀}
3. 开启链接:await connection.start()后绑定后端的回调方法名
4. 绑定事件执行发送消息方法
```C#
/* Hub继承类 */
public class CharHub : Hub
{
    //SendPublicMessage与前端发送方法对应
    public Task SendPublicMessage(string msg)
    {
        var connectionId = this.Context.ConnectionId;
        string message = $"{connectionId}--{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")}--{msg}";
        //ReceiptPublicMessage绑定前端回调方法
        return this.Clients.All.SendAsync("ReceiptPublicMessage", message);
    }
}

/* program.cs */
builder.Services.AddSignalR();
var app = builder.Build();

app.MapControllers();
app.UseCors(opt =>
{
    //不要允许所有源，因为这样编译会报错，单独筛选http://localhost:8080前端的域名
    opt.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});
app.MapHub<CharHub>("/Hub/CharHub");//前端连接URL后缀
app.Run();

/* 前端代码 */
<script>
import { onMounted, reactive } from 'vue'
import * as signalR from '@microsoft/signalr'
let connection
export default {
  name: 'HelloWorld',
  setup(){
    let state = reactive({userMessage:'',allMessage:[]})
    async function keyPress(e){
      if(e.keyCode != 13) return;
      await connection.invoke('SendPublicMessage',state.userMessage)
      state.userMessage = ''
    }
    onMounted(async function(){
      connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7069/Hub/CharHub')
      .withAutomaticReconnect().build();
      console.log(connection)
      await connection.start();
      connection.on('ReceiptPublicMessage',msg=>{
        state.allMessage.push(msg)
      });
    })
    return {
      state,
      keyPress
    }
  }
}
</script>
```

### SignalR协议协商问题
- 问题:“协商”请求被服务器A处理，而接下来的WebSocket请求却被服务器B处理
- 解决方法：粘性会话和禁用协商。
- 粘性会话:把来自同一个客户端的请求都转发给同一台服务器上。交给负载均衡服务器。 缺点：因为共享公网IP等造成请求无法被平均的分配到服务器集群；扩容的自适应性不强
- 禁用协商:直接向服务器发出WebSocket请求。WebSocket连接一旦建立后，在客户端和服务器端直接就建立了持续的网络连接通道，在这个WebSocket连接中的后续往返WebSocket通信都是由同一台服务器来处理。缺点：无法降级到“服务器发送事件”或“长轮询”，不过不是大问题。(更推荐)
```javascript
/* 组件挂载时创建连接 */
onMounted(async function(){
    const options = {skipNegotiation:true,transport: signalR.HttpTransportType.WebSockets}//跳过协商=true,运输:WebSockets
    connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7069/Hub/CharHub',options)//接入URL的同时应用Options
    .withAutomaticReconnect().build();
    console.log(connection)
    await connection.start();
    connection.on('ReceiptPublicMessage',msg=>{
    state.allMessage.push(msg)
    });
})
```

### SignalR分布式部署问题
- 问题:用户发送到某个服务端的消息只有连接那个服务端的用户能接收到，连接其他服务端的人接收不到
- 解决方案:所有服务器连接到同一个消息中间件(官方方案:Redis backplane)。前提条件：启用粘性会话，或者跳过协商
- Nuget包:Microsoft.AspNetCore.SignalR.StackExchangeRedis
```C#
/* program.cs */
builder.Services.AddSignalR().AddStackExchangeRedis("127.0.0.1",options =>
{
    options.Configuration.ChannelPrefix = "JeanSignal_";
});
```

### SignalR身份认证
- 后端服务:
1. 在program.cs里AddJwtBearer(options=>)方法里添加options.Events(JwtBearerEvents)事件的OnMessageReceived函数
2. 获取前端请求中的accessToken和path，校验该请求是否符合SignalR请求且需要校验，如是则赋值context.Token = accessToken
前端服务:
1. 定义options对象，给options.accessTokenFactory添加箭头函数 () => state.token，传入登录时获取到的token
2. 在建立连接connection时将options对象作为入参传入.withUrl()方法中

```C#
/* 后端服务 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtSetting = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
    var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SigningKey!));
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = secKey,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/Hub/ChatHub"))
                context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});
/* 前端服务 */
async function OpenConnection(){
    const options = {skipNegotiation:true,transport: signalR.HttpTransportType.WebSockets}
    options.accessTokenFactory = () => state.token;
    connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7055/Hub/ChatHub',options)
    .withAutomaticReconnect().build();
    await connection.start();
    connection.on('ReceivePrivateMessage',(msg,id)=>{
    state.fromUserMessage.push(msg)
    console.log('获取到id啦:' + id)
    });
}
```

 


# 末尾占位








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

## 特性(Attribute)
- 特性（Attribute）是用于在运行时传递程序中各种元素（比如类、方法、结构、枚举、组件等）的行为信息的声明性标签。
- 特性使用中括号声明，特性是一个类且必须直接或者间接继承 Attribute。 一个声明性标签是通过放置在它所应用的元素前面的方括号（[ ]）来描述的。
### 自定义特性(CustomAttribute)
- 必须直接或者间接地继承 Attribute
- 关联的元素:程序集(assembly)、模块(module)、类型(type)、属性(property)、事件(event)、字段(field)、方法(method)、参数(param)、返回值(return)
- 约定:所有自定义的特性名称都应该有个Attribute后缀。因为当你的Attribute施加到一个程序的元素上的时候，编译器先查找你的Attribute的定义，如果没有找到，那么它就会查找“Attribute名称"+Attribute的定义
- Attribute类是在编译的时候被实例化的，而不是像通常的类那样在运行时候才实例化
```C#
/* 特性 */
public class CustomAttribute : Attribute {
    public string Desc { get; set; }
    public CustomAttribute()
    {
        Console.WriteLine("CustomAttribute构造函数");
    }
    public CustomAttribute(string desc) { 
        this.Desc = desc;
        Console.WriteLine("CustomAttribute有参构造函数");
    }
}
/* 使用了特性的类 */
[Custom("我在类上使用")]
public class Student {
    [Custom("我在字段上使用")]
    public string name;
    [Custom("我在属性上使用")]
    public string Name { get { return name; } set { name = value; } }

    [Custom("我在方法上使用")]
    [return: Custom("我在返回值上")]
    public string GetName([Custom("参数")] int Id) {
        return name;
    }
}
/* 体现特性作用 */
static void Main(string[] args)
{
    Type type = typeof(Student);
    //判断是否在类上使用特性
    if (type.IsDefined(typeof(CustomAttribute), true))
    {
        CustomAttribute customAttribute = (CustomAttribute)type.GetCustomAttribute(typeof(CustomAttribute), true);
        Console.WriteLine(customAttribute.Desc);
    }

    MethodInfo method = type.GetMethod("GetName");
    //判断是否在方法上使用特性
    if (method.IsDefined(typeof(CustomAttribute), true))
    {
        CustomAttribute customAttribute = (CustomAttribute)method.GetCustomAttribute(typeof(CustomAttribute), true);
        Console.WriteLine(customAttribute.Desc);
    }
    Console.ReadKey();
}
```

### 特性的实际运用(数据校验)
- 定义一个抽象类，继承自Attribute
- 定义一个 LongAttribute
- 定义一个用户表 User ，标上LongAttribute特性
- 定义一个 Validate 的扩展类与扩展方法
- 实例化 User并校验数据
```C#
/* 定义一个抽象类，继承自Attribute */
public abstract class AbstractValidateAttribute : Attribute
{
    public abstract bool Validate(object oValue);
}
/* 定义一个 LongAttribute */
public class LongAttribute : AbstractValidateAttribute
{
    private int min { get; set; }
    private int max { get; set; }
    public LongAttribute(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
    public override bool Validate(object oValue)
    {
        return oValue != null && int.TryParse(oValue.ToString(), out int num) && num >= this.min && num <= this.max;
    }
}
/* 定义一个用户表 User ，标上LongAttribute特性 */
public class User
{
    public string Name { get; set; }
    [Long(1, 100)]
    public int Age
    {
        get; set;
    }
}
/* 定义一个 Validate 的扩展类与扩展方法 */
public static class ValidateExtension
{
    public static bool Validate(this object val)
    {
        Type type = val.GetType();
        foreach (var prop in type.GetProperties())
        {
            if (prop.IsDefined(typeof(LongAttribute), true))
            {
                LongAttribute longAttribute = (LongAttribute)prop.GetCustomAttribute(typeof(LongAttribute), true);
                if (!longAttribute.Validate(prop.GetValue(val)))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
/* 实例化 User并校验数据 */
static void Main(string[] args)
{
    User user1 = new User();
    user1.Age = -1;

    User user2 = new User();
    user2.Age = 23;
    Console.WriteLine(user1.Validate());

    Console.ReadKey();
}
```



- ......