using Microsoft.Extensions.Options;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Externals;
using SisandAirlines.Infra.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using static SisandAirlines.Infra.Externals.EmailSettings;

namespace SisandAirlines.Infra.Externals
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailService(IOptions<SmtpSettings> options)
        {
            var config = options.Value;

            _fromAddress = config.FromAddress;

            _smtpClient = new SmtpClient(config.Host, config.Port)
            {
                Credentials = new NetworkCredential(config.Username, config.Password),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(Customer customer, List<Ticket> tickets)
        {
            var body = BuildBoardingPassHtml(customer, tickets);

            var mail = new MailMessage(_fromAddress, customer.Email, "Cartão de embarque da Sisand Airlines", body)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mail);
        }

        private string BuildBoardingPassHtml(Customer customer, List<Ticket> tickets)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html><body style='font-family:Arial, sans-serif;'>");
            sb.AppendLine("<h2 style='color:#2E86C1;'>Sisand Airlines - Boarding Pass</h2>");
            sb.AppendLine($"<p><strong>Passenger:</strong> {customer.FullName}</p>");
            sb.AppendLine("<br/>");

            foreach (var ticket in tickets)
            {
                if (ticket.Flight == null)
                    throw new InvalidOperationException($"O ticket {ticket.Id} está sem os dados de voo");

                if (ticket.Seat == null)
                    throw new InvalidOperationException($"O Ticket {ticket.Id} não possui dados do voo.");

                sb.AppendLine("<div style='margin-bottom:20px; padding:10px; border:1px solid #ddd; border-radius:5px;'>");
                sb.AppendLine($"<p><strong>Flight:</strong> {ticket.Flight.Origin} → {ticket.Flight.Destination}</p>");
                sb.AppendLine($"<p><strong>Departure:</strong> {ticket.Flight.DepartureDate:dd/MM/yyyy HH:mm}</p>");
                sb.AppendLine($"<p><strong>Arrival:</strong> {ticket.Flight.ArrivalDate:dd/MM/yyyy HH:mm}</p>");
                sb.AppendLine($"<p><strong>Seat:</strong> {ticket.Seat.SeatType} {ticket.Seat.Number}</p>");
                sb.AppendLine($"<p><strong>Confirmation Code:</strong> <span style='color:green;'>{ticket.ConfirmationCode}</span></p>");
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<hr style='margin-top:30px;'/>");
            sb.AppendLine("<p style='font-size:0.9em; color:gray;'>Thank you for flying with Sisand Airlines.</p>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

    }
}
