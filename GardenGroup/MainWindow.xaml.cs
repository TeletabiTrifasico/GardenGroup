using System.Windows;
using GardenGroup.StartupHelpers;
using GardenGroup.ViewModels;

namespace GardenGroup;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IViewModelFactory viewModelFactory)
    {
        DataContext = new MainViewModel(viewModelFactory);
        InitializeComponent();
    }

    private MainViewModel ViewModel => DataContext as MainViewModel ?? throw new NullReferenceException();


    private void LogoutBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToLogout();
    private void ticketsBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToTickets();

    private void dashboardBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToDashboard();

    private void ManageEmployeesBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToManageEmployees();
}