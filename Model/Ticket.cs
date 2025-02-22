﻿using System.ComponentModel;
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

    public enum Types
    {
        Hardware,
        Software,
        Network,
    }

    public ObjectId Id { get; set; }
    
    [BsonElement("date_reported")]
    public DateTime DateReported { get; set; }
    
    [BsonElement("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [BsonElement("incident_type")]
    [BsonRepresentation(BsonType.String)]
    public Types IncidentType;

    [BsonIgnore]
    public string IncidentTypeString => IncidentType.ToString();
    
    [BsonElement("assigned")]
    public ObjectId Assigned { get; set; }
    
    [BsonIgnore]
    public Employee? AssignedEmployee { get; set; }
    
    [BsonElement("priority")] 
    [BsonRepresentation(BsonType.String)]
    public Priorities Priority;
    
    [BsonIgnore]
    public string PriorityString => Priority.ToString();

    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
    
    [BsonElement("status")] 
    [BsonRepresentation(BsonType.String)]
    public Statuses Status;
    
    [BsonIgnore]
    public string StatusString => Status.ToString();
    
}