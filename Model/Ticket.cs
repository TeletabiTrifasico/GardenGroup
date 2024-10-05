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
        Resolved,
        Closed,
    }

    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("date_reported")]
    public DateTime DateReported { get; set; }
    
    [BsonElement("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [BsonElement("incident_type")]
    public int IncidentType { get; set; }
    
    [BsonElement("assigned")]
    public Guid Assigned { get; set; }
    
    [BsonElement("priority")] 
    [BsonRepresentation(BsonType.String)]
    public Priorities Priority;
    
    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
    
    [BsonElement("status")] 
    [BsonRepresentation(BsonType.String)]
    private Statuses Status;
}