using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Application.Request.ShoppingCartItem
{
    public class ShoppingCartItemRequest
    {

        public Guid FlightId { get; set; }
        public List<string> SeatNumbers { get; set; }
        public SeatType SeatType { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
