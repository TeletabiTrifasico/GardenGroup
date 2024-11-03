using Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class PasswordDao : BaseDao
{
    public void InsertPasswordReset(PasswordReset passwordReset) => 
        GetCollection<PasswordReset>("PasswordReset").InsertOne(passwordReset);
    
    public bool VerifyPasswordReset(ObjectId userId, string pin) =>
        GetCollection<PasswordReset>("PasswordReset").Find(x => x.UserId == userId && x.Pin == pin).Any();
    
    public void DeletePasswordReset(ObjectId userId) => GetCollection<PasswordReset>("PasswordReset").DeleteOne(x => x.UserId == userId);
}