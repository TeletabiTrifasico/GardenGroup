using Model;
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
                IncidentType = bsonDoc["IncidentType"].AsInt32,
                AssignedTo = bsonDoc["AssignedTo"].AsString,
                DateReported = bsonDoc["DateReported"].AsDateTime,
                Priority = (Ticket.Priorities)bsonDoc["Priority"].AsInt32,
                Status = (Ticket.Statuses)bsonDoc["Status"].AsInt32,
            });
        }

        return employeeTicketList;
    }
    public Dictionary<string, double> GetTicketStatusPercentage()
    {
        var collection = GetCollection<Ticket>("Tickets");
        var totalTickets = collection.CountDocuments(FilterDefinition<Ticket>.Empty);

        if (totalTickets == 0) return new Dictionary<string, double>();

        var statusCounts = collection.Aggregate()
            .Group(ticket => ticket.Status, g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToList();

        return statusCounts.ToDictionary(
            s => ((Ticket.Statuses)s.Status).ToString(),
            s => (s.Count / (double)totalTickets) * 100);
    }
    public int GetOverdueTaskCount()
    {
        var today = DateTime.Now;
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline < today && ticket.Status == Ticket.Statuses.Open);
    }

    public int GetTicketsDueTodayCount()
    {
        var today = DateTime.Now.Date;
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline == today && ticket.Status == Ticket.Statuses.Open);
    }

    public int GetOpenTicketCount()
    {
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Status == Ticket.Statuses.Open);
    }

    public int GetTicketsOnHoldCount()
    {
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Status == Ticket.Statuses.Closed);
    }

    public int GetAllTicketCount()
    {
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(FilterDefinition<Ticket>.Empty);
    }

    public int GetTicketsDueTomorrowCount()
    {
        var tomorrow = DateTime.Now.Date.AddDays(1);
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline == tomorrow && ticket.Status == Ticket.Statuses.Open);
    }

    public int GetTicketsDueThisMonthCount()
    {
        var today = DateTime.Now.Date;
        var endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline > today && ticket.Deadline <= endOfMonth && ticket.Status == Ticket.Statuses.Open);
    }

    public int GetTicketsDueMoreThanMonthCount()
    {
        var today = DateTime.Now.Date;
        var nextMonth = today.AddMonths(1);
        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline > nextMonth && ticket.Status == Ticket.Statuses.Open);
    }

    public Dictionary<string, double> GetUnresolvedTicketsByPriority()
    {
        var collection = GetCollection<Ticket>("Tickets");
        var totalUnresolvedTickets = collection.CountDocuments(ticket => ticket.Status == Ticket.Statuses.Open || ticket.Status == Ticket.Statuses.InProgress);

        if (totalUnresolvedTickets == 0) return new Dictionary<string, double>();

        var priorityCounts = collection.Aggregate()
            .Match(ticket => ticket.Status == Ticket.Statuses.Open || ticket.Status == Ticket.Statuses.InProgress)
            .Group(ticket => ticket.Priority, g => new
            {
                Priority = g.Key,
                Count = g.Count()
            })
            .ToList();

        return priorityCounts.ToDictionary(
            p => ((Ticket.Priorities)p.Priority).ToString(),
            p => (p.Count / (double)totalUnresolvedTickets) * 100);
    }
}