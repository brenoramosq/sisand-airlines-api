using MediatR;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.Request.ShoppingCart
{
    public class GetBySessionRequest : IRequest<ResponseData>
    {
        public string Session { get; set; }
    }
}
