using System.Windows;
using System.Windows.Controls;
using GardenGroup.ViewModels;
using MongoDB.Bson;
using Service;

namespace GardenGroup.Views;

public partial class LookupTicket : UserControl
{
    private LookupTicketViewModel ViewModel => DataContext as LookupTicketViewModel ?? throw new NullReferenceException();
    private MainViewModel MainViewModel => Application.Current.MainWindow.DataContext as MainViewModel ?? throw new NullReferenceException();

    private IServiceManager _serviceManager;
    private Model.Ticket _ticket;
    private bool EditMode { get; set; } = false;

    public LookupTicket()
    {
        Loaded += (s, _) => PrepareView();
        
        InitializeComponent();
    }

    private void PrepareView()
    {
        _serviceManager = ViewModel.ServiceManager;
        LoadTicket(ViewModel.TicketId);
    }

    private void LoadTicket(ObjectId ticketId)
    {
        try
        {
            if (ticketId == ObjectId.Empty)
            {
                _ticket = new Model.Ticket();
                EditMode = true;
            }
            else
                _ticket = _serviceManager.TicketService.GetTicketById(ticketId) ?? throw new NullReferenceException();

        }
        catch (Exception e)
        {
            MessageBox.Show($"Error while retrieving ticket: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        InitProperties();
        
        if(!EditMode)
            AppendPropertiesValue();
    }

    private void InitProperties()
    {
        StatusBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Statuses));
        PriorityBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Priorities));
        IncidentTypeBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Types));
        

        if (!EditMode) 
            return;
        
        StatusBox.SelectedIndex = 0;
        PriorityBox.SelectedIndex = 0;
        IncidentTypeBox.SelectedIndex = 0;
        
        TitleGrid.Visibility = Visibility.Visible;
        DeadlineGrid.Visibility = Visibility.Visible;
    }

    private void AppendPropertiesValue()
    {
        SubjectLabel.Content = _ticket.Subject;
        
        StatusBox.SelectedIndex = (int)_ticket.Status;
        StatusBox.Tag = (int)_ticket.Status;
        
        PriorityBox.SelectedIndex = (int)_ticket.Priority;
        PriorityBox.Tag = (int)_ticket.Priority;
        
        DescriptionTxt.Text = _ticket.Description;
    }

    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            switch (EditMode)
            {
                case true when AppendNewTicketValues():
                    _serviceManager.TicketService.InsertTicket(_ticket);
                    break;
                case false:
                    _ticket.Description = DescriptionTxt.Text += GetTicketChanges();
                    _serviceManager.TicketService.UpdateTicket(_ticket);
                    break;
                default:
                    return;
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Error while saving ticket changes", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        MessageBox.Show($"Ticket {(EditMode ? "saved" : "updated")} successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        MainViewModel.SwitchToTickets();
    }

    private bool AppendNewTicketValues()
    {
        try
        {
            _ticket.Subject = string.IsNullOrEmpty(SubjectTxt.Text) 
                ? throw new Exception("Subject cannot be empty") 
                : SubjectTxt.Text;

            _ticket.Priority = (Model.Ticket.Priorities)PriorityBox.SelectedIndex;
            _ticket.Status = (Model.Ticket.Statuses)StatusBox.SelectedIndex;
            _ticket.IncidentType = (Model.Ticket.Types)IncidentTypeBox.SelectedIndex;

            _ticket.Assigned = MainViewModel.CurrentEmployee.Id;
            
            var selectedDate = DeadlinePicker.SelectedDate;
            if(selectedDate == null && selectedDate < DateTime.Today)
                throw new Exception("Selected date is invalid or empty");
            
            _ticket.Deadline = selectedDate!.Value;
            _ticket.DateReported = DateTime.Now;

            _ticket.Description = string.IsNullOrEmpty(DescriptionTxt.Text)
                ? throw new Exception("Description cannot be empty") 
                : DescriptionTxt.Text;
            
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while creating ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }

    private string GetTicketChanges()
    {
        var description = string.Empty;

        if ((int)PriorityBox.Tag != PriorityBox.SelectedIndex)
            description += $"\nPriority has changed to {(Model.Ticket.Priorities)PriorityBox.SelectedIndex}\n";
        
        if((int)StatusBox.Tag != StatusBox.SelectedIndex)
            description += $"{((int)PriorityBox.Tag != PriorityBox.SelectedIndex ? string.Empty : "\n")}Status has changed to {(Model.Ticket.Statuses)StatusBox.SelectedIndex}\n";
        
        return description;
    }

    private void StatusBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _ticket.Status = (Model.Ticket.Statuses)StatusBox.SelectedIndex;

    private void PriorityBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _ticket.Priority = (Model.Ticket.Priorities)PriorityBox.SelectedIndex;

    private void ReturnBtn_OnClick(object sender, RoutedEventArgs e) => MainViewModel.SwitchToTickets();
}