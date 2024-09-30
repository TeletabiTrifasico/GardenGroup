using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model;

[DataObject]
public class Ticket
{
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
    
    // Enum?
    [BsonElement("priority")]
    public string Priority { get; set; } = string.Empty;
    
    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    // Emum?
    [BsonElement("status")]
    private string Status { get; set; } = string.Empty;
}