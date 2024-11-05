using System;
using System.Windows;
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
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ManageEmployeesViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
