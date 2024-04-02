using MediatR;

public class TestRequestHandler1 : IRequestHandler<TestRequest>
{
    public Task Handle(TestRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"TestEventHandler3被触发了,获取到参数：{request.UserName}");
        return Task.CompletedTask;
    }
}