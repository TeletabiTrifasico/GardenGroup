using Model;
using MongoDB.Driver;
using DAL;

namespace Service;

public class TicketService
{

    private readonly TicketsDao _ticketsDao;
    public TicketService()
    {
        _ticketsDao = new TicketsDao();
    }

    public List<Ticket> GetTickets(Guid userId)
    {
        return _ticketsDao.GetTicketsForUser(userId);
    }

    public void AddTicket(Ticket ticket)
    {
        _ticketsDao.AddTicket(ticket);
    }

    public void UpdateTicket(Ticket ticket)
    {
        _ticketsDao.UpdateTicket(ticket);
    }

    public void DeleteTicket(Guid ticketId)
    {
        _ticketsDao.DeleteTicket(ticketId);
    }

    public void CloseTicket(Ticket ticket)
    {
         Ticket newTicket = ticket.CopyWith(status: "Closed");
        _ticketsDao.UpdateTicket(newTicket);
    }
    
}