using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Service.Mail;

public class Smtp(string smtpUser, string smtpPassword)
{
    private const string SmtpServer = "smtp.gmail.com";
    private const int SmtpPort = 587;

    public bool SendEmail(string toEmail, string recipientName, string pin)
    {
        // Create a new email message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Garden Group", smtpUser));
        message.To.Add(new MailboxAddress(recipientName, toEmail));
        message.Subject = "Garden Group Password Reset";
        
        var body = $"""
                   Hello,
                   
                   Your password reset PIN code is {pin}
                   
                   Best regards,
                   Garden Group
                   """;
        
        message.Body = new TextPart("plain") { Text = body };
        
        using var client = new SmtpClient();
        try
        {
            client.Connect(SmtpServer, SmtpPort, SecureSocketOptions.StartTls);
            
            client.Authenticate(smtpUser, smtpPassword);
            
            client.Send(message);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
        finally
        {
            client.Disconnect(true);
        }
        
        return false;
    }
}