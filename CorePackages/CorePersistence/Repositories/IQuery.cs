namespace CorePersistence.Repositories
{
    public interface IQuery<TEntity>
    {
        IQueryable<TEntity> Query();
    }
}
