using Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class EmployeesDao : MongoCRUD
{
    // Basic login system. Encryption needs to be added
    public bool ValidateLogin(string username, string password) =>
        _db.GetCollection<Employee>("Employees")
            .Find(x => x.Username == username && x.Password == password)
            .Any();
}