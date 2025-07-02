using MediatR;
using SisandAirlines.Application.Request.Customer;
using SisandAirlines.Domain.Interfaces.Auth;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Utils;
using SisandAirlines.Shared.Factories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Models;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.Customer
{
    public class LoginCustomerHandler : IRequestHandler<LoginCustomerRequest, ResponseData>
    {
        private readonly INotificator _notificator;

        private readonly IJwtService _jwt;

        private readonly ICustomerRepository _repository;

        public LoginCustomerHandler
        (
           INotificator notificator,

           IJwtService jwt,

           ICustomerRepository repository
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));

            _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ResponseData> Handle(LoginCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByEmailAndPasswordAsync(request.Email, request.Password);

            if(customer is null)
            {
                _notificator.Add(new Notification("Email ou senha estão incorretos.", HttpStatusCode.Unauthorized));
                return ResponseFactory.NotFound($"Email ou senha estão incorretos.");
            }

            var accessToken = _jwt.GenerateToken(customer);
            return ResponseFactory.Success(accessToken);
        }
    }
}
