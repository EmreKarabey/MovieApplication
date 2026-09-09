using CorePersistence.Repositories;

namespace Domain.Entities
{
    public class Category : Entity<Guid>
    {
        public string CategoryName { get; set; }

        public virtual ICollection<MoviesCategory> MoviesCategories { get; set; }


        public Category() { }

        public Category(Guid Id, string categoryName) : this()
        {
            EntityID = Id;
            CategoryName = categoryName;
        }
    }
}