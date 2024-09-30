using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Service;

namespace GardenGroup.ViewModels
{
    public class LoginViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IServiceManager _serviceManager;
        
        public ICommand LoginCommand { get; }
        
        public LoginViewModel(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _serviceManager = serviceManager;
            
            LoginCommand = new RelayCommand(OnLogin);
        }

        private void OnLogin(object parameter) => _mainViewModel.SwitchToDashboard();

        public bool ValidateLogin(string username, string password) =>
            _serviceManager.EmployeeService.ValidateLogin(username, password);
    }
}
