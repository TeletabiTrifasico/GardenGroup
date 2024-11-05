using Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DAL;

public class TicketsDao : BaseDao
{
    private Ticket PopulateAssignedEmployee(BsonDocument ticketDoc)
    {
        var employeesCollection = GetCollection<BsonDocument>("Employees");
        var ticket = BsonSerializer.Deserialize<Ticket>(ticketDoc);
        
        var employeeDoc = employeesCollection.Find(e => e["_id"] == ticket.Assigned).FirstOrDefault();
        if (employeeDoc != null)
            ticket.AssignedEmployee = BsonSerializer.Deserialize<Employee>(employeeDoc);
    
        return ticket;
    }
    
    public List<Ticket> GetAllTickets()
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");

        return ticketsCollection.Find(new BsonDocument())
            .ToList()
            .Select(PopulateAssignedEmployee)
            .ToList();
    }

    public Ticket? GetTicketById(ObjectId id)
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");

        return ticketsCollection.Find(new BsonDocument())
            .ToList()
            .Select(PopulateAssignedEmployee)
            .ToList().Find(x => x.Id.Equals(id));
    }

    public void UpdateTicket(Ticket ticket)
    {
        var filter = FilterEq<Ticket, ObjectId>("Id", ticket.Id);
        var updateDefinition = Builders<Ticket>.Update
            .Set(x => x.Status, ticket.Status)
            .Set(x => x.Priority, ticket.Priority)
            .Set(x => x.Description, ticket.Description);

        var result = GetCollection<Ticket>("Tickets").UpdateOne(filter, updateDefinition);

        if (result.ModifiedCount == 0)
            throw new Exception($"Failed to update ticket.");
    }
    
    public void UpdateTicketDynamic(Ticket ticket)
    {
        var filter = FilterEq<Ticket, ObjectId>("Id", ticket.Id);
        
        var updates = new List<UpdateDefinition<Ticket>>();
        
        foreach (var property in typeof(Ticket).GetProperties())
        {
            if (property.Name == "Id")
                continue;
            
            var value = property.GetValue(ticket);
            var currentUpdate = Builders<Ticket>.Update.Set(property.Name, value);
            updates.Add(currentUpdate);
        }
        
        var updateDefinition = Builders<Ticket>.Update.Combine(updates);
        
        var result = GetCollection<Ticket>("Tickets").UpdateOne(filter, updateDefinition);
        
        if(result.ModifiedCount == 0)
            throw new Exception("Failed to update ticket.");
    }
    
    public void DeleteTicket(Ticket ticket)
    {
        var filter = FilterEq<Ticket, ObjectId>("Id", ticket.Id);
        var result = GetCollection<Ticket>("Tickets").DeleteOne(filter);
        
        if(result.DeletedCount == 0)
            throw new Exception("Failed to delete ticket.");
    }
    public int GetCountByStatus(int status)
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var filter = Builders<BsonDocument>.Filter.Eq("status", status);
        return (int)ticketsCollection.CountDocuments(filter);
    }

    public int GetCountByPriority(int priority)
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var filter = Builders<BsonDocument>.Filter.Eq("priority", priority);
        return (int)ticketsCollection.CountDocuments(filter);
    }

    public int GetOverdueTicketsCount()
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var filter = Builders<BsonDocument>.Filter.Lt("deadline", DateTime.UtcNow);
        return (int)ticketsCollection.CountDocuments(filter);
    }

    public int GetTicketsDueTodayCount()
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var today = DateTime.UtcNow.Date;
        var filter = Builders<BsonDocument>.Filter.Gte("deadline", today) &
                     Builders<BsonDocument>.Filter.Lt("deadline", today.AddDays(1));
        return (int)ticketsCollection.CountDocuments(filter);
    }

    public int GetTicketsDueTomorrowCount()
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var tomorrow = DateTime.UtcNow.Date.AddDays(1);
        var filter = Builders<BsonDocument>.Filter.Gte("deadline", tomorrow) &
                     Builders<BsonDocument>.Filter.Lt("deadline", tomorrow.AddDays(1));
        return (int)ticketsCollection.CountDocuments(filter);
    }

    public int GetTicketsDueThisMonthCount()
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var today = DateTime.UtcNow;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var startOfNextMonth = startOfMonth.AddMonths(1);
        var filter = Builders<BsonDocument>.Filter.Gte("deadline", startOfMonth) &
                     Builders<BsonDocument>.Filter.Lt("deadline", startOfNextMonth);
        return (int)ticketsCollection.CountDocuments(filter);
    }

    public int GetTicketsDueMoreThanMonthCount()
    {
        var ticketsCollection = GetCollection<BsonDocument>("Tickets");
        var today = DateTime.UtcNow;
        var startOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
        var filter = Builders<BsonDocument>.Filter.Gte("deadline", startOfNextMonth);
        return (int)ticketsCollection.CountDocuments(filter);
    }
    public void InsertTicket(Ticket ticket)
    {
        var ticketsCollection = GetCollection<Ticket>("Tickets");
        ticketsCollection.InsertOne(ticket);
    }
    
    public List<Ticket> GetTicketsByEmployeeId(ObjectId employeeId)
    {
        var filter = Builders<Ticket>.Filter.Eq(t => t.Assigned, employeeId);
        return GetCollection<Ticket>("Tickets").Find(filter).ToList();
    }

}
