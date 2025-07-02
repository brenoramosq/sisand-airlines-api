using MediatR;
using SisandAirlines.Application.Request.ShoppingCart;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.ShoppingCart
{
    public class AssociateCustomerToSessionHandler : IRequestHandler<AssociateCustomerToSessionRequest>
    {
        private readonly INotificator _notificator;
        private readonly IShoppingCartRepository _repository;

        public AssociateCustomerToSessionHandler(INotificator notificator, IShoppingCartRepository repository)
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public async Task Handle(AssociateCustomerToSessionRequest request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _repository.GetBySessionAsync(request.Session);

            if (shoppingCart is null)
            {
                _notificator.Add(new Notification("Carrinho não encontrado para essa sessão", HttpStatusCode.NotFound));
                return;
            }

            if(shoppingCart.CanAssociateCustomer())
            {
                _notificator.Add(new Notification("Carrinho não pode ser vinculado. Já está finalizado ou associado a um cliente.", HttpStatusCode.Conflict));
                return;
            }

            shoppingCart.SetCustomerId(request.CustomerId);
            await _repository.AssociateCustomerToCartAsync(shoppingCart.Id, request.CustomerId);
        }
    }
}
