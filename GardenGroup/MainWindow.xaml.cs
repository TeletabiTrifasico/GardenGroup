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


    private void ticketsBtn_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SwitchToTickets();
    }

    private void dashboardBtn_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SwitchToDashboard();
    }
}