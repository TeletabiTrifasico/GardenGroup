using System;
using System.Windows.Controls;
using GardenGroup.ViewModels;
using Service;

namespace GardenGroup.Views
{
    /// <summary>
    /// Interaction logic for ManageEmployees.xaml
    /// </summary>
    public partial class ManageEmployees : UserControl
    {
        public ManageEmployees()
        {
            InitializeComponent();

            DataContext = new ManageEmployeesViewModel(new ServiceManager());
        }
    }
}
