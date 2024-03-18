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


# 简书地址:https://www.jianshu.com/p/9f09fe043564