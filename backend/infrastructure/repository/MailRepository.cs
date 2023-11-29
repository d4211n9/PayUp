using MimeKit;

namespace infrastructure.repository;

public class MailRepository
{
    public void SendInviteEmail(string message1, string reciever)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("The Webshop Inc.", Environment.GetEnvironmentVariable("fromemail")));
        message.To.Add(new MailboxAddress("Customer", reciever));
        message.Subject = "Your order confirmation";

        message.Body = new TextPart("plain")
        {
            Text = @"Total order price: "+ " det virker" 
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