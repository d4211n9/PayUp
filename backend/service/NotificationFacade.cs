using api.models;
using infrastructure.repository;
using MimeKit;

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
        var message = new MimeMessage();
        message.Subject = "invitation to a new PayUp Group";

        message.Body = new TextPart("html")
        {
            Text = @"
            <body>
            <h1>You have been invited to</h1>
            <h1>" + invitation.Name + @"</h1>
            <h3>" +invitation.Description+ @"</p>
            <a> " + "http://localhost:4200/" + @"
            </body>
            </html>"
        };//todo should replace local link with our baseurl before deploying
        
        string invite = "invite";
        _mailRepository.SendInviteEmail(message, email);
        return true;

    }
}
