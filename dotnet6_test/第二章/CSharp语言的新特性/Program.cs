
using var fileStream = File.OpenWrite("C:/Users/JeanG/Desktop/12.txt");
using var writter = new StreamWriter(fileStream);
writter.WriteLine("Hello World !!!");
writter.WriteLine("Jean");
writter.Dispose();


string s = File.ReadAllText("C:/Users/JeanG/Desktop/12.txt");
Console.WriteLine(s);
Console.ReadLine();