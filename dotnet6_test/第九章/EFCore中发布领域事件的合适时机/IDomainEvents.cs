using MediatR;

namespace EFCore中发布领域事件的合适时机
{
    public interface IDomainEvents
    {
        IEnumerable<INotification> GetDomainEvents();
        void AddDomainEvent(INotification eventItem);
        void AddDomainEventIfAbsent(INotification eventItem);
        void ClearDomainEvents();
    }
}
