using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model;

public class PasswordReset(ObjectId userId, string pin)
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("user_id")]
    public ObjectId UserId { get; set; } = userId;

    [BsonElement("pin")]
    public string Pin { get; set; } = pin;
}