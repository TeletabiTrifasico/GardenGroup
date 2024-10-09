﻿using Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class TicketsDao : MongoCRUD
{
    public List<Ticket> GetAllTickets() =>
        GetCollection<Ticket>("Tickets")
            .AsQueryable()
            .ToList();

    [Obsolete("Obsolete")]
    public List<EmployeeTicket> GetEmployeesTickets()
    {
        // Define the collections
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var employeesCollection = GetCollection<BsonDocument>("Employees");

        // Define the aggregation pipeline
        var pipeline = new[]
        {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Employees" },                   // Join with Employees collection
                { "localField", "assigned" },               // Field from Tickets collection
                { "foreignField", "_id" },                  // Field from Employees collection
                { "as", "employeeInfo" }                    // Output array field containing matched employees
            }),
            new BsonDocument("$unwind", "$employeeInfo"),  // Unwind the array to flatten the structure
            new BsonDocument("$project", new BsonDocument // Project the required fields
            {
                { "_id", 0 },                            // Exclude the original _id
                { "FullName", new BsonDocument("$concat", new BsonArray { "$employeeInfo.first_name", " ", "$employeeInfo.last_name" }) },
                { "Subject", "$subject" },
                { "IncidentType", "$incident_type" },
                { "AssignedTo", "$employeeInfo.username" },
                { "DateReported", "$date_reported" },
                { "Priority", "$priority" },
                { "Status", "$status" }
            })
        };

        // Execute the aggregation pipeline
        var bsonResults = ticketsCollection.Aggregate<BsonDocument>(pipeline).ToList();

        // Convert the BSON documents to EmployeeTicket objects
        var employeeTicketList = new List<EmployeeTicket>();

        foreach (var bsonDoc in bsonResults)
        {
            employeeTicketList.Add(new EmployeeTicket
            {
                FullName = bsonDoc["FullName"].AsString,
                Subject = bsonDoc["Subject"].AsString,
                IncidentType = (Ticket.Types)bsonDoc["IncidentType"].AsInt32,
                AssignedTo = bsonDoc["AssignedTo"].AsString,
                DateReported = bsonDoc["DateReported"].AsDateTime,
                Priority = (Ticket.Priorities)bsonDoc["Priority"].AsInt32,
                Status = (Ticket.Statuses)bsonDoc["Status"].AsInt32,
            });
        }

        return employeeTicketList;
    }
}