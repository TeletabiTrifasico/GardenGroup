using System.Windows;
using System.Windows.Controls;
using GardenGroup.ViewModels;
using Service;

namespace GardenGroup.Views
{
    public partial class MyTickets : UserControl // Ensure this is a UserControl if used within MainWindow, or Window if opened separately
    {
        public MyTickets(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            InitializeComponent();
            
            if (serviceManager == null || mainViewModel == null)
            {
                throw new ArgumentNullException("serviceManager or mainViewModel is null");
            }

            DataContext = new MyTicketsViewModel(serviceManager, mainViewModel);
        }
    }
}