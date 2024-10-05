namespace Model;

public class EmployeeTicket
{
    public string FullName { get; set; }
    public string Subject { get; set; }
    public int IncidentType { get; set; }
    public string AssignedTo { get; set; }
    
    public DateTime DateReported { get; set; }
    public Ticket.Priorities Priority { get; set; }
    public Ticket.Statuses Status { get; set; }
}