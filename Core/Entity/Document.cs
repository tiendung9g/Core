using MongoDB.Bson;

namespace Core.Entity.Entity
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
    }
}
