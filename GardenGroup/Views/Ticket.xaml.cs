using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GardenGroup.ViewModels;

namespace GardenGroup.Views
{
    /// <summary>
    /// Interaction logic for Ticket.xaml
    /// </summary>
    public partial class Ticket : UserControl
    {
        
        private TicketViewModel ViewModel => DataContext as TicketViewModel ?? throw new NullReferenceException();
        
        public Ticket()
        {
            Loaded += (s, e) => AddTicketsToList();
            
            InitializeComponent();
        }

        private void AddTicketsToList()
        {
            var data = ViewModel.ServiceManager.TicketService.GetAllEmployeesTicketsAsync();
            
            ticketsList.Items.Clear();
            ticketsList.ItemsSource = data;
        }
    }
}
