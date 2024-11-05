using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GardenGroup.StartupHelpers;
using GardenGroup.Views;
using Model;
using MongoDB.Bson;
using Service;

namespace GardenGroup.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public Employee CurrentEmployee { get; set; }

    private readonly IViewModelFactory _viewModelFactory;
    private object _currentView;
    private bool _isSidebarVisible;
    private readonly IServiceManager _serviceManager;

    public MainViewModel(IViewModelFactory viewModelFactory, IServiceManager serviceManager)
    {
        _viewModelFactory = viewModelFactory;
        IsSidebarVisible = false;
        _serviceManager = serviceManager;

        CurrentView = _viewModelFactory.CreateViewModel<LoginViewModel, MainViewModel>(this);
    }

    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));

            IsSidebarVisible = value is not (LoginViewModel or PasswordResetViewModel);
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
    public void SwitchToLogout() => CurrentView = _viewModelFactory.CreateViewModel<LoginViewModel>();

    public void SwitchToResetPassword() => CurrentView = _viewModelFactory.CreateViewModel<PasswordResetViewModel>();

    public void SwitchToDashboard() => CurrentView = _viewModelFactory.CreateViewModel<DashboardViewModel>();
    public void SwitchToManageEmployees() => CurrentView = _viewModelFactory.CreateViewModel<ManageEmployeesViewModel>();

    public void SwitchToTickets()
    {
        if (CurrentEmployee == null)
        {
            throw new InvalidOperationException("CurrentEmployee is not set.");
        }

        if (CurrentEmployee.UserType == Privilieges.ServiceDesk)
        {
            // Load TicketViewModel for service desk users
            CurrentView = _viewModelFactory.CreateViewModel<TicketViewModel>();
        }
        else
        {
            // Load MyTicketsViewModel for regular users
            var myTicketsViewModel = new MyTicketsViewModel(_serviceManager, this);
            var myTicketsView = new MyTickets(_serviceManager, this)
            {
                DataContext = myTicketsViewModel
            };
            CurrentView = myTicketsView;
        }
    }
    
    public void SwitchToLookupTicket(ObjectId ticketId) => 
        CurrentView = _viewModelFactory.CreateViewModelWithParameter<LookupTicketViewModel, ObjectId>(ticketId);

    #endregion
}