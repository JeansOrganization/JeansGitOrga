
Console.WriteLine("1-Main:" + Thread.CurrentThread.ManagedThreadId);
Console.WriteLine(await CalcAsync(10000));
Console.WriteLine("2-Main:" + Thread.CurrentThread.ManagedThreadId);

//async Task<decimal> CalcAsync(int n)
//{
//    Console.WriteLine("CalcAsync:" + Thread.CurrentThread.ManagedThreadId);
//    decimal result = 1;
//    Random rd = new Random();
//    for (int i = 0; i < n; i++)
//    {
//        result += (decimal)rd.NextDouble();
//    }
//    return result;
//}

async Task<decimal> CalcAsync(int n)
{
    Console.WriteLine("CalcAsync:" + Thread.CurrentThread.ManagedThreadId);
    return await Task.Run(() =>
    {
        Console.WriteLine("Task.Run:" + Thread.CurrentThread.ManagedThreadId);
        decimal result = 1;
        Random rd = new Random();
        for (int i = 0; i < n; i++)
        {
            result += (decimal)rd.NextDouble();
        }
        return result;
    });
}