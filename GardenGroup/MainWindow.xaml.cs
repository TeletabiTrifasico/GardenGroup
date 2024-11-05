using System.Windows;
using GardenGroup.StartupHelpers;
using GardenGroup.ViewModels;

namespace GardenGroup;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel ViewModel => DataContext as MainViewModel ?? throw new NullReferenceException();
    
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }

    private void LogoutBtn_Click(object sender, RoutedEventArgs e) =>
        ViewModel.SwitchToLogout();
    private void ticketsBtn_Click(object sender, RoutedEventArgs e) =>
        ViewModel.SwitchToTickets();

    private void dashboardBtn_Click(object sender, RoutedEventArgs e) =>
        ViewModel.SwitchToDashboard();

    private void ManageEmployeesBtn_Click(object sender, RoutedEventArgs e) =>
        ViewModel.SwitchToManageEmployees();
}