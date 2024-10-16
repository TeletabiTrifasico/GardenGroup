using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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
                { "_id", 1 },                            // Exclude the original _id
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
                Id = bsonDoc["_id"].AsObjectId,
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

    public Ticket GetTicketByIdAsync(ObjectId id)
    {
        var ticket = GetCollection<Ticket>("Tickets").AsQueryable().ToList().Find(x => x.Id.Equals(id));
        return ticket;
    }

    public void UpdateTicket(Ticket ticket)
    {
        var filter = Builders<Ticket>.Filter.Eq(x => x.Id, ticket.Id);
        var update = Builders<Ticket>.Update
            .Set(x => x.Status, ticket.Status)
            .Set(x => x.Priority, ticket.Priority);

        var updateResult = _db.GetCollection<Ticket>("Tickets").UpdateOne(filter, update);
        
        if (updateResult.ModifiedCount == 0)
        {
            Console.WriteLine("No documents were updated.");
        }

        //var update = GetCollection<Ticket>("Tickets").UpdateOne(x => x.Id == , filter);
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
        var todayStart = DateTime.Now.Date; // Start of today (00:00:00)
        var todayEnd = todayStart.AddDays(1).AddTicks(-1); // End of today (23:59:59)

        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline >= todayStart && ticket.Deadline <= todayEnd && ticket.Status == Ticket.Statuses.Open);
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
        var tomorrowStart = DateTime.Now.Date.AddDays(1);
        var tomorrowEnd = tomorrowStart.AddDays(1).AddTicks(-1); // End of tomorrow (23:59:59)

        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline >= tomorrowStart && ticket.Deadline <= tomorrowEnd && ticket.Status == Ticket.Statuses.Open);
    }

    public int GetTicketsDueThisMonthCount()
    {
        var today = DateTime.Now.Date;
        var endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)).AddTicks(-1); // End of the month (23:59:59)

        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline >= today && ticket.Deadline <= endOfMonth && ticket.Status == Ticket.Statuses.Open);
    }

    public int GetTicketsDueMoreThanMonthCount()
    {
        var nextMonthStart = DateTime.Now.Date.AddMonths(1);
        var nextMonthEnd = nextMonthStart.AddMonths(1).AddTicks(-1); // End of next month

        var collection = GetCollection<Ticket>("Tickets");
        return (int)collection.CountDocuments(ticket => ticket.Deadline > nextMonthEnd && ticket.Status == Ticket.Statuses.Open);
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