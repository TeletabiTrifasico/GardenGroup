using System.Windows;
using System.Windows.Controls;
using MongoDB.Bson;
using Service;

namespace GardenGroup.Views.Windows;

public partial class LookupTicket : Window
{
    private IServiceManager _serviceManager;
    private Model.Ticket _ticket = null!;
    
    public LookupTicket(IServiceManager serviceManager, ObjectId ticketId)
    {
        _serviceManager = serviceManager;
        Loaded += (s, e) => LoadTicket(ticketId); 
        
        InitializeComponent();
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
            Close();
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
    }

    private string GetTicketChanges()
    {
        var description = string.Empty;

        if ((int)PriorityBox.Tag != PriorityBox.SelectedIndex)
            description += $"\nPriority has changed to {(Model.Ticket.Priorities)PriorityBox.SelectedIndex}\n";
        
        if((int)StatusBox.Tag != StatusBox.SelectedIndex)
            description += $"\nStatus has changed to {(Model.Ticket.Statuses)StatusBox.SelectedIndex}\n";
        
        return description;
    }

    private void StatusBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _ticket.Status = (Model.Ticket.Statuses)StatusBox.SelectedIndex;

    private void PriorityBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _ticket.Priority = (Model.Ticket.Priorities)PriorityBox.SelectedIndex;
}