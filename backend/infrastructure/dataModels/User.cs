namespace infrastructure.dataModels;

public class User
{
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateTime Created { get; set; }
    public required string ProfileUrl { get; set; }
}