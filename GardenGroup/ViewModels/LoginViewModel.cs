using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using Service;

namespace GardenGroup.ViewModels
{
    public class LoginViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IServiceManager _serviceManager;
        
        public ICommand LoginCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        
        public LoginViewModel(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _serviceManager = serviceManager;
            
            LoginCommand = new RelayCommand(OnLogin);
            ResetPasswordCommand = new RelayCommand(PasswordReset);
        }

        private void OnLogin(object parameter) => _mainViewModel.SwitchToDashboard();

        private void PasswordReset(object parameter) => _mainViewModel.SwitchToResetPassword();

        public Employee? Login(string username, string password) =>
            _serviceManager.EmployeeService.Login(username, password);

        public void SetLoggedInEmployee(Employee employee) => 
            _mainViewModel.CurrentEmployee = employee;
    }
}
