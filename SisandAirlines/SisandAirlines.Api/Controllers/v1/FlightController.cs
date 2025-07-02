using MediatR;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Application.Request.Flight;
using SisandAirlines.Application.Request.ShoppingCart;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/flight")]
    [ApiController]
    public class FlightController : MainController
    {
        private readonly IMediator _mediator;
        public FlightController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(notificator));
        }


        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableFlightsAsync
        (
            [FromQuery][Required] string origin,
            [FromQuery][Required] string destination,
            [FromQuery][Required] DateTime date, 
            [FromQuery][Required] int passengers, 
            [FromQuery][Required] SeatType type
        )
        {
            var result = await _mediator.Send(new GetAvailableFlightsRequest()
            {
                DepartureDate = date,
                Origin = origin,
                Destination = destination,
                NumberPassengers = passengers,
                SeatType = type
            });


            return CustomResponse(HttpStatusCode.OK, result);
        }

        [HttpPost("create-daily-flights")]
        public async Task<IActionResult> CreateCartWithItemAsync()
        {
            var request = new CreateDailyFlightsRequest() { DepartureDate = DateTime.Now };
            await _mediator.Send(request);
            
            return CustomResponse(HttpStatusCode.OK, $"Voos criados com sucesso para o dia {request.DepartureDate.ToString("dd-MM-yyyy")}");
        }
    }
}
