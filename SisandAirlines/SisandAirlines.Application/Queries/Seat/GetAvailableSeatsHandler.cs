using MediatR;
using SisandAirlines.Application.Request.Seat;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Shared.Factories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Models;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.Queries.Seat
{
    public class GetAvailableSeatsHandler : IRequestHandler<GetAvailableSeatsRequest, ResponseData>
    {
        private readonly INotificator _notificator;
        private readonly ISeatRepository _repository;

        public GetAvailableSeatsHandler(INotificator notificator, ISeatRepository repository)
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ResponseData> Handle(GetAvailableSeatsRequest request, CancellationToken cancellationToken)
        {
            var seats = await _repository.GetSeatsByFlightIdAsync(request.FlightId);

            if (!seats.Any())
            {
                _notificator.Add(new Notification($"Não foram encontrados assentos no voo {request.FlightId}", HttpStatusCode.NotFound));
                return ResponseFactory.NotFound($"Não foram encontrados assentos no voo {request.FlightId}");
            }

            return ResponseFactory.Success(seats);
        }
    }
}
