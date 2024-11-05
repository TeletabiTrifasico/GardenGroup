using System.Windows;
using System.Windows.Controls;
using GardenGroup.ViewModels;
using MongoDB.Bson;
using Service;

namespace GardenGroup.Views
{
    public partial class MyTickets : UserControl // Ensure this is a UserControl if used within MainWindow, or Window if opened separately
    {
        private MainViewModel MainViewModel => Application.Current.MainWindow.DataContext as MainViewModel ?? throw new NullReferenceException();
        public MyTickets(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            InitializeComponent();
            
            if (serviceManager == null || mainViewModel == null)
            {
                throw new ArgumentNullException("serviceManager or mainViewModel is null");
            }

            DataContext = new MyTicketsViewModel(serviceManager, mainViewModel);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ticketId = ((Button)sender).Tag as ObjectId? ?? default;
            MainViewModel.SwitchToLookupTicket(ticketId);
        }
    }
}