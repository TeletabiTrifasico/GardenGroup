﻿using System.Windows.Controls;
using GardenGroup.ViewModels;
using GardenGroup.Views.Windows;
using Model;

namespace GardenGroup.Views
{
    /// <summary>
    /// Interaction logic for Ticket.xaml
    /// </summary>
    public partial class Ticket : UserControl
    {
        
        private TicketViewModel ViewModel => DataContext as TicketViewModel ?? throw new NullReferenceException();
        private List<Model.EmployeeTicket> _tickets;
        
        public Ticket()
        {
            Loaded += (s, e) => GetData();
            
            InitializeComponent();
        }

        private void GetData()
        {
            _tickets = ViewModel.ServiceManager.TicketService.GetAllEmployeesTicketsAsync();
            InitComboxItem();
            
            UpdateTicketList();
        }

        private void UpdateTicketList()
        {
            var data = _tickets;
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
                data = data.OrderByDescending(x => x.FullName.Contains(EmployeeTxt.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            
            TicketsList.ItemsSource = data;
        }

        private void InitComboxItem()
        {
            StatusBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Statuses));
            StatusBox.SelectedIndex = 0;
            
            PriorityBox.ItemsSource = Enum.GetValues(typeof(Model.Ticket.Priorities));
            PriorityBox.SelectedIndex = 0;
        }

        private void PriorityBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateTicketList();

        private void EmployeeTxt_OnTextChanged(object sender, TextChangedEventArgs e) => UpdateTicketList();

        private void StatusBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateTicketList();

        private void TicketsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = TicketsList.SelectedItem as EmployeeTicket;
            if (selected == null)
                return;
            
            var lookup = new LookupTicket(ViewModel.ServiceManager, selected.Id);
            lookup.Show();
        }
    }
}
