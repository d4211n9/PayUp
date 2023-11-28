using System.ComponentModel.DataAnnotations;

namespace api.models;

public enum NotificationType 
{
    GroupInvite,
 
}

public class NotificationDto
{
    public Enum Category { get; set; }
    
    [Required]
    public string Subject { get; set; }
    [Required]
    public string Body { get; set; }//todo shoul be max 50 chars
    [Required]
    public string Footer { get; set; }
    [Required]
    public DateTime InviteReceived { get; set; }
}


public class GroupInviteNotification
{
    [Required]
    public int GroupId { get; set; }
    [Required]
    public string GroupName { get; set; }
    [Required]
    public string GroupDescription { get; set; }//todo shoul be max 50 chars
    [Required]
    public string SenderFullName { get; set; }
    [Required]
    public DateTime InviteReceived { get; set; }
}