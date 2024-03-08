
public class TestServiceUSImpl : ITestService
{
    public string Name { get; set; }

    public void SayHi()
    {
        Console.WriteLine($"Hi,{Name}");
    }
}