using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAL;
using GardenGroup.StartupHelpers;
using GardenGroup.ViewModels;
using GardenGroup.Views;
using Service;

namespace GardenGroup;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IViewModelFactory viewModelFactory)
    {
        DataContext = new ViewModels.MainViewModel(viewModelFactory);
        InitializeComponent();
    }


    private void LogoutBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToLogout();
    private void ticketsBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToTickets();

    private void dashboardBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToDashboard();

    private void ManageEmployeesBtn_Click(object sender, RoutedEventArgs e) =>
        ((MainViewModel)DataContext).SwitchToManageEmployees();
}