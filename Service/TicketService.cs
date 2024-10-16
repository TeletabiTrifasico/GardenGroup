using DAL;
using Model;
using MongoDB.Bson;

namespace Service;

public class TicketService
{
    private readonly TicketsDao _ticketsDao;
    public TicketService() => _ticketsDao = new TicketsDao();
    
    public List<Ticket> GetAllTickets() => _ticketsDao.GetAllTickets();

    public List<EmployeeTicket> GetAllEmployeesTicketsAsync() => _ticketsDao.GetEmployeesTickets();
    
    public Ticket GetTicketById(ObjectId ticketId) => _ticketsDao.GetTicketByIdAsync(ticketId);
    
    public void UpdateTicket(Ticket ticket) => _ticketsDao.UpdateTicket(ticket);
}