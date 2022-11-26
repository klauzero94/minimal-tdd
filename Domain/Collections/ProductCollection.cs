using Kernel.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Collections;

[BsonCollection("products")]
public class ProductCollection : CollectionBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? Price { get; set; }
}