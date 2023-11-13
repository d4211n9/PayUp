namespace infrastructure.dataModels;

public class PasswordHash
{
    public required string Email { get; set; }
    public required string Hash { get; set; }
    public required string Salt { get; set; }
    public required string Algorithm { get; set; }
}