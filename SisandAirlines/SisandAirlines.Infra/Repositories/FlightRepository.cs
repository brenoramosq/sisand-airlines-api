using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;

namespace SisandAirlines.Infra.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public FlightRepository(string connectionString, IUnitOfWork unitOfWork)
        {
            _connectionString = connectionString;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Flight flight)
        {
            var query =
            @"
                INSERT INTO flight (id, departure_date, arrival_date, duration, origin, destination, code, airplane_id, start_time)
                VALUES (@id, @departureDate, @arrivalDate, @duration, @origin, @destination, @code, @airplaneId, @startTime)
            ";

            await _unitOfWork.Connection.ExecuteAsync(query, new
            {
                id = flight.Id,
                departureDate = flight.DepartureDate,
                arrivalDate = flight.ArrivalDate,
                duration = flight.Duration,
                origin = flight.Origin,
                destination = flight.Destination,
                code = flight.Code,
                airplaneId = flight.AirplaneId,
                startTime = flight.StartTime
            }, _unitOfWork.Transaction);
        }

        public async Task<Flight?> GetByIdAsync(Guid id)
        {
            var query =
            @"
                SELECT
                    id,
                    code,
                    departure_date,
                    arrival_date,
                    duration,
                    origin,
                    destination,
                    airplane_id,
                    start_time
                FROM
                    flight
                WHERE
                    id = @id
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Flight>(query, new { id = id });            
        }

        public async Task<Flight?> GetByDepartureDateAsync(DateTime departureDate)
        {
            var query =
            @"
                SELECT
                    id,
                    code,
                    departure_date AS departureDate,
                    arrival_date AS arrivalDate,
                    duration,
                    origin,
                    destination,
                    airplane_id AS airplaneId,
                    start_time AS startTime
                FROM
                    flight
                WHERE
                    departure_date = @departureDate
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Flight>(query, new { departureDate });
        }
    }
}
