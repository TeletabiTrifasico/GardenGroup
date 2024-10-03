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
        Open,
        InProgress,
        Closed,
    }

    public enum Types
    {
        Hardware,
        Software,
        Network,
    }
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("date_reported")]
    public DateTime DateReported { get; set; }
    
    [BsonElement("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [BsonElement("incident_type")] public Types IncidentType;
    
    [BsonElement("assigned")]
    public ObjectId Assigned { get; set; }
    
    [BsonElement("priority")] public Priorities Priority;
    
    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
    
    [BsonElement("status")] public Statuses Status;
}