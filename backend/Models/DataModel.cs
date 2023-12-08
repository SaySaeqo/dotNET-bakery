using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DataModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId dataId { get; set; }

    public required int id { get; set; }

    public required DateTime date { get; set; }

    public required int value { get; set; }

    public required string type { get; set; }
}