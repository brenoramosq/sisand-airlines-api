using MediatR;
using SisandAirlines.Application.Request.Flight;
using SisandAirlines.Infra.DAO.Interfaces;
using SisandAirlines.Shared.Factories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Models;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.Queries.Flight
{
    public class GetAvailableFlightsHandler : IRequestHandler<GetAvailableFlightsRequest, ResponseData>
    {
        private readonly INotificator _notificator;

        private readonly IFlightDAO _dao;

        public GetAvailableFlightsHandler(INotificator notificator, IFlightDAO dao)
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
            _dao = dao ?? throw new ArgumentNullException(nameof(dao));
        }

        public async Task<ResponseData> Handle(GetAvailableFlightsRequest request, CancellationToken cancellationToken)
        {            
            var availables = await _dao.GetAvailableFlightsAsync
            (
                request.Origin,
                request.Destination,
                request.NumberPassengers,
                request.SeatType,
                request.DepartureDate,
                TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"))
            );

            if(!availables.Any())
            {
                _notificator.Add(new Notification($"Voos não foram encontrados para a data: {request.DepartureDate} de {request.Origin} até {request.Destination}", HttpStatusCode.NotFound));
                return ResponseFactory.NotFound($"Voos não foram encontrados para a data: {request.DepartureDate}");
            }

            return ResponseFactory.Success(availables);
        }
    }
}
