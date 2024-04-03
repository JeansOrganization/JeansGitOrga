using MediatR;

namespace EFCore中发布领域事件的合适时机.EventHandler
{
    public class ModifyUserLogHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly ILogger<ModifyUserLogHandler> logger;

        public ModifyUserLogHandler(ILogger<ModifyUserLogHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"ID[{notification.id}]用户已被更新");
            return Task.CompletedTask;
        }
    }
}
