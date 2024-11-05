using System.Windows;
using System.Windows.Controls;
using GardenGroup.ViewModels;
using MongoDB.Bson;

namespace GardenGroup.Views;

/// <summary>
/// Interaction logic for Ticket.xaml.
/// Used by service employees
/// </summary>
public partial class Ticket : UserControl
{
        
    private TicketViewModel ViewModel => DataContext as TicketViewModel ?? throw new NullReferenceException();
    private MainViewModel MainViewModel => Application.Current.MainWindow.DataContext as MainViewModel ?? throw new NullReferenceException();
    private List<Model.Ticket> _tickets;
        
    public Ticket()
    {
        Loaded += (s, _) => PrepareView();
            
        InitializeComponent();
    }

    private List<Model.Ticket> GetTickets() => ViewModel.ServiceManager.TicketService.GetAllTickets(); 
    
    private void PrepareView()
    {
        _tickets = GetTickets();
        InitComboboxItem();
        InitDatePickers();
            
        UpdateTicketList();
    }

    private void UpdateTicketList(List<Model.Ticket> tickets = null)
    {
        var data = tickets;
        if(data == null)
            data = _tickets;
        
        //var data = _tickets;
        data = data.OrderByDescending(x => x.Status == Model.Ticket.Statuses.Open).ToList();

        try
        {
            var parsed = Enum.TryParse<Model.Ticket.Statuses>(StatusBox.SelectedValue.ToString(), out var status);
            if (parsed)
                data = data.OrderByDescending(x => x.Status == status).ToList();

            parsed = Enum.TryParse<Model.Ticket.Priorities>(PriorityBox.SelectedValue.ToString(), out var priority);
            if (parsed)
                data = data.OrderByDescending(x => x.Priority == priority).ToList();
        }
        catch (NullReferenceException)
        { }
        
        if (!string.IsNullOrEmpty(EmployeeTxt.Text))
            data = data.OrderByDescending(x => x.AssignedEmployee?.FullName.Contains(EmployeeTxt.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        
        var startDate = StartDatePicker.SelectedDate ?? _tickets.Min(x => x.DateReported);
        var endDate = EndDatePicker.SelectedDate ?? DateTime.Today;
        
        data = data.Where(x => x.DateReported >= startDate && x.DateReported <= endDate).ToList();

        TicketsList.ItemsSource = data;
    }

    private void InitComboboxItem()
    {
        StatusBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Statuses));
        StatusBox.SelectedIndex = 0;
        
        PriorityBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Priorities));
        PriorityBox.SelectedIndex = 0;
    }

    private void InitDatePickers()
    {
        var earliestDate = _tickets.Min(x => x.DateReported);
        StartDatePicker.SelectedDate = earliestDate;
        
        EndDatePicker.SelectedDate = DateTime.Today;
    }

    private void PriorityBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateTicketList();

    private void EmployeeTxt_OnTextChanged(object sender, TextChangedEventArgs e) => UpdateTicketList();

    private void StatusBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateTicketList();

    private void TicketsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TicketsList.SelectedItem is not Model.Ticket selected)
            return;
          
        MainViewModel.SwitchToLookupTicket(selected.Id);
    }

    private void LookupTicket_FormClosed()
    {
        _tickets = GetTickets();
        UpdateTicketList();
    }

    private void StartDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e) => UpdateTicketList();
        
    private void EndDatePicker_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e) => UpdateTicketList();

    private void ResetDatesBtn_OnClick(object sender, RoutedEventArgs e)
    {
        InitDatePickers();
        UpdateTicketList();
    }

    private void NewTicketBtn_OnClick(object sender, RoutedEventArgs e) => 
        MainViewModel.SwitchToLookupTicket(ObjectId.Empty);

    // Bogdan's part
    private void SearchTxt_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchTxt = SearchTxt.Text;
        var ticketSearchHelper = new TicketSearchHelper();
        var data = ticketSearchHelper.FilterTicketsBySearchTerm(_tickets, searchTxt);
        
        UpdateTicketList(data);
    }
}