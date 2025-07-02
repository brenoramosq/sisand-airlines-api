using MediatR;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.Request.Customer
{
    public class LoginCustomerRequest : IRequest<ResponseData>
    {
        public string Email { get; set; }   
        public string Password { get; set; }
    }
}
