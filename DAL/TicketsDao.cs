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
        var update = Builders<Ticket>.Update
            .Set(x => x.Status, ticket.Status)
            .Set(x => x.Priority, ticket.Priority);

        var result = GetCollection<Ticket>("Tickets").UpdateOne(filter, update);

        if (result.ModifiedCount == 0)
            throw new Exception($"Failed to update ticket.");
    }

    public void DeleteTicket(Ticket ticket)
    {
        var filter = FilterEq<Ticket, ObjectId>("Id", ticket.Id);
        var result = GetCollection<Ticket>("Tickets").DeleteOne(filter);
        
        if(result.DeletedCount == 0)
            throw new Exception("Failed to delete ticket.");
    }
}