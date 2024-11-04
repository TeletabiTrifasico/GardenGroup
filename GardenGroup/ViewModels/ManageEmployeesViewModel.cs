using System.Collections.ObjectModel;
using System.Windows.Input;
using Model;
using Service;

namespace GardenGroup.ViewModels
{
    public class ManageEmployeesViewModel
    {
        private readonly IServiceManager _serviceManager;

        public ObservableCollection<Employee> Employees { get; }

        // Commands for CRUD operations
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }

        public ManageEmployeesViewModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;

            // Load employees from the service
            Employees = new ObservableCollection<Employee>(_serviceManager.EmployeeService.GetEmployees());

            // Initialize commands with respective methods
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            EditEmployeeCommand = new RelayCommand(EditEmployee, CanEditOrDeleteEmployee);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee, CanEditOrDeleteEmployee);
        }

        private void AddEmployee(object parameter)
        {
            var newEmployee = new Employee
            {
                Firstname = "New",
                Lastname = "Employee",
                Email = "new.employee@example.com",
                Phone = "000-000-0000",
                UserType = Privilieges.NormalUser
            };

            _serviceManager.EmployeeService.AddEmployee(newEmployee);
            Employees.Add(newEmployee);
        }

        private void EditEmployee(object parameter)
        {
            if (parameter is Employee employeeToEdit)
            {
                _serviceManager.EmployeeService.UpdateEmployee(employeeToEdit);
            }
        }

        private void DeleteEmployee(object parameter)
        {
            if (parameter is Employee employeeToDelete)
            {
                _serviceManager.EmployeeService.DeleteEmployee(employeeToDelete.Id);
                Employees.Remove(employeeToDelete);
            }
        }

        private bool CanEditOrDeleteEmployee(object parameter) => parameter is Employee;
    }
}
