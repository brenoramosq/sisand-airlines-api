using MediatR;
using SisandAirlines.Application.Request.ShoppingCartItem;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.Request.ShoppingCart
{
    public class CreateShoppingCartRequest : IRequest<ResponseData>
    {
        public Guid? CustomerId { get; set; }
        public string Session { get; set; }
        public Guid FlightId { get; set; }             
        public List<ShoppingCartItemRequest> Items { get; set; }       
    }    
}
