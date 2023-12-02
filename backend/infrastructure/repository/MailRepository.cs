using MimeKit;

namespace infrastructure.repository;

public class MailRepository
{
    public void SendInviteEmail(MimeMessage message, string reciever)
    {

        message.From.Add(new MailboxAddress("PayUp", "PayUpNotifications"));
        message.To.Add(new MailboxAddress("Customer", reciever));

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("fromemail"), Environment.GetEnvironmentVariable("frompass") );
            client.Send(message);
            client.Disconnect(true);
        }
    }
}