namespace SisandAirlines.Infra.Models
{
    public class AvailableFlightDTO
    {
        public Guid FlightId { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int SeatType { get; set; }
        public TimeSpan StartTime { get; set; } 
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FlightCode { get; set; }
        public decimal Price
        {
            get
            {
                return SeatType == 1 ? 159.97m : 399.93m;
            }
        }

        public string[] AvailableSeats { get; set; }
    }
}
