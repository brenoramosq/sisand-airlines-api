using MediatR;
using SisandAirlines.Application.Request.ShoppingCart;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Domain.Interfaces.Externals;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;
using SisandAirlines.Infra.Repositories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.ShoppingCart
{
    public class CheckoutShoppingCartHandler : IRequestHandler<CheckoutShoppingCartRequest>
    {
        private readonly INotificator _notificator;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICustomerRepository _customerRepository;

        private readonly IEmailService _emailService;

        public CheckoutShoppingCartHandler
        (
            INotificator notificator,

            IUnitOfWork unitOfWork,

            IShoppingCartRepository shoppingCartRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository,
            ICustomerRepository customerRepository,
            IPurchaseRepository purchaseRepository,

            IEmailService emailService
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _shoppingCartRepository = shoppingCartRepository ?? throw new ArgumentNullException(nameof(shoppingCartRepository));
            _seatRepository = seatRepository ?? throw new ArgumentNullException(nameof(seatRepository));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _purchaseRepository = purchaseRepository ?? throw new ArgumentNullException(nameof(purchaseRepository));

            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task Handle(CheckoutShoppingCartRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var shoppingCart = await _shoppingCartRepository.GetWithItemsById(request.ShoppingCartId);

                if (shoppingCart is null || shoppingCart.IsFinalized)
                {
                    _notificator.Add(new Notification("Carrinho inválido ou já finalizado.", HttpStatusCode.NotFound));
                    return;
                }

                if (!shoppingCart.Items.Any())
                {
                    _notificator.Add(new Notification("Não há itens no carrinho.", HttpStatusCode.NotFound));
                    return;
                }

                if (shoppingCart.CustomerId != request.CustomerId)
                {
                    _notificator.Add(new Notification("O carrinho não pertence ao cliente.", HttpStatusCode.Conflict));
                    return;
                }

                var reservedSeatIds = new List<Guid>();
                var tickets = new List<Ticket>();

                foreach (var item in shoppingCart.Items)
                {
                    var seat = await _seatRepository.GetByIdAsync(item.SeatId);

                    if (seat is null)
                    {
                        _notificator.Add(new Notification($"Assento não existe", HttpStatusCode.NotFound));
                        return;
                    }

                    reservedSeatIds.Add(seat.Id);

                    tickets.Add(new Ticket
                    (
                        flightId: item.FlightId,
                        customerId: request.CustomerId,
                        seatId: seat.Id
                    ));
                }

                await _seatRepository.ReserveSeatsAsync(reservedSeatIds);

                var purchase = new Purchase
                (
                     customerId: request.CustomerId,
                     shoppingCartId: shoppingCart.Id,
                     totalAmount: shoppingCart.Items.Sum(i => i.UnitPrice * i.Quantity)
                );

                await _purchaseRepository.CreateAsync(purchase);

                foreach (var ticket in tickets)
                {
                    ticket.SetConfirmationCode(purchase.ConfirmationCode);
                    await _ticketRepository.CreateAsync(ticket);
                }

                shoppingCart.Finalize();
                await _shoppingCartRepository.UpdateAsync(shoppingCart);

                var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

                await _emailService.SendEmailAsync(customer, tickets);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
