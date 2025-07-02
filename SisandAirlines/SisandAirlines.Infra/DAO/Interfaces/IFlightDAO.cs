using SisandAirlines.Domain.Enums;
using SisandAirlines.Infra.Models;

namespace SisandAirlines.Infra.DAO.Interfaces
{
    public interface IFlightDAO
    {
        Task<IEnumerable<AvailableFlightDTO>> GetAvailableFlightsAsync
        (
            string origin,
            string destination,
            int numberPassengers,
            SeatType seatType,
            DateTime date,
            TimeSpan startTime
        );
    }
}
