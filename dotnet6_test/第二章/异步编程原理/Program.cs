using HttpClient httpClient = new HttpClient();
string html = await httpClient.GetStringAsync("http://www.baidu.com");
Console.WriteLine(html);
string destFilePath = "d:/test/1.txt";
string content = "hello async and await";
await File.WriteAllTextAsync(destFilePath, content);
string content2 = await File.ReadAllTextAsync(destFilePath);
Console.WriteLine(content2);


