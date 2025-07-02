using MediatR;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Application.Request.Seat;
using SisandAirlines.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/seat")]
    [ApiController]
    public class SeatController : MainController
    {
        private readonly IMediator _mediator;

        public SeatController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(notificator));
        }

        [HttpGet("by-flight")]
        public async Task<IActionResult> GetSeatsByFlightIdAsync([FromQuery][Required] Guid flightId)
        {
            var response = await _mediator.Send(new GetAvailableSeatsRequest()
            {
                FlightId = flightId
            });

            return CustomResponse(HttpStatusCode.OK, response);
        }
    }
}
