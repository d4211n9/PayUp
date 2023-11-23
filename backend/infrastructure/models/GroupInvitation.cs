using System.ComponentModel.DataAnnotations;

namespace api.models;

public class GroupInvitation
{
    [Required]
    public int ReceiverId { get; set; } 
    [Required]
    public int GroupId { get; set; }
}

public class FullGroupInvitation : GroupInvitation
{
    [Required]
    public int SenderId { get; set; }
}