using MediatR;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.Request.Flight
{
    public class GetAvailableFlightsRequest : IRequest<ResponseData>
    {
        public DateTime DepartureDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int NumberPassengers { get; set; }
        public SeatType SeatType { get; set; }
    }
}
