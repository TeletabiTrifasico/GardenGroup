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
    private Model.Ticket _ticket = null!;
    
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
            _ticket = _serviceManager.TicketService.GetTicketById(ticketId) ?? throw new NullReferenceException("Ticket not found");
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error while retrieving ticket: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        InitProperties();
    }

    private void InitProperties()
    {
        SubjectLabel.Content = _ticket.Subject;
        
        StatusBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Statuses));
        StatusBox.SelectedIndex = (int)_ticket.Status;
        StatusBox.Tag = (int)_ticket.Status;
            
        PriorityBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Priorities));
        PriorityBox.SelectedIndex = (int)_ticket.Priority;
        PriorityBox.Tag = (int)_ticket.Priority;
        
        DescriptionTxt.Text = _ticket.Description;
    }

    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            _ticket.Description += GetTicketChanges();
            _serviceManager.TicketService.UpdateTicket(_ticket);
        }
        catch (Exception)
        {
            MessageBox.Show("Error while saving ticket", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        MessageBox.Show("Ticket updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        MainViewModel.SwitchToTickets();
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