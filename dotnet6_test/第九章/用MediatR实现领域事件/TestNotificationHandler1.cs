using MediatR;

public class TestNotificationHandler1 : INotificationHandler<TestEvent>
{
    public Task Handle(TestEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"TestEventHandler1被触发了,获取到参数：{notification.UserName}");
        return Task.CompletedTask;
    }
}