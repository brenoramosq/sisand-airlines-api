using MediatR;

namespace SisandAirlines.Application.Request.ShoppingCart
{
    public class AssociateCustomerToSessionRequest : IRequest
    {
        public Guid CustomerId { get; set; }
        public string Session { get; set; }
    }
}
