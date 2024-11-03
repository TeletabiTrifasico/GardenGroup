using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Service.Mail;

public class Smtp(string smtpUser, string smtpPassword)
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;

    public bool SendEmail(string toEmail, string pin)
    {
        // Create a new email message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender Name", smtpUser));
        message.To.Add(new MailboxAddress("Recipient Name", toEmail));
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
            client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            
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