using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore中发布领域事件的合适时机
{
    public class User : BaseEntity
    {
        public Guid Id { get; init; }
        public string UserName { get; init; }
        public string Email { get; private set; }
        public string? NickName { get; private set; }
        public int? Age { get; private set; }
        public bool IsDeleted { get; private set; }
        private User()
        {
        }
        public User(string userName, string email)
        {
            this.Id = Guid.NewGuid();
            this.UserName = userName;
            this.Email = email;
            this.IsDeleted = false;
            AddDomainEvent(new UserAddedEvent(this));
        }

        public void ChangeEmail(string newValue)
        {
            this.Email = newValue;
            AddDomainEventIfAbsent(new UserUpdatedEvent(this.Id));
        }

        public void ChangeNickName(string newValue)
        {
            this.NickName = newValue;
            AddDomainEventIfAbsent(new UserUpdatedEvent(this.Id));
        }

        public void ChangeAge(int newValue)
        {
            this.Age = newValue;
            AddDomainEventIfAbsent(new UserUpdatedEvent(this.Id));
        }

        //软删除 SoftDelete
        public void SoftDelete()
        {
            this.IsDeleted = true;
            AddDomainEventIfAbsent(new UserSoftDeletedEvent(this.Id));
        }
    }

}
