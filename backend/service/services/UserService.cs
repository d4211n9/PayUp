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
        if (ReferenceEquals(user, null))
            throw new SqlNullValueException("Could not find user");
        return user;
    }

    public User EditProfileInfo(SessionData? data, UserInfoDto user)
    {
        if (data.UserId != user.Id) //checks login user is equal to editUserObject.
            throw new SecurityException("You are not Authorised for this action");
        var responseUser = _userRepository.EditUserInfo(user);
        if (ReferenceEquals(responseUser, null)) throw new SqlTypeException("Could not Edit user");//checks if response user is null before returning it.
        return responseUser; 
    }


    public bool DeleteAccount(SessionData? data, int userId)
    {
        if (data.UserId != userId) //checks login user is equal to editUserObject.
            throw new SecurityException("You are not Authorised for this action");
        var wasDeleted = _userRepository.DeleteUser(userId);
        if (!wasDeleted) throw new SqlTypeException("Could not Edit user");//checks if response is true before returning it.
        return wasDeleted; 
    }
}