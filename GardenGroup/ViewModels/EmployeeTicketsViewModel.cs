using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenGroup.ViewModels;

// Tickets view model for employees that are not in a service desk
public class EmployeeTicketsViewModel(IServiceManager service, MainViewModel viewModel)
{
    public IServiceManager ServiceManager
    {
        get => service;
        private set => value = service;
    }
}

