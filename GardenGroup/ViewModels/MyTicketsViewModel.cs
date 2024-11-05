using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GardenGroup.Views;
using Service;
using Ticket = Model.Ticket;

namespace GardenGroup.ViewModels
{
    public class MyTicketsViewModel : ViewModelBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<Ticket> Tickets { get; set; } = new ObservableCollection<Ticket>();
        
        public MyTicketsViewModel(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            _serviceManager = serviceManager;
            _mainViewModel = mainViewModel;
        
            AddNewTicketCommand = new RelayCommand(OpenTicketSubmission);
            LoadTickets();
        }

        public ICommand AddNewTicketCommand { get; }

        public void LoadTickets()
        {
            Tickets.Clear();
            var currentEmployee = _mainViewModel.CurrentEmployee;
            if (currentEmployee != null)
            {
                var tickets = _serviceManager.TicketService.GetTicketsByEmployeeId(currentEmployee.Id);
                foreach (var ticket in tickets)
                {
                    Tickets.Add(ticket);
                }
            }
        }

        private void OpenTicketSubmission(object obj)
        {
            var ticketSubmissionWindow = new TicketSubmission(_serviceManager, _mainViewModel);
            ticketSubmissionWindow.ShowDialog();
            LoadTickets(); // Refresh the tickets after submission
        }
    }
}