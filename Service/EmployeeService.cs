using DAL;
using Model;

namespace Service;

public class EmployeeService
{
    private readonly EmployeesDao _employeesDao;
    public EmployeeService()
    {
        _employeesDao = new EmployeesDao();
    }

    public Employee Login(string username, string password) => 
        _employeesDao.Login(username, password);
    
    public List<Employee> GetEmployees() => 
        _employeesDao.GetEmployees();
}