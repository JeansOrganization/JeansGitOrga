using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EFCore中发布领域事件的合适时机
{
    public class BaseDbContext : DbContext
    {
        private readonly IMediator mediator;
        public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            this.mediator = mediator;
        }

        public override int SaveChanges()
        {
            throw new NotImplementedException("Don not call SaveChanges, please call SaveChangesAsync instead.");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            if (result == 0) return result;
            var entities = this.ChangeTracker.Entries<IDomainEvents>()
                .Where(r => r.Entity.GetDomainEvents().Any()).ToList();
            var domainEvents = entities.SelectMany(r => r.Entity.GetDomainEvents()).ToList();
            entities.ForEach(r => r.Entity.ClearDomainEvents());
            foreach (var e in domainEvents)
            {
                await mediator.Publish(e);
            }
            return result;
        }
    }
}
