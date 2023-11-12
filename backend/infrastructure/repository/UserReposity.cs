using infrastructure.dataModels;

namespace infrastructure.repository;

public class UserRepository
{

    public UserRepository()
    {
    }

    public User Create(string fullName, string email, string phone, string profileUrl)
    {
        throw new NotImplementedException();
    }

    public User? GetById(string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAll()
    {
        throw new NotImplementedException();
    }
}