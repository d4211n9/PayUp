using System.ComponentModel.DataAnnotations;

namespace api.models;

public class RegisterModel
{

    [Required] public required string Email { get; set; }//todo should check that it is a valid mail 
    
    [Required] public required string FullName { get; set; }

    [Required] [MinLength(8)] public required string Password { get; set; }
    
    [Required] public required string PhoneNumber { get; set; }
    
    [Required] public required DateTime Created { get; set; }//todo should check that it is a valid mail 

    [Required] public required string ProfileUrl { get; set; }
}