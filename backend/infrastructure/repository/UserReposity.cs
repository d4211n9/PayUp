using infrastructure.dataModels;

namespace infrastructure.repository;

public class UserRepository
{
    public User Create(string email, string fullName, string phoneNumber, string profileUrl)
    {
        //throw new NotImplementedException("create in user respository is not implemented yet ");
        var user = new User
        {
            Email = email,
            FullName = fullName,
            PhoneNumber = phoneNumber,
            Created = DateTime.Now,
            ProfileUrl = profileUrl
        };

        Console.Write("You created a user");
        Console.Write("email:  " + user.Email);
        return user;
    }
}