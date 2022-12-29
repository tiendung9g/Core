using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Entity.Entity;
using Core.Entity.IRepository;

namespace tiendung9.Core.Entity.Repository
{
    public class GenricRepository<TEntity, TModel> : IGenericRepository<TEntity, TModel> where TEntity : BaseEntity where TModel : BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenricRepository(IMapper mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task Add(TModel model)
        {
            await _dbSet.AddAsync(_mapper.Map<TEntity>(model));
        }

        public async Task AddRange(IEnumerable<TModel> models)
        {
            await _dbSet.AddRangeAsync(_mapper.Map<IEnumerable<TEntity>>(models));
        }

        public async Task BulkAdd(IEnumerable<TModel> models)
        {
            await _dbSet.BulkInsertAsync(_mapper.Map<IEnumerable<TEntity>>(models));
        }

        public async Task BulkDelete(IEnumerable<Guid> Ids)
        {
            Expression<Func<TEntity, bool>> express = x => (Ids.Contains(x.Id));
            var items = await _dbSet.Where(express).ToListAsync();
            await _dbSet.BulkDeleteAsync(items);
        }

        public async Task BulkUpdate(IEnumerable<TModel> models)
        {
            await _dbSet.BulkUpdateAsync(_mapper.Map<IEnumerable<TEntity>>(models));
        }

        public async Task Delete(Guid Id)
        {
            _dbSet.Remove(_mapper.Map<TEntity>(await _dbSet.FindAsync(Id)));
        }

        public async Task DeleteRange(IEnumerable<Guid> Ids)
        {
            Expression<Func<TEntity, bool>> express = x => (Ids.Contains(x.Id));
            var items = await _dbSet.Where(express).ToListAsync();
            if (items.Count > 0)
            {
                _dbSet.RemoveRange(items);
            }
        }

        public async Task<bool> Exist(Guid Id)
        {
            return await _dbSet.AnyAsync(x => x.Id == Id);
        }

        public async Task<bool> ExistRange(IEnumerable<Guid> Ids)
        {
            return await _dbSet.AnyAsync(x => Ids.Contains(x.Id));
        }

        public async Task<IEnumerable<TModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<TModel>>(await _dbSet.AsNoTracking().ToListAsync());
        }

        public async Task<TModel> GetById(Guid Id)
        {
            return _mapper.Map<TModel>(await _dbSet.FindAsync(Id));
        }

        public async Task<IQueryable<TModel>> Paging(Expression<Func<TModel, bool>> expression, int page, int size)
        {
            var expressionMap = _mapper.Map<Expression<Func<TEntity, bool>>>(expression);
            return _mapper.Map<IQueryable<TModel>>(await _dbSet.Where(expressionMap).AsNoTracking().Skip(page * size).Take(size).ToListAsync());
        }

        public async Task<IQueryable<TModel>> Query(Expression<Func<TModel, bool>> expression)
        {
            var expressionMap = _mapper.Map<Expression<Func<TEntity, bool>>>(expression);
            return _mapper.Map<IQueryable<TModel>>(await _dbSet.Where(expressionMap).AsNoTracking().ToListAsync());
        }
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(TModel model)
        {
            _dbSet.Update(_mapper.Map<TEntity>(model));
        }

        public async Task UpdateRange(IEnumerable<TModel> models)
        {
            _dbSet.UpdateRange(_mapper.Map<IEnumerable<TEntity>>(models));
        }
    }
}