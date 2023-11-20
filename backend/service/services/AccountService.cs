
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

    private readonly PasswordHashRepository _passwordHashRepository;
    private readonly ILogger<AccountService> _logger;
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
        try
        {
            var passwordHash = _passwordHashRepository.GetById(model.Email); //gets the hash from database and authenticates it  
            if (ReferenceEquals(passwordHash, null)) throw new KeyNotFoundException("Invalid credential");
            
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
            var isValid = hashAlgorithm.VerifyHashedPassword(model.Password, passwordHash.Hash, passwordHash.Salt);

            if (isValid)
            {
               var user = _userRepository.GetByEmail(model.Email);
               if (ReferenceEquals(user, null)) throw new KeyNotFoundException("Could not load user");
               
               return user;
            }
        }
        catch (KeyNotFoundException k)
        {
            _logger.LogError("Authenticate error: {Message}", k);
            throw new Exception(k.Message);
        }
        catch (Exception e)
        {    
            _logger.LogError("Authenticate error: {Message}", e);
            throw new Exception("Could not Authenticate User");
        }
        throw new InvalidCredentialException("Invalid credential!");
    }

    public User Register(RegisterModel model)
    {
        try
        {
            var user = _userRepository.Create(model, DateTime.Now); //creates the user 
            if (ReferenceEquals(user, null)) throw new SqlTypeException("Could not Create user");

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

            var isCreated =_passwordHashRepository.Create(password); //stores the password
                if (isCreated == false) throw new SqlTypeException("Could not create user");
                return user;
        }
        catch (SqlTypeException e)
        {
            _logger.LogError("Register error: {Message}", e);
            throw new Exception(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("Register error: {Message}", e);
            throw new Exception("Could not Register User");
        }
    }
}