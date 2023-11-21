using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DataModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public bool IsCritical { get; set; }

    public int Value { get; set; }

    public string Type { get; set; }
}