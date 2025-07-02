using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface IFlightRepository
    {
        Task CreateAsync(Flight flight);
        Task<Flight?> GetByIdAsync(Guid id);
        Task<Flight?> GetByDepartureDateAsync(DateTime departureDate);
    }
}
