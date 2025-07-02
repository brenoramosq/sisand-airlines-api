using MediatR;
using SisandAirlines.Application.Request.ShoppingCart;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Shared.Factories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Models;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.Queries.ShoppingCart
{
    public class GetBySessionHandler : IRequestHandler<GetBySessionRequest, ResponseData>
    {
        private readonly INotificator _notificator;
        private readonly IShoppingCartRepository _repository;

        public GetBySessionHandler(INotificator notificator, IShoppingCartRepository repository)
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ResponseData> Handle(GetBySessionRequest request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _repository.GetBySessionAsync(request.Session);

            if (shoppingCart is null)
            {
                _notificator.Add(new Notification("Carrinho não encontrado para essa sessão", HttpStatusCode.NotFound));
                return ResponseFactory.NotFound("Carrinho não encontrado para essa sessão.");
            }
            
            return ResponseFactory.Success(shoppingCart);
        }
    }
}
