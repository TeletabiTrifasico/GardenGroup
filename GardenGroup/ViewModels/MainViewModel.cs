using System.ComponentModel;
using GardenGroup.StartupHelpers;
using Service;

namespace GardenGroup.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IServiceManager _serviceManager;
        private object _currentView;

        public MainViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            //_serviceManager = serviceManager;
            
            // Initialize with the Login view
            //CurrentView = new LoginViewModel(this);
            CurrentView = _viewModelFactory.CreateViewModel<LoginViewModel, MainViewModel>(this);
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
            //CurrentView = new DashboardViewModel(_serviceManager);
            CurrentView = _viewModelFactory.CreateViewModel<DashboardViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
