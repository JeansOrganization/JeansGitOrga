using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EFCore中发布领域事件的合适时机
{
    public class UserDbContext : BaseDbContext
    {
        public DbSet<User> Users { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options, IMediator mediator) : base(options,mediator)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }


    }
}
