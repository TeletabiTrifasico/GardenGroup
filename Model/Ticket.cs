using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model;

[DataObject]
public class Ticket
{
    public enum Priorities
    {
        Low,
        Normal,
        High,
    }
    public enum Statuses
    {
        Unresolved,
        Solved,
    }
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("date_reported")]
    public DateTime DateReported { get; set; }
    
    [BsonElement("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [BsonElement("incident_type")]
    public string IncidentType { get; set; } = string.Empty;
    
    [BsonElement("assigned")]
    public ObjectId Assigned { get; set; }
    
    // Enum? Yes
    [BsonElement("priority")] public Priorities Priority;
    
    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    // Emum? Yes
    [BsonElement("status")] private Statuses Status;
}