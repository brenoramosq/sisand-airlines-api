using MediatR;
using SisandAirlines.Application.Request.Customer;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;
using SisandAirlines.Shared.Validators;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.Customer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest>
    {
        private readonly INotificator _notificator;

        private readonly ICustomerRepository _repository;

        public CreateCustomerHandler
        (
            INotificator notificator,

            ICustomerRepository repository
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            if (!BrazilianDocumentValidator.IsValidCpf(request.Document))
            {
                _notificator.Add(new Notification("O CPF é inválido.", HttpStatusCode.Conflict));
                return;
            }

            if (!request.Password.Equals(request.PasswordConfirmation))
            {
                _notificator.Add(new Notification("A senha e a confirmação de senha não correspondem.", HttpStatusCode.BadRequest));
                return;
            }

            if (!IsOver18(request.DateOfBirth))
            {
                _notificator.Add(new Notification("É necessário ter mais de 18 anos para realizar o cadastro.", HttpStatusCode.Conflict));
                return;
            }

            var emailExists = await _repository.GetByEmailAsync(request.Email);
            if (emailExists is not null)
            {
                _notificator.Add(new Notification("O e-mail já foi cadastrado.", HttpStatusCode.Conflict));
                return;
            }

            var documentExists = await _repository.GetByDocumentAsync(request.Document);
            if (documentExists is not null)
            {
                _notificator.Add(new Notification("O CPF já foi cadastrado.", HttpStatusCode.Conflict));
                return;
            }            

            var newCustomer = new Domain.Entities.Customer
            (
                request.FullName,
                request.Email,
                request.Document,
                request.DateOfBirth,
                request.Password,
                request.SecondaryPassword
            );

            await _repository.CreateAsync(newCustomer);
        }

        private bool IsOver18(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age >= 18;
        }
    }
}
