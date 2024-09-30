using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Service;

namespace GardenGroup.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public readonly IServiceManager _serviceManager;
        private object _currentView;

        public MainViewModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            // Initialize with the Login view
            CurrentView = new LoginViewModel(this);
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public void SwitchToDashboard()
        {
            // Switch to the Dashboard view
            CurrentView = new DashboardViewModel(_serviceManager);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
