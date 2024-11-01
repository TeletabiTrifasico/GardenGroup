using DAL;
using Model;
using MongoDB.Bson;

namespace Service;

public class TicketService
{
    private readonly TicketsDao _ticketsDao;
    public TicketService() => _ticketsDao = new TicketsDao();
    
    public List<Ticket> GetAllTickets() => _ticketsDao.GetAllTickets();
    
    public Ticket GetTicketById(ObjectId ticketId) => _ticketsDao.GetTicketById(ticketId);
    
    public void UpdateTicket(Ticket ticket) => _ticketsDao.UpdateTicket(ticket);
}