using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entity.Entity
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
    }
}
