using MongoDB.Bson;
using System.Linq.Expressions;
using Core.Entity.Entity;

namespace Core.Entity.IRepository
{
    public interface IMongoRepository<TDocument, TModel> where TDocument : IDocument where TModel : IDocument
    {
        Task<TModel> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<TModel> FindByIdAsync(ObjectId id);

        Task InsertOneAsync(TModel document);

        Task InsertManyAsync(ICollection<TModel> documents);

        Task ReplaceOneAsync(TModel document);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteByIdAsync(ObjectId id);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
