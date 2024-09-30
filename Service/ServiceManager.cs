using DAL;

namespace Service;

public class ServiceManager : IServiceManager
{
    public EmployeeService EmployeeService { get; set; } = new();
    public TicketService TicketService { get; set; } = new();
}