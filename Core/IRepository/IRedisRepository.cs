using Core.Entity.Entity;

namespace Core.Entity.IRepository
{
    public interface IRedisRepository<TEntity, TModel> where TEntity : BaseEntity where TModel : BaseEntity
    {
        Task<IEnumerable<TModel>> GetAsyncs(Guid id);
        Task<TModel> GetAsync(Guid id);
        Task AddAsync(TModel entity);
        Task DeleteAsync(Guid id);
        Task RefreshAsync(Guid id);
    }
}
