using Model;
using MongoDB.Driver;
using DAL.Extensions;

namespace DAL;

public class EmployeesDao : MongoCRUD
{
    public Employee Login(string username, string password)
    {
        //searches employee by username
        var employee = GetCollection<Employee>("Employees")
            .Find(x => x.Username == username)
            .FirstOrDefault();
        //verify the password
        if (employee != null && BCrypt.Net.BCrypt.Verify(password, employee.Password))
        {
            return employee; //good login :)
        }
        return null; //bad login :(
    }
    
    public List<Employee> GetEmployees() => 
        GetCollection<Employee>("Employees")
            .AsQueryable()
            .ToList();
}