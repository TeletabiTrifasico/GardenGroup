using Model;
using Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GardenGroup.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly TicketService _ticketService;

        public DashboardViewModel(IServiceManager serviceManager)
        {
            _ticketService = serviceManager.TicketService;
            UpdateTicketStatus();
            UpdateMetrics();
            UpdateMetricsByPriority();
        }

        // Properties for Ticket Status Overview (Progress Bars)
        private double _openPercentage;
        public double OpenPercentage
        {
            get => _openPercentage;
            set
            {
                _openPercentage = value;
                OnPropertyChanged();
            }
        }

        private double _inProgressPercentage;
        public double InProgressPercentage
        {
            get => _inProgressPercentage;
            set
            {
                _inProgressPercentage = value;
                OnPropertyChanged();
            }
        }

        private double _resolvedPercentage;
        public double ResolvedPercentage
        {
            get => _resolvedPercentage;
            set
            {
                _resolvedPercentage = value;
                OnPropertyChanged();
            }
        }
        private double _closedWithoutResolutionPercentage;

        public double ClosedWithoutResolutionPercentage
        {
            get => _closedWithoutResolutionPercentage;
            set
            {
                _closedWithoutResolutionPercentage = value;
                OnPropertyChanged(nameof(ClosedWithoutResolutionPercentage));
            }
        }


        // Properties for Metric Boxes
        private int _overdueTasks;
        public int OverdueTasks
        {
            get => _overdueTasks;
            set
            {
                _overdueTasks = value;
                OnPropertyChanged();
            }
        }

        private int _ticketsDueToday;
        public int TicketsDueToday
        {
            get => _ticketsDueToday;
            set
            {
                _ticketsDueToday = value;
                OnPropertyChanged();
            }
        }

        private int _openTickets;
        public int OpenTickets
        {
            get => _openTickets;
            set
            {
                _openTickets = value;
                OnPropertyChanged();
            }
        }

        private int _ticketsOnHold;
        public int TicketsOnHold
        {
            get => _ticketsOnHold;
            set
            {
                _ticketsOnHold = value;
                OnPropertyChanged();
            }
        }

        private int _allTickets;
        public int AllTickets
        {
            get => _allTickets;
            set
            {
                _allTickets = value;
                OnPropertyChanged();
            }
        }

        // Properties for Unresolved Tickets by Priority (Pie Chart)
        private double _lowPriorityPercentage;
        public double LowPriorityPercentage
        {
            get => _lowPriorityPercentage;
            set
            {
                _lowPriorityPercentage = value;
                OnPropertyChanged();
            }
        }

        private double _normalPriorityPercentage;
        public double NormalPriorityPercentage
        {
            get => _normalPriorityPercentage;
            set
            {
                _normalPriorityPercentage = value;
                OnPropertyChanged();
            }
        }

        private double _highPriorityPercentage;
        public double HighPriorityPercentage
        {
            get => _highPriorityPercentage;
            set
            {
                _highPriorityPercentage = value;
                OnPropertyChanged();
            }
        }

        // Properties for "Tickets Coming Due"
        public int TicketsDueTomorrow { get; set; }
        public int TicketsDueThisMonth { get; set; }
        public int TicketsDueMoreThanMonth { get; set; }

        // Fetch ticket status percentages
        private void UpdateTicketStatus()
        {
            var statusData = _ticketService.GetTicketStatusPercentage();

            OpenPercentage = statusData.ContainsKey("Open") ? statusData["Open"] : 0;
            InProgressPercentage = statusData.ContainsKey("InProgress") ? statusData["InProgress"] : 0;
            ResolvedPercentage = statusData.ContainsKey("Resolved") ? statusData["Resolved"] : 0;
            ClosedWithoutResolutionPercentage = statusData.ContainsKey("Closed") ? statusData["Closed"] : 0;
        }

        // Fetch metrics from TicketService
        private void UpdateMetrics()
        {
            OverdueTasks = _ticketService.GetOverdueTaskCount();
            TicketsDueToday = _ticketService.GetTicketsDueTodayCount();
            OpenTickets = _ticketService.GetOpenTicketCount();
            TicketsOnHold = _ticketService.GetTicketsOnHoldCount();
            AllTickets = _ticketService.GetAllTicketCount();
            TicketsDueTomorrow = _ticketService.GetTicketsDueTomorrowCount();
            TicketsDueThisMonth = _ticketService.GetTicketsDueThisMonthCount();
            TicketsDueMoreThanMonth = _ticketService.GetTicketsDueMoreThanMonthCount();
        }

        // Observable collection to hold pie chart data
        public ObservableCollection<PriorityModel> PriorityData { get; set; }

        private void UpdateMetricsByPriority()
        {
            var priorityData = _ticketService.GetUnresolvedTicketsByPriority();
            PriorityData = new ObservableCollection<PriorityModel>
            {
                new PriorityModel { Priority = "Low", Percentage = priorityData.ContainsKey("Low") ? priorityData["Low"] : 0 },
                new PriorityModel { Priority = "Normal", Percentage = priorityData.ContainsKey("Normal") ? priorityData["Normal"] : 0 },
                new PriorityModel { Priority = "High", Percentage = priorityData.ContainsKey("High") ? priorityData["High"] : 0 }
            };
            OnPropertyChanged(nameof(PriorityData));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PriorityModel
    {
        public string Priority { get; set; }
        public double Percentage { get; set; }
    }
}
