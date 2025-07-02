using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Application.Request.ShoppingCart;
using SisandAirlines.Shared.Interfaces;
using System.Net;
using System.Security.Claims;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/shopping-cart")]
    [ApiController]
    public class ShoppingCartController : MainController
    {
        private readonly IMediator _mediator;

        public ShoppingCartController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("create-with-items")]
        public async Task<IActionResult> CreateCartWithItemsAsync([FromBody] CreateShoppingCartRequest request)
        {
            var response = await _mediator.Send(request);

            return CustomResponse(HttpStatusCode.OK, response);
        }

        [HttpPut("associate-customer")]
        public async Task<IActionResult> AssociateCustomerToSessionAsync([FromBody] AssociateCustomerToSessionRequest request)
        {
            await _mediator.Send(request);
            return CustomResponse(HttpStatusCode.OK, "Cliente associado com sucesso.");
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("checkout")]
        public async Task<IActionResult> FinalizeCheckout([FromBody] CheckoutShoppingCartRequest request)
        {
            await _mediator.Send(request);
            return CustomResponse(HttpStatusCode.OK, $"Checkout finalizado para o cliente {request.CustomerId}");            
        }
    }
}
