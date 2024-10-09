using System.Windows;
using System.Windows.Controls;
using Service;

namespace GardenGroup.Views.Windows;

public partial class LookupTicket : Window
{
    private IServiceManager _serviceManager;
    private Model.Ticket _ticket;
    public LookupTicket(IServiceManager serviceManager, Guid ticketId)
    {
        _serviceManager = serviceManager;
        Loaded += (s, e) => LoadTicket(ticketId); 
        
        InitializeComponent();
    }

    private void LoadTicket(Guid ticketId)
    {
        try
        {
            _ticket = _serviceManager.TicketService.GetTicketById(ticketId);
            if(_ticket == null)
                throw new NullReferenceException("Ticket not found");
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error while retriving ticket: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
        InitProperties();
    }

    private void InitProperties()
    {
        SubjectLabel.Content = _ticket.Subject;
        
        StatusBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Statuses));
        StatusBox.SelectedIndex = (int)_ticket.Status;
            
        PriorityBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Priorities));
        PriorityBox.SelectedIndex = (int)_ticket.Priority;
        
        DescriptionTxt.Text = _ticket.Description;
    }

    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        _serviceManager.TicketService.UpdateTicket(_ticket);
        MessageBox.Show("Ticket updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void StatusBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _ticket.Status = (Model.Ticket.Statuses)StatusBox.SelectedIndex;

    private void PriorityBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _ticket.Priority = (Model.Ticket.Priorities)PriorityBox.SelectedIndex;
}