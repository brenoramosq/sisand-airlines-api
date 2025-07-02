using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Domain.Entities
{
    public class ShoppingCartItem : Entity
    {
        public Guid ShoppingCartId { get; private set; }
        public Guid FlightId { get; private set; }       
        public SeatType SeatType { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public Guid SeatId { get; private set; }

        public ShoppingCartItem(Guid id, Guid shoppingCartId, Guid flightId, SeatType seatType, int quantity, decimal unitPrice, Guid seatId)
            : base(id)
        {
            ShoppingCartId = shoppingCartId;
            FlightId = flightId;
            SeatType = seatType;
            Quantity = quantity;
            UnitPrice = unitPrice;
            SeatId = seatId;
        }

        public ShoppingCartItem(Guid shoppingCartId, Guid flightId, SeatType seatType, int quantity, decimal unitPrice, Guid seatId)
            :base()
        {
            ShoppingCartId = shoppingCartId;
            FlightId = flightId;
            SeatType = seatType;
            Quantity = quantity;
            UnitPrice = unitPrice;
            SeatId = seatId;
        }
    }
}
