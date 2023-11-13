using System.ComponentModel.DataAnnotations;

namespace api.models;

public class RegisterModel
{
    [Required] public required string Email { get; set; }
    
    [Required] public required string FullName { get; set; }

    [Required] [MinLength(8)] public required string Password { get; set; }
    
    [Required] public required string PhoneNumber { get; set; }

    [Required] public required string ProfileUrl { get; set; }
}