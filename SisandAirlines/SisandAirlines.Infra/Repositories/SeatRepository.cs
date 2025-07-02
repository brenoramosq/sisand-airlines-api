using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;
using System.Data.Common;

namespace SisandAirlines.Infra.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public SeatRepository(string connectionString, IUnitOfWork unitOfWork)
        {
            _connectionString = connectionString;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Seat>> GetSeatsByFlightIdAsync(Guid flightId)
        {
            var query =
            @"
                      SELECT 
                        id,
                        flight_id as flightId,
                        number,
                        seat_type as seatType,
                        is_reserved as isReserved
                    FROM 
                        seat
                    WHERE 
                        flight_id = @flight_id
                    ORDER BY 
                        number;
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryAsync<Seat>(query, new { flight_id = flightId });           
        }

        public async Task<IEnumerable<Seat>> GetAvailableSeatsByTypeAsync(Guid flightId, int seatType)
        {
            var query =
            @"
                SELECT 
                    id,
                    number,
                    seat_type,
                    is_reserved
                FROM 
                    seat 
                WHERE 
                    flight_id = @flightId 
                    AND seat_type = @seatType 
                    AND is_reserved = false
                ORDER BY number
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryAsync<Seat>(query, new { flightId, seatType });            
        }

        public async Task CreateAsync(Seat seat)
        {
            var query =
            @"
                INSERT INTO seat (id, flight_id, number, seat_type, is_reserved)
                VALUES (@id, @flightId, @number, @seatType, @isReserved)
            ";

            await _unitOfWork.Connection.ExecuteAsync(query, new
            {
                id = seat.Id,
                flightId = seat.FlightId,
                number = seat.Number,
                seatType = seat.SeatType,
                isReserved = seat.IsReserved
            }, _unitOfWork.Transaction);
        }

        public async Task CreateAsync(Airplane airplane)
        {
            var query =
            @"
                INSERT INTO airplane (id, model, code)
                VALUES (@id, @model, @code)
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new
            {
                id = airplane.Id,
                model = airplane.Model,
                code = airplane.Code
            });
        }

        public async Task ReserveSeatsAsync(List<Guid> seatIds)
        {
            var query = @"
                UPDATE 
                    seat 
                SET 
                    is_reserved = true 
                WHERE 
                    id = ANY(@seatIds)";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new { seatIds });
        }

    }
}
