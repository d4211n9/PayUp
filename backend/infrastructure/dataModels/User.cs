using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace infrastructure.dataModels;

public class User
{
    public required int Id { get; set; }
    [Required, EmailAddress] public required string Email { get; set; }
    [Required] public required string FullName { get; set; }
    [Required, Phone] public required string PhoneNumber { get; set; }
    [Required, Timestamp] public required DateTime Created { get; set; }
    [Required, Url] public required string ProfileUrl { get; set; }
}