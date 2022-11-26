using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Collections;

public class CollectionBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt => Id.CreationTime;
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
}