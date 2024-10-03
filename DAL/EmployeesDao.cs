using Model;
using MongoDB.Driver;
using DAL.Extensions;

namespace DAL;

public class EmployeesDao : MongoCRUD
{
    public Employee Login(string username, string password) =>
        GetCollection<Employee>("Employees")
            .Find(x => x.Username == username && x.Password == password)
            .FirstOrDefault();
    
    // Change to this once passwords are encrypted
    
    /*public bool ValidateLogin(string username, string password) =>
        GetCollection<Employee>("Employees")
            .Find(x => x.Username == username && Encryption.Verify(password, x.Password))
            .Any();*/
}