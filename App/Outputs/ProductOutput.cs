using MongoDB.Bson.Serialization.Attributes;

namespace App.Outputs;

[BsonIgnoreExtraElements]
public class ProductOutput : OutputBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
}