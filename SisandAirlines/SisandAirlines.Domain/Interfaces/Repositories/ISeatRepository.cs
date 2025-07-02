using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetSeatsByFlightIdAsync(Guid flightId);
        Task<IEnumerable<Seat>> GetAvailableSeatsByTypeAsync(Guid flightId, int seatType);
        Task CreateAsync(Seat seat);
        Task ReserveSeatsAsync(List<Guid> seatIds);
    }
}
