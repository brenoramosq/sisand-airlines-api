
using MediatR;

namespace SisandAirlines.Application.Request.Flight
{
    public class CreateDailyFlightsRequest : IRequest
    {
        public DateTime DepartureDate { get; set; }
    }
}
