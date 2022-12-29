using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using Core.Entity.Constant;
using Core.Entity.Entity;
using Core.Entity.IRepository;

namespace Core.Entity.Repository
{
    public class MongoRepository<TDocument, TModel> : IMongoRepository<TDocument, TModel> where TDocument : IDocument where TModel : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly IMapper _mapper;

        public MongoRepository(IMapper mapper, IMongoDbSettings settings)
        {
            _mapper = mapper;
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }
        private protected string? GetCollectionName(Type documentType)
        {
            return (documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }

        public async Task<TModel> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _mapper.Map<TModel>(await _collection.FindAsync(filterExpression));
        }

        public async Task<TModel> FindByIdAsync(ObjectId id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return _mapper.Map<TModel>(await _collection.FindAsync(filter));
        }

        public async Task InsertOneAsync(TModel document)
        {
            await _collection.InsertOneAsync(_mapper.Map<TDocument>(document));
        }
        public async Task InsertManyAsync(ICollection<TModel> documents)
        {
            await _collection.InsertManyAsync(_mapper.Map<ICollection<TDocument>>(documents));
        }

        public async Task ReplaceOneAsync(TModel document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, _mapper.Map<TDocument>(document));
        }
        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }
        public async Task DeleteByIdAsync(ObjectId id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            await _collection.FindOneAndDeleteAsync(filter);
        }
        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.DeleteManyAsync(filterExpression);
        }
    }
}
