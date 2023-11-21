using System.Data.SqlTypes;
using System.Security.Authentication;
using api.models;
using infrastructure.dataModels;
using infrastructure.repository;
using Microsoft.Extensions.Logging;
using service.services.Password;

namespace service.services;

public class AccountService
{
    private readonly ILogger<AccountService> _logger;

    private readonly PasswordHashRepository _passwordHashRepository;
    private readonly UserRepository _userRepository;

    public AccountService(UserRepository userRepository,
        ILogger<AccountService> logger,
        PasswordHashRepository passwordHashRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordHashRepository = passwordHashRepository;
    }

    public User Authenticate(LoginModel model)
    {
        var passwordHash =
            _passwordHashRepository.GetById(model.Email); //gets the hash from database and authenticates it  
        if (ReferenceEquals(passwordHash, null)) throw new KeyNotFoundException();

        var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
        var isValid = hashAlgorithm.VerifyHashedPassword(model.Password, passwordHash.Hash, passwordHash.Salt);

        if (!isValid) throw new InvalidCredentialException("Invalid credential!");
        
        var user = _userRepository.GetByEmail(model.Email);
        if (ReferenceEquals(user, null)) throw new KeyNotFoundException();
        return user;

    }

    public User Register(RegisterModel model)//todo should have a check for if email already exists
    {
        var user = _userRepository.Create(model, DateTime.Now); //creates the user 
        if (ReferenceEquals(user, null)) throw new SqlTypeException(" Create user");

        var hashAlgorithm = PasswordHashAlgorithm.Create(); //chooses hashing algorithm and hashes password
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(model.Password, salt);
        var password = new PasswordHash
        {
            Id = user.Id,
            Hash = hash,
            Salt = salt,
            Algorithm = hashAlgorithm.GetName()
        };

        var isCreated = _passwordHashRepository.Create(password); //stores the password
        if (isCreated == false) throw new SqlTypeException(" Create user");
        return user; 
    }
    
}