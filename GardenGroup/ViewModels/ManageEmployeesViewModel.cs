using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Model;
using Service;
using MongoDB.Bson;

namespace GardenGroup.ViewModels
{
    public class ManageEmployeesViewModel : INotifyPropertyChanged
    {
        private readonly IServiceManager _serviceManager;

        private ObservableCollection<EmployeeViewModel> _employees;
        public ObservableCollection<EmployeeViewModel> Employees
        {
            get => _employees;
            private set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _isAddEditPopupOpen;
        public bool IsAddEditPopupOpen
        {
            get => _isAddEditPopupOpen;
            set
            {
                _isAddEditPopupOpen = value;
                OnPropertyChanged(nameof(IsAddEditPopupOpen));
            }
        }

        private bool _isDeleteConfirmationOpen;
        public bool IsDeleteConfirmationOpen
        {
            get => _isDeleteConfirmationOpen;
            set
            {
                _isDeleteConfirmationOpen = value;
                OnPropertyChanged(nameof(IsDeleteConfirmationOpen));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            private set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
                OnPropertyChanged(nameof(PopupTitle));
            }
        }

        public string PopupTitle => IsEditing ? "Edit Employee" : "Add Employee";

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            private set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        private string _emailError;
        public string EmailError
        {
            get => _emailError;
            private set
            {
                _emailError = value;
                OnPropertyChanged(nameof(EmailError));
                OnPropertyChanged(nameof(IsEmailErrorVisible));
            }
        }

        private string _phoneError;
        public string PhoneError
        {
            get => _phoneError;
            private set
            {
                _phoneError = value;
                OnPropertyChanged(nameof(PhoneError));
                OnPropertyChanged(nameof(IsPhoneErrorVisible));
            }
        }

        public Visibility IsEmailErrorVisible => string.IsNullOrEmpty(EmailError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPhoneErrorVisible => string.IsNullOrEmpty(PhoneError) ? Visibility.Collapsed : Visibility.Visible;

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand SaveEmployeeCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ConfirmDeleteCommand { get; }

        public ManageEmployeesViewModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            LoadEmployees();

            AddEmployeeCommand = new RelayCommand(AddEmployee);
            EditEmployeeCommand = new RelayCommand(EditEmployee, CanEditOrDeleteEmployee);
            DeleteEmployeeCommand = new RelayCommand(OpenDeleteConfirmation, CanEditOrDeleteEmployee);
            SaveEmployeeCommand = new RelayCommand(SaveEmployee);
            CancelCommand = new RelayCommand(Cancel);
            ConfirmDeleteCommand = new RelayCommand(ConfirmDelete);
        }

        private void LoadEmployees()
        {
            var employeeModels = _serviceManager.EmployeeService.GetEmployees();
            Employees = new ObservableCollection<EmployeeViewModel>(
                employeeModels.Select(e => new EmployeeViewModel(e))
            );
        }

        private void AddEmployee(object parameter)
        {
            SelectedEmployee = new Employee();
            Username = string.Empty;
            Password = string.Empty;
            IsEditing = false;
            IsAddEditPopupOpen = true;
        }
        private void EditEmployee(object parameter)
        {
            if (parameter is EmployeeViewModel employeeToEdit)
            {
                SelectedEmployee = employeeToEdit.Employee;
                Username = employeeToEdit.Employee.Username;
                Password = string.Empty;
                IsEditing = true;
                IsAddEditPopupOpen = true;
            }
        }

        private bool ValidateInput()
        {
            EmailError = string.Empty;
            PhoneError = string.Empty;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(SelectedEmployee.Email))
            {
                EmailError = "Invalid email format. Example: example@domain.com";
            }

            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$");
            if (!phoneRegex.IsMatch(SelectedEmployee.Phone))
            {
                PhoneError = "Invalid phone format. Example: +1234567890";
            }

            return string.IsNullOrEmpty(EmailError) && string.IsNullOrEmpty(PhoneError);
        }

        private void SaveEmployee(object parameter)
        {
            if (!ValidateInput())
            {
                return;
            }

            SelectedEmployee.Username = Username;
            if (!string.IsNullOrEmpty(Password))
            {
                SelectedEmployee.Password = HashPassword(Password);
            }

            if (IsEditing)
            {
                _serviceManager.EmployeeService.UpdateEmployee(SelectedEmployee);
            }
            else
            {
                _serviceManager.EmployeeService.AddEmployee(SelectedEmployee);
            }

            IsAddEditPopupOpen = false;
            LoadEmployees();
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        private void Cancel(object parameter)
        {
            IsAddEditPopupOpen = false;
            IsDeleteConfirmationOpen = false;
            SelectedEmployee = null;
        }

        private void OpenDeleteConfirmation(object parameter)
        {
            if (parameter is EmployeeViewModel employeeToDelete)
            {
                SelectedEmployee = employeeToDelete.Employee;
                IsDeleteConfirmationOpen = true;
            }
        }

        private void ConfirmDelete(object parameter)
        {
            if (SelectedEmployee != null)
            {
                _serviceManager.EmployeeService.DeleteEmployee(SelectedEmployee.Id);
                LoadEmployees();
            }
            IsDeleteConfirmationOpen = false;
            SelectedEmployee = null;
        }

        private bool CanEditOrDeleteEmployee(object parameter) => parameter is EmployeeViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Nested EmployeeViewModel class to handle display logic for roles
        public class EmployeeViewModel
        {
            public Employee Employee { get; }
            public string UserTypeDisplay => GetUserTypeDisplay(Employee.UserType);

            public EmployeeViewModel(Employee employee)
            {
                Employee = employee;
            }

            private string GetUserTypeDisplay(Privilieges userType)
            {
                return userType switch
                {
                    Privilieges.NormalUser => "Normal User",
                    Privilieges.ServiceDesk => "Service Desk",
                    _ => "Unknown Role"
                };
            }
        }
    }
}
