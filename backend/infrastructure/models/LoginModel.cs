namespace api.models;

public class LoginModel
{
    public string Email { get; set; }//todo create checks for both variables (look in Register model)
    public string Password { get; set; }
}