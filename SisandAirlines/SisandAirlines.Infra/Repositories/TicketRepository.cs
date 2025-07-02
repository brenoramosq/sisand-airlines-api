using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;

namespace SisandAirlines.Infra.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public TicketRepository(string connectionString, IUnitOfWork unitOfWork)
        {
            _connectionString = connectionString;
            _unitOfWork = unitOfWork;
        }
        
        public async Task CreateAsync(Ticket ticket)
        {
            var query =
            @"
                 INSERT INTO ticket (id, flight_id, customer_id, seat_id, issue_date, confirmation_code)
                 VALUES (@id, @flightId, @customerId, @seatId, @issueDate, @confirmationCode)
            ";

            await _unitOfWork.Connection.ExecuteAsync
            (
                query,
                new
                {
                    id = ticket.Id,
                    flightId = ticket.FlightId,
                    customerId = ticket.CustomerId,
                    seatId = ticket.SeatId,
                    issueDate = ticket.IssueDate,
                    confirmationCode = ticket.ConfirmationCode,
                },
                _unitOfWork.Transaction
            );

            using var connection = new NpgsqlConnection(_connectionString);
        }
    }
}
