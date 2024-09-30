using DAL;

namespace Service;

public class EmployeeService
{
    private readonly EmployeesDao _employeesDao;
    public EmployeeService()
    {
        _employeesDao = new EmployeesDao();
    }

    public bool ValidateLogin(string username, string password) => 
        _employeesDao.ValidateLogin(username, password);
}