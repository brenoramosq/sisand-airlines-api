namespace SisandAirlines.Domain.Entities
{
    public class Ticket : Entity
    {
        public Guid FlightId { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid SeatId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public string ConfirmationCode { get; private set; }

        public Flight Flight { get; private set; }
        public Seat Seat { get; private set; }

        public Ticket(Guid flightId, Guid customerId, Guid seatId)
            :base()
        {
            FlightId = flightId;
            CustomerId = customerId;
            SeatId = seatId;
            IssueDate = DateTime.Now;
        }

        public Ticket(Guid id, Guid flightId, Guid customerId, Guid seatId, string confirmationCode, DateTime issueDate)
            : base(id)
        {
            FlightId = flightId;
            CustomerId = customerId;
            SeatId = seatId;
            ConfirmationCode = confirmationCode;
            IssueDate = issueDate;
        }

        public Ticket(Guid id, Guid flightId, Guid customerId, Guid seatId, DateTime issueDate, string confirmationCode, Flight flight, Seat seat)
            :base(id) 
        {
            FlightId = flightId;
            CustomerId = customerId;
            SeatId = seatId;
            IssueDate = issueDate;
            ConfirmationCode = confirmationCode;
            Flight = flight;
            Seat = seat;
        }

        public void SetConfirmationCode(string confirmationCode)
        {
            ConfirmationCode = confirmationCode;
        }
    }
}
