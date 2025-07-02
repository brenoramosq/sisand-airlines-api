using MediatR;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Application.Request.Customer;
using SisandAirlines.Shared.Interfaces;
using System.Net;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customer")]
    [ApiController]
    public class CustomerController : MainController
    {
        private readonly IMediator _mediator;
        
        public CustomerController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(notificator));
        }

        [HttpPost("register")]
        public async Task<ActionResult> CreateAsync([FromBody] CreateCustomerRequest request)
        {
            await _mediator.Send(request);
            return CustomResponse(HttpStatusCode.Created, "Cliente cadastrado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCustomerRequest request)
        {           
            var token = await _mediator.Send(request);
            return CustomResponse(HttpStatusCode.OK, token);            
        }
    }
}
