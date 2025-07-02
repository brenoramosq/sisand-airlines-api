using MediatR;
using SisandAirlines.Application.Request.ShoppingCartItem;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Shared.Factories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Models;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.ShoppingCartItem
{
    public class CreateShoppingCartItemHandler : IRequestHandler<CreateShoppingCartItemRequest, ResponseData>
    {
        private readonly INotificator _notificator;

        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        public CreateShoppingCartItemHandler
        (
            INotificator notificator, 
            IShoppingCartRepository shoppingCartRepository, 
            IShoppingCartItemRepository shoppingCartItemRepository
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
           
            _shoppingCartItemRepository = shoppingCartItemRepository ?? throw new ArgumentNullException(nameof(shoppingCartRepository));
            _shoppingCartRepository = shoppingCartRepository ?? throw new ArgumentNullException(nameof(shoppingCartRepository)) ;
        }

        public async Task<ResponseData> Handle(CreateShoppingCartItemRequest request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _shoppingCartRepository.GetByIdAsync(request.ShoppingCartId);
            if(shoppingCart is null)
            {
                _notificator.Add(new Notification("Carrinho não existe", HttpStatusCode.NotFound));
                return ResponseFactory.NotFound("Carrinho não existe");
            }

            var item = new Domain.Entities.ShoppingCartItem
            (
                shoppingCartId: request.ShoppingCartId,             
                flightId: request.FlightId,
                seatType: request.Item.SeatType,
                quantity:  request.Item.Quantity,
                unitPrice: request.Item.UnitPrice
            );

            await _shoppingCartItemRepository.CreateOneAsync(item);

            return ResponseFactory.Success("Item adicionado com sucesso.");
        }
    }
}
