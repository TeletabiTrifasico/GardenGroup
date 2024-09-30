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

        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoginCommand = new RelayCommand(OnLogin);
        }

        public ICommand LoginCommand { get; }

        private void OnLogin(object parameter)
        {
            _mainViewModel.SwitchToDashboard();
        }

        public bool ValidateLogin(string username, string password)
        {
            return _mainViewModel._serviceManager.EmployeeService.ValidateLogin(username, password);
        }
    }
}
