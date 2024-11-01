using Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DAL;

public class TicketsDao : MongoCRUD
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
    }
}