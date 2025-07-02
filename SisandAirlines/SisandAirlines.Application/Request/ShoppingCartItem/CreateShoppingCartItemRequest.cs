using MediatR;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.Request.ShoppingCartItem
{
    public class CreateShoppingCartItemRequest : IRequest<ResponseData>
    {
        public Guid ShoppingCartId { get; set; }
        public Guid FlightId { get; set; }
        public ShoppingCartItemRequest Item { get; set; }
    }
}
