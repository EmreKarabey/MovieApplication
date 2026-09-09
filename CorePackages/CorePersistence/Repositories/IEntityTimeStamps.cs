namespace CorePersistence.Repositories
{
    public interface IEntityTimeStamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}