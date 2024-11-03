using Service;

namespace GardenGroup.ViewModels;

public class PasswordResetViewModel(IServiceManager service, MainViewModel viewModel)
{
    public IServiceManager ServiceManager
    {
        get => service;
        private set => value = service;
    }
    
    public void SwitchToLogin() => viewModel.SwitchToLogin();
}