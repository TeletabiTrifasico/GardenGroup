using System.Collections.ObjectModel;
using System.Windows.Input;
using GardenGroup.ViewModels;
using Model;
using Service;

namespace GardenGroup.ViewModels
{
    public class TicketSubmissionViewModel : ViewModelBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly MainViewModel _mainViewModel;  
        private string _title;
        private string _description;
        private Ticket.Types _category;
        private Ticket.Priorities _priority;
        private string _confirmationMessage;

        public TicketSubmissionViewModel(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            _serviceManager = serviceManager;
            _mainViewModel = mainViewModel;  // Store the reference to MainViewModel
            SubmitTicketCommand = new RelayCommand(OnSubmitTicket);
            Categories = new ObservableCollection<Ticket.Types>((Ticket.Types[])Enum.GetValues(typeof(Ticket.Types)));
            Priorities = new ObservableCollection<Ticket.Priorities>((Ticket.Priorities[])Enum.GetValues(typeof(Ticket.Priorities)));
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Ticket.Types Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public Ticket.Priorities Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        public string ConfirmationMessage
        {
            get => _confirmationMessage;
            set => SetProperty(ref _confirmationMessage, value);
        }

        public ObservableCollection<Ticket.Types> Categories { get; }
        public ObservableCollection<Ticket.Priorities> Priorities { get; }

        public ICommand SubmitTicketCommand { get; }

        public event Action? SubmitCompleted;

        private void OnSubmitTicket(object obj)
        {
            var currentEmployee = _mainViewModel.CurrentEmployee;
            if (currentEmployee == null)
            {
                ConfirmationMessage = "Error: No logged-in employee found.";
                return;
            }

            var ticket = new Ticket
            {
                Subject = Title,
                Description = Description,
                IncidentType = Category,
                Priority = Priority,
                Status = Ticket.Statuses.Open,
                DateReported = DateTime.UtcNow,
                Assigned = currentEmployee.Id
            };

            _serviceManager.TicketService.AddTicket(ticket);
            ConfirmationMessage = "Ticket submitted successfully!";
        
            SubmitCompleted?.Invoke();
        }
    }
}
