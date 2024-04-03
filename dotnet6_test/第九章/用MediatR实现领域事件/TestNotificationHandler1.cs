using MediatR;

public class TestNotificationHandler1 : INotificationHandler<TestNotification>
{
    public Task Handle(TestNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"TestEventHandler1被触发了,获取到参数：{notification.UserName}");
        return Task.CompletedTask;
    }
}