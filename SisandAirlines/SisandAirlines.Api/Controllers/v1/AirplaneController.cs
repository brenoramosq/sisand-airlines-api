using MediatR;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Application.Request.Aiplane;
using SisandAirlines.Shared.Interfaces;
using System.Net;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/airplane")]
    [ApiController]
    public class AirplaneController : MainController
    {
        private readonly IMediator _mediator;
        public AirplaneController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(notificator));
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateAsync([FromBody] CreateAirplaneRequest request)
        {
            await _mediator.Send(request);
            return CustomResponse(HttpStatusCode.Created, "Aviões cadastrados com sucesso.");
        }
    }
}
