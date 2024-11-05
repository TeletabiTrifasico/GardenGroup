using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DAL;
using Model;
using Service;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;

namespace GardenGroup.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly TicketService _ticketService;
        private readonly MainViewModel _mainViewModel;
        private MainViewModel MainViewModel => Application.Current.MainWindow?.DataContext as MainViewModel ?? throw new NullReferenceException("MainViewModel is null");

        public DashboardViewModel(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            _ticketService = serviceManager.TicketService;
            _mainViewModel = mainViewModel;

            PriorityData = new ObservableCollection<PriorityModel>();
            AllTickets = new ObservableCollection<Ticket>();

            LowPriorityPercentage = new ChartValues<double> { 0 };
            NormalPriorityPercentage = new ChartValues<double> { 0 };
            HighPriorityPercentage = new ChartValues<double> { 0 };

            LoadDashboardData();
            LoadAllTickets();
        }

        public Employee GetCurrentEmployee()
        {
            return MainViewModel.CurrentEmployee ?? throw new NullReferenceException("CurrentEmployee is null");
        }
        public string UserName => MainViewModel.CurrentEmployee?.Firstname ?? "Unknown";
        public string UserRole => MainViewModel.CurrentEmployee?.UserType.ToString() ?? "Role not available";



        // Metric Boxes Properties
        private int _overdueTasks;
        public int OverdueTasks
        {
            get => _overdueTasks;
            set { _overdueTasks = value; OnPropertyChanged(); }
        }

        private int _ticketsDueToday;
        public int TicketsDueToday
        {
            get => _ticketsDueToday;
            set { _ticketsDueToday = value; OnPropertyChanged(); }
        }

        private int _openTickets;
        public int OpenTickets
        {
            get => _openTickets;
            set { _openTickets = value; OnPropertyChanged(); }
        }

        private int _ticketsOnHold;
        public int TicketsOnHold
        {
            get => _ticketsOnHold;
            set { _ticketsOnHold = value; OnPropertyChanged(); }
        }

        private int _allTicketsCount;
        public int AllTicketsCount
        {
            get => _allTicketsCount;
            set { _allTicketsCount = value; OnPropertyChanged(); }
        }

        // Ticket Status Overview Properties
        private double _openPercentage;
        public double OpenPercentage
        {
            get => _openPercentage;
            set { _openPercentage = value; OnPropertyChanged(); }
        }

        private double _inProgressPercentage;
        public double InProgressPercentage
        {
            get => _inProgressPercentage;
            set { _inProgressPercentage = value; OnPropertyChanged(); }
        }

        private double _resolvedPercentage;
        public double ResolvedPercentage
        {
            get => _resolvedPercentage;
            set { _resolvedPercentage = value; OnPropertyChanged(); }
        }

        private double _closedWithoutResolutionPercentage;
        public double ClosedWithoutResolutionPercentage
        {
            get => _closedWithoutResolutionPercentage;
            set { _closedWithoutResolutionPercentage = value; OnPropertyChanged(); }
        }

        // Properties for Pie Chart Values
        public ChartValues<double> LowPriorityPercentage { get; set; }
        public ChartValues<double> NormalPriorityPercentage { get; set; }
        public ChartValues<double> HighPriorityPercentage { get; set; }

        // Unresolved Tickets by Priority (Pie Chart) Properties
        public ObservableCollection<PriorityModel> PriorityData { get; set; }

        // Observable collection to hold all tickets for the scrollable list
        public ObservableCollection<Ticket> AllTickets { get; set; }

        // "Tickets Coming Due" Properties
        public int TicketsDueTomorrow { get; set; }
        public int TicketsDueThisMonth { get; set; }
        public int TicketsDueMoreThanMonth { get; set; }

        private void LoadDashboardData()
        {
            // Load metrics for Metric Boxes
            OverdueTasks = _ticketService.GetOverdueTicketsCount();
            TicketsDueToday = _ticketService.GetTicketsDueTodayCount();
            OpenTickets = _ticketService.GetCountByStatus((int)Ticket.Statuses.Open);
            TicketsOnHold = _ticketService.GetCountByStatus((int)Ticket.Statuses.InProgress);
            AllTicketsCount = _ticketService.GetAllTickets().Count;

            // Tickets Coming Due metrics
            TicketsDueTomorrow = _ticketService.GetTicketsDueTomorrowCount();
            TicketsDueThisMonth = CalculateTicketsDueThisMonth();
            TicketsDueMoreThanMonth = CalculateTicketsDueMoreThanMonth();

            // Load status percentages
            UpdateStatusPercentages();

            // Load priority data for Pie Chart
            UpdatePriorityData();
        }

        private void LoadAllTickets()
        {
            // Clear the existing tickets and load fresh data
            AllTickets.Clear();
            var tickets = _ticketService.GetAllTickets();
            foreach (var ticket in tickets)
            {
                AllTickets.Add(ticket);
            }
        }

        private void UpdateStatusPercentages()
        {
            int totalTickets = AllTicketsCount > 0 ? AllTicketsCount : 1; // Avoid division by zero
            OpenPercentage = _ticketService.GetCountByStatus((int)Ticket.Statuses.Open) / (double)totalTickets * 100;
            InProgressPercentage = _ticketService.GetCountByStatus((int)Ticket.Statuses.InProgress) / (double)totalTickets * 100;
            ResolvedPercentage = _ticketService.GetCountByStatus((int)Ticket.Statuses.Closed) / (double)totalTickets * 100;
            ClosedWithoutResolutionPercentage = _ticketService.GetCountByStatus((int)Ticket.Statuses.Closed) / (double)totalTickets * 100;
        }

        private void UpdatePriorityData()
        {
            var totalTickets = AllTicketsCount > 0 ? AllTicketsCount : 1; // Avoid division by zero

            // Update ChartValues to reflect new data and got them rounded to 2 decimal places
            LowPriorityPercentage[0] = Math.Round(_ticketService.GetCountByPriority((int)Ticket.Priorities.Low) / (float)totalTickets * 100, 2);
            NormalPriorityPercentage[0] = Math.Round(_ticketService.GetCountByPriority((int)Ticket.Priorities.Normal) / (float)totalTickets * 100, 2);
            HighPriorityPercentage[0] = Math.Round(_ticketService.GetCountByPriority((int)Ticket.Priorities.High) / (float)totalTickets * 100, 2);

            // Notify the view of the changes
            OnPropertyChanged(nameof(LowPriorityPercentage));
            OnPropertyChanged(nameof(NormalPriorityPercentage));
            OnPropertyChanged(nameof(HighPriorityPercentage));
        }

        private int CalculateTicketsDueThisMonth() => _ticketService.GetTicketsDueThisMonthCount();
        private int CalculateTicketsDueMoreThanMonth() => _ticketService.GetTicketsDueMoreThanMonthCount();

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
