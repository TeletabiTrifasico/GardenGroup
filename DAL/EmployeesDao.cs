using DAL.Extensions;
using Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class EmployeesDao : BaseDao
{
    public Employee? Login(string username, string password)
    {
        //searches employee by username
        var employee = GetCollection<Employee>("Employees")
            .Find(x => x.Username == username)
            .FirstOrDefault();
        
        // Encryption.Verify cannot be implemented into Mongo's Linq query because it is not supported
        // The if statement had to be implemented to avoid exceptions. 
        if (employee != null && Encryption.Verify(password, employee.Password))
            return employee;
        
        return null;
    }
    
    public List<Employee> GetEmployees() => 
        GetCollection<Employee>("Employees")
            .AsQueryable()
            .ToList();
    
    public Employee? GetEmployeeByEmail(string email) => 
        GetCollection<Employee>("Employees")
            .Find(e => e.Email == email)
            .FirstOrDefault();

    public void ChangePassword(ObjectId employeeId, string hash)
    {
        var filter = FilterEq<Employee, ObjectId>("Id", employeeId);
        var update = Builders<Employee>.Update
            .Set(e => e.Password, hash);
        
        var result = GetCollection<Employee>("Employees").UpdateOne(filter, update);
        if(result.ModifiedCount == 0)
            throw new Exception("Failed to update employee's password.");
    }
}