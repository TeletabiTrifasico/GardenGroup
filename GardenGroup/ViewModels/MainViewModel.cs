using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GardenGroup.StartupHelpers;
using Model;

namespace GardenGroup.ViewModels;

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

        SwitchToLogin();
    }

    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));

            IsSidebarVisible = !(value is LoginViewModel or PasswordResetViewModel);
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
        if (CurrentView is LoginViewModel or PasswordResetViewModel)
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

    public void SwitchToLogin() => CurrentView = _viewModelFactory.CreateViewModel<LoginViewModel, MainViewModel>(this);
    public void SwitchToResetPassword() => CurrentView = _viewModelFactory.CreateViewModel<PasswordResetViewModel>();
    
    public void SwitchToDashboard() => CurrentView = _viewModelFactory.CreateViewModel<DashboardViewModel>();

    public void SwitchToTickets() =>
        CurrentView = CurrentEmployee.UserType == Privilieges.ServiceDesk
            ? _viewModelFactory.CreateViewModel<TicketViewModel>()
            : _viewModelFactory.CreateViewModel<EmployeeTicketsViewModel>();

    #endregion
}