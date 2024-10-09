namespace Model;

public class EmployeeTicket
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Subject { get; set; }
    public Ticket.Types IncidentType { get; set; }
    public string AssignedTo { get; set; }
    
    public DateTime DateReported { get; set; }
    public string ParsedDateReported => DateReported.ToString("dd/MM/yyyy HH:mm");

    public Ticket.Priorities Priority { get; set; }
    public Ticket.Statuses Status { get; set; }
}