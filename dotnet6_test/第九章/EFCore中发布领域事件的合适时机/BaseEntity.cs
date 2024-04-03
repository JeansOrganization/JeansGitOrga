using MediatR;

namespace EFCore中发布领域事件的合适时机
{
    public class BaseEntity : IDomainEvents
    {
        private List<INotification> EventList = new List<INotification>();
        public void AddDomainEvent(INotification eventItem)
        {
            this.EventList.Add(eventItem);
        }

        public void AddDomainEventIfAbsent(INotification eventItem)
        {
            if (!EventList.Contains(eventItem))
            {
                this.EventList.Add(eventItem);
            }
        }

        public void ClearDomainEvents()
        {
            this.EventList.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return this.EventList;
        }
    }
}
