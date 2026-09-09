using CorePersistence.Repositories;

namespace Domain.Entities
{
    public class ForumCategory : Entity<Guid>
    {
        public string Name { get; set; }

        public ICollection<Forum> Forums { get; set; }
    }
}
