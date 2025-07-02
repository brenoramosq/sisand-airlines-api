using MediatR;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.Request.Seat
{
    public class GetAvailableSeatsRequest : IRequest<ResponseData>
    {
        public Guid FlightId { get; set; }
    }
}
