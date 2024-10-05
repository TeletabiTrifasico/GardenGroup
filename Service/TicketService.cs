using DAL;
using Model;

namespace Service;

public class TicketService
{
    private readonly TicketsDao _ticketsDao;
    public TicketService() => _ticketsDao = new TicketsDao();
    
    public List<Ticket> GetAllTickets() => _ticketsDao.GetAllTickets();

    public List<EmployeeTicket> GetAllEmployeesTicketsAsync() => _ticketsDao.GetEmployeesTickets();
}