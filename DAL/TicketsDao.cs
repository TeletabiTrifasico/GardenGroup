using  Model;
using MongoDB.Driver;
using DAL.Extensions;


namespace DAL;



public class TicketsDao : MongoCRUD
{
    public List<Ticket> GetTicketsForUser(Guid userId)
    {
        var tickets = GetCollection<Ticket>("Tickets").Find(t => t.Assigned == userId).ToList();
        return tickets;
    }

    public void AddTicket(Ticket ticket)
    {
        InsertRecord<Ticket>("Tickets", ticket);
    }

    public void UpdateTicket(Ticket ticket)
    {
        UpdateRecord<Ticket>("Tickets", ticket.Id, ticket);
    }
    public void DeleteTicket(Guid ticketId)
    {
        DeleteRecord<Ticket>("Tickets", ticketId);
    }

     
}