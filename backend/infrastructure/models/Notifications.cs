using System.ComponentModel.DataAnnotations;

namespace api.models;

public class GroupInviteNotification
{
    [Required]
    public int GroupId { get; set; }
    [Required]
    public string GroupName { get; set; }
    [Required]
    public string GroupDescription { get; set; }
    [Required, Url]
    public string GroupImage { get; set; }
    [Required]
    public int SenderId { get; set; }
    [Required, EmailAddress]
    public string SenderEmail { get; set; }
    [Required]
    public string SenderFullName { get; set; }
    [Required, Url]
    public string SenderProfileImage { get; set; }
    [Required]
    public DateTime InviteReceived { get; set; }
}