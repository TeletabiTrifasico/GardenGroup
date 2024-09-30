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
        private IServiceManager _serviceManager;
        public LoginViewModel(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _serviceManager = serviceManager;
            
            LoginCommand = new RelayCommand(OnLogin);
        }

        public ICommand LoginCommand { get; }

        private void OnLogin(object parameter)
        {
            _mainViewModel.SwitchToDashboard();
        }

        public bool ValidateLogin(string username, string password)
        {
            return _serviceManager.EmployeeService.ValidateLogin(username, password);
        }
    }
}
