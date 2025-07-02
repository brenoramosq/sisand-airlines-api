using MediatR;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Shared.Interfaces;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/email")]
    [ApiController]
    public class EmailController : MainController
    {
        private readonly IMediator _mediator;

        public EmailController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}
