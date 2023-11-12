using api.models;
using infrastructure.dataModels;
using infrastructure.repository;
using Service;

namespace service.services;

public class AccountService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHashRepository _passwordHashRepository;
    public AccountService(UserRepository userRepository, PasswordHashRepository passwordHashRepository)
    {
        _userRepository = userRepository;
        _passwordHashRepository = passwordHashRepository;
    }
    
    public User Register(RegisterModel model)
    {
        var hashAlgorithm = PasswordHashAlgorithm.Create();
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(model.Password, salt);
        
        //todo should create our user in database
        var user = _userRepository.Create(model.Email, model.FullName, model.PhoneNumber, model.ProfileUrl);
       // _passwordHashRepository.Create(user.Email, hash, salt, hashAlgorithm.GetName());
        return user;
    }
    
}