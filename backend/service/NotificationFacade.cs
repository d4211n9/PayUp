using api.models;
using infrastructure.repository;

namespace service;

public class NotificationFacade
{
    private MailRepository _mailRepository;
    public NotificationFacade(MailRepository mailRepository)
    {
        _mailRepository = mailRepository;
    }
    
    public static NotificationDto CreateInviteNotification(GroupInviteNotification invitation)
    {
        NotificationDto result = new()
        {
            Subject =  "you have been invited to: " + invitation.GroupName,
            Body = invitation.GroupDescription,
            Footer = invitation.GroupId.ToString(),
            InviteReceived = invitation.InviteReceived,
            Category = NotificationCategory.GroupInvite
        };
        return result;
    }

    public bool SendInviteEmail(Group invitation, string email)
    {
        var invitationMessage = "Yuu Have been invited to: \n" + 
                                invitation.Name +
                                "\n \n with this description:\n" +
                                invitation.Description + 
                                "\nhttp://localhost:4200/";

        string invite = "invite";
        _mailRepository.SendInviteEmail(invitationMessage,invite , email);
        return true;

    }
}
