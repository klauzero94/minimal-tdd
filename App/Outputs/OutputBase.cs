using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Outputs;

[BsonIgnoreExtraElements]
public class OutputBase
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}