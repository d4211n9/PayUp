namespace service.services;

public class AccountService
{
    public User Register(RegisterCommandModel model)
    {
        var hashAlgorithm = PasswordHashAlgorithm.Create();
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(model.Password, salt);
        var user = _userRepository.Create(model.FullName, model.Email, model.AvatarUrl);
        _passwordHashRepository.Create(user.Id, hash, salt, hashAlgorithm.GetName());
        return user;
    }
    
}