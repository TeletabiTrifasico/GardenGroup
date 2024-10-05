using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GardenGroup.StartupHelpers;
using Model;
using Service;

namespace GardenGroup.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public Employee CurrentEmployee { get; set; }

        private readonly IViewModelFactory _viewModelFactory;
        private object _currentView;
        private bool _isSidebarVisible;

        public MainViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            IsSidebarVisible = false;

            CurrentView = _viewModelFactory.CreateViewModel<LoginViewModel, MainViewModel>(this);
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));

                IsSidebarVisible = !(value is LoginViewModel);
                UpdateAlignments();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Bindings

        public bool IsSidebarVisible
        {
            get => _isSidebarVisible;
            set
            {
                _isSidebarVisible = value;
                OnPropertyChanged();
            }
        }

        private HorizontalAlignment _currentHorizontalAlignment;
        public HorizontalAlignment CurrentHorizontalAlignment
        {
            get => _currentHorizontalAlignment;
            set
            {
                _currentHorizontalAlignment = value;
                OnPropertyChanged();
            }
        }

        private VerticalAlignment _currentVerticalAlignment;
        public VerticalAlignment CurrentVerticalAlignment
        {
            get => _currentVerticalAlignment;
            set
            {
                _currentVerticalAlignment = value;
                OnPropertyChanged();
            }
        }

        private void UpdateAlignments()
        {
            if (CurrentView is LoginViewModel)
            {
                CurrentHorizontalAlignment = HorizontalAlignment.Center;
                CurrentVerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                CurrentHorizontalAlignment = HorizontalAlignment.Left;
                CurrentVerticalAlignment = VerticalAlignment.Top;
            }
        }

        #endregion

        #region Switches

        public void SwitchToDashboard() => CurrentView = _viewModelFactory.CreateViewModel<DashboardViewModel>();

        public void SwitchToTickets() =>
            CurrentView = CurrentEmployee.UserType == Privilieges.ServiceDesk ?
            _viewModelFactory.CreateViewModel<TicketViewModel>() :
            _viewModelFactory.CreateViewModel<EmployeeTicketsViewModel>();

        #endregion
    }
}
