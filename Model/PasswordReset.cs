using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model;

public class PasswordReset
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("user_id")]
    public ObjectId UserId { get; set; }
    
    [BsonElement("pin")]
    public string Pin { get; set; }

    public PasswordReset(ObjectId userId, string pin)
    {
        UserId = userId;
        Pin = pin;
    }
}