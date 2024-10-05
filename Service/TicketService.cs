using DAL;
using Model;

namespace Service;

public class TicketService
{
    private readonly TicketsDao _ticketsDao;
    public TicketService() => _ticketsDao = new TicketsDao();
    
    public List<Ticket> GetAllTickets() => _ticketsDao.GetAllTickets();

    public List<EmployeeTicket> GetAllEmployeesTicketsAsync() => _ticketsDao.GetEmployeesTickets();
   
    // Fetch ticket status percentages for progress bars
    public Dictionary<string, double> GetTicketStatusPercentage()
    {
        return _ticketsDao.GetTicketStatusPercentage();
    }

    // Fetch overdue task count
    public int GetOverdueTaskCount()
    {
        return _ticketsDao.GetOverdueTaskCount();
    }

    // Fetch tickets due today
    public int GetTicketsDueTodayCount()
    {
        return _ticketsDao.GetTicketsDueTodayCount();
    }

    // Fetch open ticket count
    public int GetOpenTicketCount()
    {
        return _ticketsDao.GetOpenTicketCount();
    }

    // Fetch tickets on hold count
    public int GetTicketsOnHoldCount()
    {
        return _ticketsDao.GetTicketsOnHoldCount();
    }

    // Fetch all tickets count
    public int GetAllTicketCount()
    {
        return _ticketsDao.GetAllTicketCount();
    }

    // Fetch tickets due tomorrow count
    public int GetTicketsDueTomorrowCount()
    {
        return _ticketsDao.GetTicketsDueTomorrowCount();
    }

    // Fetch tickets due this month
    public int GetTicketsDueThisMonthCount()
    {
        return _ticketsDao.GetTicketsDueThisMonthCount();
    }

    // Fetch tickets due more than a month from now
    public int GetTicketsDueMoreThanMonthCount()
    {
        return _ticketsDao.GetTicketsDueMoreThanMonthCount();
    }

    // Fetch unresolved tickets by priority for pie chart
    public Dictionary<string, double> GetUnresolvedTicketsByPriority()
    {
        return _ticketsDao.GetUnresolvedTicketsByPriority();
    }
}