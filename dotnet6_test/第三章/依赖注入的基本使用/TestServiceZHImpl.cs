
public class TestServiceZHImpl : ITestService
{
    public string Name { get; set; }

    public void SayHi()
    {
        Console.WriteLine($"你好呀！{Name}");
    }
}