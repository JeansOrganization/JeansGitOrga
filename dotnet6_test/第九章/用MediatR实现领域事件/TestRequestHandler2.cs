using MediatR;

public class TestRequestHandler2 : IRequestHandler<TestRequest>
{
    public Task Handle(TestRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"TestEventHandler4被触发了,获取到参数：{request.UserName}");
        return Task.CompletedTask;
    }
}