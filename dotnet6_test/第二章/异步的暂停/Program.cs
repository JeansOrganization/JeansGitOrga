
using HttpClient httpClient = new HttpClient();
string h1 = await httpClient.GetStringAsync("http://www.baidu.com");
Console.WriteLine(h1);
Thread.Sleep(3000); //同步延迟:会阻塞进程
string h2 = await httpClient.GetStringAsync("https://www.ptpress.com.cn");
Console.WriteLine(h2);
await Task.Delay(3000); //异步延迟:不会阻塞进程
string h3 = await httpClient.GetStringAsync("https://www.rymooc.com");
Console.WriteLine(h3);
