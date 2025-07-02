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
        private readonly ISeatRepository _seatRepository;

        public CreateShoppingCartHandler
        (
            INotificator notificator, 
            IUnitOfWork unitOfWork, 
            IShoppingCartRepository repository,
            IShoppingCartItemRepository itemRepository,
            ISeatRepository seatRepository
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _seatRepository = seatRepository ?? throw new ArgumentNullException(nameof (seatRepository));
        }

        public async Task<ResponseData> Handle(CreateShoppingCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                
                var shoppingCart = new Domain.Entities.ShoppingCart(request.CustomerId, request.Session);
                
                await _repository.CreateAsync(shoppingCart);

                foreach ( var item in request.Items )
                {
                    foreach (var number in item.SeatNumbers)
                    {
                        var seat = await _seatRepository.GetByFlightIdAndNumberAsync(item.FlightId, number);

                        if(seat is null)
                        {
                            _notificator.Add
                            (
                                new Shared.Notifications.Notification($"Assento {number} não está disponível no voo {item.FlightId}",
                                System.Net.HttpStatusCode.Conflict)
                            );
                        }

                        var cartItem = new Domain.Entities.ShoppingCartItem
                        (
                            shoppingCartId: shoppingCart.Id,
                            flightId: item.FlightId,
                            seatType: item.SeatType,
                            quantity: item.Quantity,
                            unitPrice: item.UnitPrice,
                            seatId: seat.Id
                        );

                        await _itemRepository.CreateOneAsync(cartItem);
                    }
                }               
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
