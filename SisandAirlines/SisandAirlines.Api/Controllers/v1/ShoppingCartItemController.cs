using MediatR;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Shared.Interfaces;

namespace SisandAirlines.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/shopping-cart-item")]
    [ApiController]
    public class ShoppingCartItemController : MainController
    {
        private readonly IMediator _mediator;
        public ShoppingCartItemController(INotificator notificator, IMediator mediator) 
            : base(notificator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(notificator));
        }
    }
}
