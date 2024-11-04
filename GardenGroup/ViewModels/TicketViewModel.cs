using Service;

namespace GardenGroup.ViewModels;

public class TicketViewModel(IServiceManager service, MainViewModel viewModel)
{
    public IServiceManager ServiceManager => service;
}