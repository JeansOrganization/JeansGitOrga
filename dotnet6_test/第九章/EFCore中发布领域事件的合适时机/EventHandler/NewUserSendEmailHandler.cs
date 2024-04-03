using MediatR;

namespace EFCore中发布领域事件的合适时机.EventHandler
{
    public class NewUserSendEmailHandler : INotificationHandler<UserAddedEvent>
    {
        private readonly ILogger<NewUserSendEmailHandler> logger;

        public NewUserSendEmailHandler(ILogger<NewUserSendEmailHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(UserAddedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.user;
            logger.LogInformation($"新增了用户[姓名:{user.UserName},ID:{user.Id},昵称:{user.NickName}]");
            return Task.CompletedTask;
        }
    }
}
