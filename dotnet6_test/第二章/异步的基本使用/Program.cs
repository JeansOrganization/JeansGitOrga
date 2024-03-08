//基本用法
/*
Console.WriteLine("before write file");
await File.WriteAllTextAsync("d:/1.txt", "hello async");
Console.WriteLine("before read file");
string s = await File.ReadAllTextAsync("d:/1.txt");
Console.WriteLine(s);*/
//错误用法：没有用await调用异步方法

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

