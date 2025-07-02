using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Externals
{
    public interface IEmailService
    {
        Task SendEmailAsync(Customer customer, List<Ticket> tickets);
    }
}
