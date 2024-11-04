using Service;

namespace GardenGroup.ViewModels;

public class PasswordResetViewModel(IServiceManager service, MainViewModel viewModel)
{
    public IServiceManager ServiceManager => service;
}