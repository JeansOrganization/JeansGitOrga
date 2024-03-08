
while (true)
{
    string input = Console.ReadLine()!;
    if(int.TryParse(input,out int num))
    {
        string content = await ReadFileAsync2(num);
        Console.WriteLine(content);
    }
    else
    {
        Console.WriteLine("输入格式错误");
    }
}


async Task<string> ReadFileAsync1(int num)
{
    switch (num)
    {
        case 0:
            return await File.ReadAllTextAsync("d:/test/a.txt"); ;
        case 1:
            return await File.ReadAllTextAsync("d:/test/b.txt");
        default:
            return "没有此选项";
    }
}

Task<string> ReadFileAsync2(int num)
{
    switch (num)
    {
        /* 避免了拆箱之后再装箱，直接将Task<string>return出去，由调用方进行await获取数据 */
        case 0:
            return File.ReadAllTextAsync("d:/test/a.txt");
        case 1:
            return File.ReadAllTextAsync("d:/test/b.txt");
        default:
            //手动创建Task对象
            return Task.FromResult("没有此选项");
    }
}


Task ReadFileAsync3(int num)
{
    switch (num)
    {
        case 0:
            return File.ReadAllTextAsync("d:/test/a.txt");
        case 1:
            return File.ReadAllTextAsync("d:/test/b.txt");
        default:
            Console.WriteLine("没有此选项");
            return Task.CompletedTask;
    }
}