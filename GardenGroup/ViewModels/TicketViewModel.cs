using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenGroup.ViewModels;

public class TicketViewModel(IServiceManager service, MainViewModel viewModel)
{
    public IServiceManager ServiceManager => service;
}