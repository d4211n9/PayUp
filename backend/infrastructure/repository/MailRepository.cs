using MimeKit;

namespace infrastructure.repository;

public class MailRepository
{
    public void SendInviteEmail(string body, string subject, string reciever)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("payUp", "PayUpNotifications"));
        message.To.Add(new MailboxAddress("Customer", reciever));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = @body
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Environment.GetEnvironmentVariable("fromemail"), Environment.GetEnvironmentVariable("frompass") );
            client.Send(message);
            client.Disconnect(true);
        }
    }
}