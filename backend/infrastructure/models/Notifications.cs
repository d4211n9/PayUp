using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace api.models;

public enum NotificationCategory
{
    GroupInvite = 1
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

public class GroupInviteDto
{
    [Required]
    public bool Accepted  { get; set; }
    [Required]
    public int GroupId{ get; set; }
    
}

public class NotificationSettingsDto
{
    public int UserId { get; set; }
    public bool InviteNotification { get; set; }
    public bool InviteNotificationEmail { get; set; }
    public bool ExpenseNotification { get; set; }
    public bool ExpenseNotificationEmail { get; set; }
}
