using api.models;

namespace service;

public class NotificationFacade
{
    public static NotificationDto CreateInviteNotification(GroupInviteNotification invitation)
    {
        NotificationDto result = new()
        {
            Subject =  "you have been invited to: " + invitation.GroupName,
            Body = invitation.GroupDescription,
            Footer = invitation.GroupId.ToString(),
            InviteReceived = invitation.InviteReceived,
            Category = NotificationType.GroupInvite
        };
        return result;
    }
}
