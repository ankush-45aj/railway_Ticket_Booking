using Microsoft.Extensions.Options;
using RailwayBookingApp.Models;

using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendTicketEmail(string userEmail, string passengerName,
        string trainName, string route, string seats, string price)
    {
        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _settings.SenderEmail,
                _settings.SenderPassword)
        };

        string htmlBody = $@"<h2>Booking Confirmed</h2>
        <p>Hi {passengerName}</p>
        <p>Train: {trainName}</p>
        <p>Route: {route}</p>
        <p>Seats: {seats}</p>
        <p>Total Fare: ₹{price}</p>";

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, "RailOps"),
            Subject = "Ticket Confirmed 🎫",
            Body = htmlBody,
            IsBodyHtml = true
        };

        mailMessage.To.Add(userEmail);

        await client.SendMailAsync(mailMessage);
    }
}
