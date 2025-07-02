using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;

namespace SisandAirlines.Infra.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;

        public TicketRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task CreateAsync(Ticket ticket)
        {
            var query =
            @"
                 INSERT INTO ticket (id, flight_id, customer_id, seat_id, issue_date, confirmation_code)
                 VALUES (@id, @flightId, @customerId, @seatId, @issueDate, @confirmationCode)
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new
            {
                id = ticket.Id,
                flightId = ticket.FlightId,
                customerId = ticket.CustomerId,
                seatId = ticket.SeatId,
                issueDate = ticket.IssueDate,
                confirmationCode = ticket.ConfirmationCode,
            });
        }
    }
}
