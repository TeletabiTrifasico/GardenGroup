using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            private set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
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

        // Popup title property to reflect Add or Edit state
        public string PopupTitle => IsEditing ? "Edit Employee" : "Add Employee";

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            private set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                OnPropertyChanged(nameof(UserTypeDisplay));
            }
        }

        // Display-friendly user type based on SelectedEmployee's UserType as an int
        public string UserTypeDisplay => SelectedEmployee != null ? GetUserTypeDisplay(SelectedEmployee.UserType) : "Unknown Role";

        // Method to convert the integer user_type value to a display string
        private string GetUserTypeDisplay(Privilieges userType)
        {
            return userType switch
            {
                Privilieges.NormalUser => "Normal User",
                Privilieges.ServiceDesk => "Service Desk",
                _ => "Unknown Role"
            };
        }

        // Validation properties
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

        // Visibility properties for validation messages
        public Visibility IsEmailErrorVisible => string.IsNullOrEmpty(EmailError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IsPhoneErrorVisible => string.IsNullOrEmpty(PhoneError) ? Visibility.Collapsed : Visibility.Visible;

        // Commands for CRUD operations
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

            // Initialize commands with respective methods
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            EditEmployeeCommand = new RelayCommand(EditEmployee, CanEditOrDeleteEmployee);
            DeleteEmployeeCommand = new RelayCommand(OpenDeleteConfirmation, CanEditOrDeleteEmployee);
            SaveEmployeeCommand = new RelayCommand(SaveEmployee);
            CancelCommand = new RelayCommand(Cancel);
            ConfirmDeleteCommand = new RelayCommand(ConfirmDelete);
        }

        private void LoadEmployees()
        {
            Employees = new ObservableCollection<Employee>(_serviceManager.EmployeeService.GetEmployees());
        }

        private void AddEmployee(object parameter)
        {
            SelectedEmployee = new Employee();
            IsEditing = false;
            IsAddEditPopupOpen = true;
        }

        private void EditEmployee(object parameter)
        {
            if (parameter is Employee employeeToEdit)
            {
                SelectedEmployee = employeeToEdit;
                IsEditing = true;
                IsAddEditPopupOpen = true;
            }
        }

        private bool ValidateInput()
        {
            EmailError = string.Empty;
            PhoneError = string.Empty;

            // Email validation
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(SelectedEmployee.Email))
            {
                EmailError = "Invalid email format. Example: example@domain.com";
            }

            // Phone validation
            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$"); // E.164 format
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

            if (IsEditing)
            {
                _serviceManager.EmployeeService.UpdateEmployee(SelectedEmployee);
            }
            else
            {
                _serviceManager.EmployeeService.AddEmployee(SelectedEmployee);
            }
            IsAddEditPopupOpen = false;
            LoadEmployees(); // Refresh the list
        }

        private void Cancel(object parameter)
        {
            IsAddEditPopupOpen = false;
            IsDeleteConfirmationOpen = false;
            SelectedEmployee = null;
        }

        private void OpenDeleteConfirmation(object parameter)
        {
            if (parameter is Employee employeeToDelete)
            {
                SelectedEmployee = employeeToDelete;
                IsDeleteConfirmationOpen = true;
            }
        }

        private void ConfirmDelete(object parameter)
        {
            if (SelectedEmployee != null)
            {
                _serviceManager.EmployeeService.DeleteEmployee(SelectedEmployee.Id);
                LoadEmployees(); // Refresh the list
            }
            IsDeleteConfirmationOpen = false;
            SelectedEmployee = null;
        }

        private bool CanEditOrDeleteEmployee(object parameter) => parameter is Employee;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}