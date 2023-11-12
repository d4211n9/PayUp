
using System.Security.Authentication;
using infrastructure.dataModels;
using infrastructure.repository;
using service.services.Password;

namespace service.services;

public class AccountService
{

    private readonly PasswordHashRepository _passwordHashRepository;
    private readonly UserRepository _userRepository;

    public AccountService(UserRepository userRepository,
        PasswordHashRepository passwordHashRepository)
    {
 
        _userRepository = userRepository;
        _passwordHashRepository = passwordHashRepository;
    }

    public User? Authenticate(string email, string password)
    {
        try
        {
            var passwordHash = _passwordHashRepository.GetByEmail(email);
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
            var isValid = hashAlgorithm.VerifyHashedPassword(password, passwordHash.Hash, passwordHash.Salt);
            if (isValid) return _userRepository.GetById(passwordHash.UserId);
        }
        catch (Exception e)
        {
            //_logger.LogError("Authenticate error: {Message}", e);
        }

        throw new InvalidCredentialException("Invalid credential!");
    }

    public User Register(string fullName, string email, string password, string phone, string profileUrl)
    {
        var hashAlgorithm = PasswordHashAlgorithm.Create();
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(password, salt);
        var user = _userRepository.Create(fullName, email, phone, DateTime.Now, profileUrl);
        //_passwordHashRepository.Create(user.Email, hash, salt, hashAlgorithm.GetName());
        return new User
        {
            Email = null,
            FullName = null,
            PhoneNumber = null,
            Created = default,
            ProfileUrl = null
        };
    }
}