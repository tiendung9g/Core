using System.Linq.Expressions;
using Core.Entity.Entity;

namespace Core.Entity.IRepository
{
    public interface IGenericRepository<TEntity, TModel> where TEntity : BaseEntity where TModel : BaseEntity
    {
        public Task Add(TModel model);
        public Task Update(TModel model);
        public Task Delete(Guid Id);
        public Task<TModel> GetById(Guid Id);
        public Task<IQueryable<TModel>> Query(Expression<Func<TModel, bool>> expression);
        public Task<IEnumerable<TModel>> GetAll();
        public Task AddRange(IEnumerable<TModel> models);
        public Task UpdateRange(IEnumerable<TModel> models);
        public Task DeleteRange(IEnumerable<Guid> Ids);
        public Task SaveChange();
        public Task<IQueryable<TModel>> Paging(Expression<Func<TModel, bool>> expression, int page, int size);
        public Task<bool> Exist(Guid Id);
        public Task<bool> ExistRange(IEnumerable<Guid> Ids);
        public Task BulkAdd(IEnumerable<TModel> models);
        public Task BulkUpdate(IEnumerable<TModel> models);
        public Task BulkDelete(IEnumerable<Guid> Ids);
    }
}