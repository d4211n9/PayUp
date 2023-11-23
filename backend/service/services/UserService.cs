using System.Data.SqlTypes;
using System.Security;
using infrastructure.dataModels;
using infrastructure.repository;

namespace service.services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepo)
    {
        _userRepository = userRepo;
    }

    public User GetLoggedInUser(SessionData data)
    {
        var user = _userRepository.GetById(data.UserId);
        if (ReferenceEquals(user, null)) throw new SqlNullValueException();
        return user;
    }

    public User EditProfileInfo(SessionData? data, UserInfoDto user)
    {
        var responseUser = _userRepository.EditUserInfo(user, data.UserId);
        if (ReferenceEquals(responseUser, null)) throw new SqlNullValueException("Edit User");//checks if response user is null before returning it.
        return responseUser; 
    }
    
    
    public bool DeleteAccount(SessionData? data)
    {
        var wasDeleted = _userRepository.DeleteUser(data.UserId);
        if (!wasDeleted) throw new SqlNullValueException(" delete user");//checks if response is true before returning it.
        return wasDeleted; 
    }
}