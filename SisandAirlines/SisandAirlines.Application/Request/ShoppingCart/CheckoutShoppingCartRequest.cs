using MediatR;

namespace SisandAirlines.Application.Request.ShoppingCart
{
    public class CheckoutShoppingCartRequest : IRequest
    {
        public Guid ShoppingCartId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
