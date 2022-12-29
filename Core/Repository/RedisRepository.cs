using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using Core.Entity.Entity;
using Core.Entity.IRepository;

namespace Core.Entity.Repository
{
    public class RedisRepository<TEntity, TModel> : IRedisRepository<TEntity, TModel> where TEntity : BaseEntity where TModel : BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private DistributedCacheEntryOptions option;
        public RedisRepository(IMapper mapper, IDistributedCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }

        public async Task AddAsync(TModel entity)
        {
            await _cache.SetAsync(entity.Id.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity)), option.SetSlidingExpiration(TimeSpan.FromMinutes(5)));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _cache.RemoveAsync(id.ToString());
        }

        public async Task<TModel> GetAsync(Guid id)
        {
            return _mapper.Map<TModel>(await _cache.GetAsync(id.ToString()));
        }

        public async Task<IEnumerable<TModel>> GetAsyncs(Guid id)
        {
            return _mapper.Map<IEnumerable<TModel>>(await _cache.GetAsync(id.ToString()));
        }

        public async Task RefreshAsync(Guid id)
        {
            await _cache.RefreshAsync(id.ToString());
        }
    }
}
