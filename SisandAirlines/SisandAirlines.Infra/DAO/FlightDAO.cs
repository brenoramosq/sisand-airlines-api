using Dapper;
using Npgsql;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Infra.DAO.Interfaces;
using SisandAirlines.Infra.Models;

namespace SisandAirlines.Infra.DAO
{
    public class FlightDAO : IFlightDAO
    {
        private readonly string _connectionString;

        public FlightDAO(string connectionString)
        {
            _connectionString = connectionString;   
        }

        public async Task<IEnumerable<AvailableFlightDTO>> GetAvailableFlightsAsync
        (
            string origin,
            string destination,
            int numberPassengers,
            SeatType seatType,
            DateTime date,
            TimeSpan startTime
        )
        {
            var query =
            @"
                SELECT 
                    f.id as flightId,
                    f.code as flightCode,
                    f.origin,
                    f.destination,
                    f.departure_date as departureDate,
                    f.start_time as StartTime,
                    s.seat_type as seatType,
                    ARRAY_AGG(s.""number"" ORDER BY s.""number"") AS availableSeats
                FROM 
                    flight f
                JOIN 
                    seat s ON s.flight_id = f.id
                WHERE 
                    f.origin = @origin
                    AND f.destination = @destination
                    AND f.departure_date = @date
                    AND f.start_time >= @startTime
                    AND s.is_reserved = false
                    AND s.seat_type = @seatType
                GROUP BY 
                    f.id,
                    f.code,
                    f.origin,
                    f.destination,
                    f.departure_date,
                    f.start_time,
                   s.seat_type
            ";

            var parameters = new
            {
                origin,
                destination,
                date,
                seatType,
                numberPassengers,
                startTime
            };

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryAsync<AvailableFlightDTO>(query, parameters);
        }
    }
}
