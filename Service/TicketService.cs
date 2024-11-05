using DAL;
using Model;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Service
{
    public class TicketService
    {
        private readonly TicketsDao _ticketsDao = new();

        public List<Ticket> GetAllTickets() => _ticketsDao.GetAllTickets();
        public Ticket? GetTicketById(ObjectId ticketId) => _ticketsDao.GetTicketById(ticketId);

        public void UpdateTicket(Ticket ticket) => _ticketsDao.UpdateTicket(ticket);
        public void UpdateTicketDynamic(Ticket ticket) => _ticketsDao.UpdateTicketDynamic(ticket);

        public void DeleteTicket(Ticket ticket) => _ticketsDao.DeleteTicket(ticket);
        
        public void InsertTicket(Ticket ticket) => _ticketsDao.InsertTicket(ticket);

        public int GetCountByStatus(int status) => _ticketsDao.GetCountByStatus(status);
        public int GetCountByPriority(int priority) => _ticketsDao.GetCountByPriority(priority);

        public int GetOverdueTicketsCount() => _ticketsDao.GetOverdueTicketsCount();
        public int GetTicketsDueTodayCount() => _ticketsDao.GetTicketsDueTodayCount();
        public int GetTicketsDueTomorrowCount() => _ticketsDao.GetTicketsDueTomorrowCount();

        // Calculate count of tickets due within this month
        public int GetTicketsDueThisMonthCount() => _ticketsDao.GetTicketsDueThisMonthCount();

        // Calculate count of tickets due beyond this month
        public int GetTicketsDueMoreThanMonthCount() => _ticketsDao.GetTicketsDueMoreThanMonthCount();
        
        public List<Ticket> GetTicketsByEmployeeId(ObjectId employeeId)
        {
            return _ticketsDao.GetTicketsByEmployeeId(employeeId);
        }

        public void AddTicket(Ticket ticket)
        {
            _ticketsDao.InsertTicket(ticket);
        }

        public double GetPriorityPercentage(int priority)
        {
            var totalTickets = _ticketsDao.GetAllTickets().Count;
            if (totalTickets == 0) return 0;

            var priorityCount = _ticketsDao.GetCountByPriority(priority);
            return (priorityCount / (double)totalTickets) * 100;
        }

        public double GetStatusPercentage(int status)
        {
            var totalTickets = _ticketsDao.GetAllTickets().Count;
            if (totalTickets == 0) return 0;

            var statusCount = _ticketsDao.GetCountByStatus(status);
            return (statusCount / (double)totalTickets) * 100;
        }
    }
}
