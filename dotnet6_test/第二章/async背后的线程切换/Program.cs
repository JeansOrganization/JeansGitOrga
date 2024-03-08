
Console.WriteLine("1-thread" + Thread.CurrentThread.ManagedThreadId);
string str = new string('a', 100000);
Console.WriteLine("2-thread" + Thread.CurrentThread.ManagedThreadId);
await File.WriteAllTextAsync("d:/1.txt", str);
Console.WriteLine("3-thread" + Thread.CurrentThread.ManagedThreadId);
await File.WriteAllTextAsync("d:/1.txt", str);
Console.WriteLine("4-thread" + Thread.CurrentThread.ManagedThreadId);
File.WriteAllText("d:/1.txt", str);
Console.WriteLine("5-thread" + Thread.CurrentThread.ManagedThreadId);
