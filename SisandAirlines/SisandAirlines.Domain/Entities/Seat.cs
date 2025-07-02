using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Domain.Entities
{
    public class Seat : Entity
    {

        public Guid FlightId { get; set; }
        public string Number { get; set; }
        public SeatType SeatType { get; set; }
        public bool IsReserved { get; set; }    

        public Seat(Guid flightId, string number, SeatType seatType)
            :base()
        {
            FlightId = flightId;
            Number = number;
            SeatType = seatType;
            IsReserved = false;
        }

        public Seat(Guid id, Guid flightId, string number, SeatType seatType, bool isReserved)
            : base(id)
        {
            FlightId = flightId;
            Number = number;
            SeatType = seatType;
            IsReserved = isReserved;
        }

        public void Reserve() 
        {
            IsReserved = true;
        }        
    }
}
