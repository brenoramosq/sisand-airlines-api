using MediatR;
using SisandAirlines.Application.Request.ShoppingCart;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;
using SisandAirlines.Shared.Factories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Models;

namespace SisandAirlines.Application.UseCases.Command.ShoppingCart
{
    public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartRequest, ResponseData>
    {
        private readonly INotificator _notificator;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IShoppingCartRepository _repository;
        private readonly IShoppingCartItemRepository _itemRepository;

        public CreateShoppingCartHandler
        (
            INotificator notificator, 
            IUnitOfWork unitOfWork, 
            IShoppingCartRepository repository,
            IShoppingCartItemRepository itemRepository
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public async Task<ResponseData> Handle(CreateShoppingCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                
                var shoppingCart = new Domain.Entities.ShoppingCart(request.CustomerId, request.Session);
                
                await _repository.CreateAsync(shoppingCart);

                var items = request?
                    .Items?
                    .Select(sci => new Domain.Entities.ShoppingCartItem
                    (
                        shoppingCartId:  shoppingCart.Id, 
                        flightId: request.FlightId, 
                        seatType: sci.SeatType, 
                        quantity: sci.Quantity,
                        unitPrice: sci.UnitPrice
                    ));

                await _itemRepository.CreateManyAsync(items.ToList());
               
                await _unitOfWork.SaveChangesAsync();

                return ResponseFactory.Success($"Carrinho criado com sucesso");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception($"Erro ao criar carrinho e seus itens: {ex.Message}");
            }            
        }
    }
}
