using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface ITicketRepository
    {        
        Task CreateAsync(Ticket ticket);
    }
}
