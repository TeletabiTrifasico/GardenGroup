using DAL;
using Model;
using MongoDB.Bson;

namespace Service;

public class TicketService
{
    private readonly TicketsDao _ticketsDao = new();

    public List<Ticket> GetAllTickets() => _ticketsDao.GetAllTickets();
    
    public Ticket? GetTicketById(ObjectId ticketId) => _ticketsDao.GetTicketById(ticketId);
    
    public void UpdateTicket(Ticket ticket) => _ticketsDao.UpdateTicket(ticket);
    
    public void DeleteTicket(Ticket ticket) => _ticketsDao.DeleteTicket(ticket);
}