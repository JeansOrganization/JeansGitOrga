
Console.WriteLine("1-thread" + Thread.CurrentThread.ManagedThreadId);
var task1 = File.ReadAllTextAsync("d:/test/1.txt");
Console.WriteLine("2-thread" + Thread.CurrentThread.ManagedThreadId);
var task2 = File.ReadAllTextAsync("d:/test/2.txt");
Console.WriteLine("3-thread" + Thread.CurrentThread.ManagedThreadId);
var task3 = File.ReadAllTextAsync("d:/test/3.txt");
Console.WriteLine("4-thread" + Thread.CurrentThread.ManagedThreadId);
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