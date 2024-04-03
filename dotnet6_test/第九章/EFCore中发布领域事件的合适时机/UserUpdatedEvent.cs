using MediatR;

namespace EFCore中发布领域事件的合适时机
{
    public record UserUpdatedEvent(Guid id) : INotification;
}
