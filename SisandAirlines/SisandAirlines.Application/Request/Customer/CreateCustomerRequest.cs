using MediatR;

namespace SisandAirlines.Application.Request.Customer
{
    public class CreateCustomerRequest : IRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string SecondaryPassword { get; set; }
    }
}
