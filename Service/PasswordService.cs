using DAL;
using Model;
using MongoDB.Bson;

namespace Service;

public class PasswordService
{

    private readonly PasswordDao _passwordDao = new();
    
    public void PasswordReset(PasswordReset passwordReset) => _passwordDao.InsertPasswordReset(passwordReset);
    
    public bool VerifyPasswordReset(ObjectId userId, string pin) => _passwordDao.VerifyPasswordReset(userId, pin);
    
    public void DeletePasswordReset(ObjectId userId) => _passwordDao.DeletePasswordReset(userId);
}