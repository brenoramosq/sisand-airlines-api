using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Application.Request.ShoppingCartItem
{
    public class ShoppingCartItemRequest
    {
        public SeatType SeatType { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
