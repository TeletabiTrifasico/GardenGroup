using Model;

// Bogdan's part
public class TicketSearchHelper
{
    public List<Ticket> FilterTicketsBySearchTerm(List<Ticket> tickets, string searchTerm)
    {
        if (tickets == null) 
            return new List<Ticket>();
        
        if (string.IsNullOrEmpty(searchTerm))
            return tickets;

        var keywords = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return tickets.Where(ticket =>
            keywords.All(keyword =>
                ticket.Subject.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                ticket.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        ).ToList();
    }
}