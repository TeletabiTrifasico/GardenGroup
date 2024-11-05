using DAL;
using Model;
using MongoDB.Bson;

namespace Service;

public class EmployeeService
{
    private readonly EmployeesDao _employeesDao = new();

    public Employee? Login(string username, string password) => 
        _employeesDao.Login(username, password);
    
    public List<Employee> GetEmployees() => 
        _employeesDao.GetEmployees();
    
    public Employee? GetEmployeeByEmail(string email) => _employeesDao.GetEmployeeByEmail(email);
    
    public void ChangePassword(ObjectId employeeId, string hash) => _employeesDao.ChangePassword(employeeId, hash);
    public void AddEmployee(Employee employee) =>
            _employeesDao.AddEmployee(employee);

    public void UpdateEmployee(Employee employee) =>
        _employeesDao.UpdateEmployee(employee);

    public void DeleteEmployee(ObjectId employeeId) =>
        _employeesDao.DeleteEmployee(employeeId);
}