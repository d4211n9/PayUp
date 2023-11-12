using infrastructure.dataModels;

namespace infrastructure.repository;


public class PasswordHashRepository
{

    public PasswordHashRepository()
    {
    }

    public PasswordHash GetByEmail(string email)
    {
        throw new NotImplementedException();
    }
    
    public void Create(string userId, string hash, string salt, string algorithm)
    {
        throw new NotImplementedException();
    }

    public void Update(string userId, string hash, string salt, string algorithm)
    {
        throw new NotImplementedException();
    }
}