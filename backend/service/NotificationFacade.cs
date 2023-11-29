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
        var invitationMessage = "Yuu Have been invited to: \n" + 
                                invitation.Name +
                                "\n \n with this description:\n" +
                                invitation.Description + 
                                "\n http://localhost:4200/";

        var message = new MimeMessage();
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
        _mailRepository.SendInviteEmail(message,invite , email);
        

        return true;

    }
}
