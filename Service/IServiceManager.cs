﻿namespace Service;

public interface IServiceManager
{
    EmployeeService EmployeeService { get; }
    TicketService TicketService { get; }
    PasswordService PasswordService { get; }
}