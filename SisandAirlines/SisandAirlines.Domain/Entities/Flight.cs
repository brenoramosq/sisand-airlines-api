namespace SisandAirlines.Domain.Entities
{
    public class Flight : Entity
    {
        public DateTime DepartureDate { get; private set; }
        public DateTime ArrivalDate { get; private set; }     
        public TimeSpan Duration { get; private set; }
        public string Origin { get; private set; }
        public string Destination { get; private set; } 
        public Guid AirplaneId { get; private set; }
        public string Code { get; private set; }
        public TimeSpan StartTime { get; private set; }
        
        private readonly List<Seat> _seats;
        public IReadOnlyCollection<Seat> Seats => _seats;

        public Flight(Guid airplaneId, int startTime)
        : base()
        {
            AirplaneId = airplaneId;
            Code = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper(); ;
            DepartureDate = DateTime.Now;
            ArrivalDate = DateTime.Now;
            Duration = TimeSpan.FromHours(1);
            Origin = "Curitiba";
            Destination = "São Paulo";

            _seats = new List<Seat>();
            StartTime = TimeSpan.FromHours(startTime);
        }

        public Flight
        (
            Guid id, 
            string code, 
            DateTime departureDate, 
            DateTime arrivalDate, 
            TimeSpan duration, 
            string origin,
            string destination,
            Guid airplaneId,
            TimeSpan startTime
        )
            :base(id)
        {
            Code = code;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
            Duration = duration;
            Origin = origin;
            Destination = destination;            
            AirplaneId = airplaneId;
            StartTime = startTime;
        }

        public void AddSeat(Seat seat)
        {
            _seats.Add(seat);
        }
    }
}
