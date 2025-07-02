using MediatR;
using SisandAirlines.Application.Request.Flight;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.Flight
{
    public class CreateDailyFlightsHandler : IRequestHandler<CreateDailyFlightsRequest>
    {
        private readonly INotificator _notificator;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly ISeatRepository _seatRepository;

        public CreateDailyFlightsHandler
        (
            INotificator notificator, 
            
            IUnitOfWork unitOfWork,

            IFlightRepository flightRepository, 
            IAirplaneRepository airplaneRepository,
            ISeatRepository seatRepository
        )
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _airplaneRepository = airplaneRepository ?? throw new ArgumentNullException(nameof(airplaneRepository));
            _seatRepository = seatRepository ?? throw new ArgumentNullException(nameof (seatRepository));
        }

       
        public async Task Handle(CreateDailyFlightsRequest request, CancellationToken cancellationToken)
        {
            try
            {               
                var airplanes = await _airplaneRepository.GetAllAsync();

                if (!airplanes.Any())
                {
                    _notificator.Add(new Notification("Nenhum avião foi encontrado.", HttpStatusCode.NotFound));
                    return;
                }

                var departureTimes = new[] { 0, 3, 6, 9, 12, 15, 18, 21 };

                await _unitOfWork.BeginTransactionAsync();
                foreach (var airplane in airplanes)
                {
                    foreach (var time in departureTimes)
                    {
                        var airplaneExists = await _flightRepository.GetByDepartureDateAsync(request.DepartureDate.Date);

                        if (airplaneExists is not null)
                        {
                            _notificator.Add
                            (
                                new Notification($"Os voos para esse dia {request.DepartureDate.Date.ToString("dd/MM/yyyy")} ultrapassam o limite permitido.",
                                HttpStatusCode.Conflict
                            ));

                            await _unitOfWork.RollbackAsync();
                            return;
                        }

                        var flight = new Domain.Entities.Flight(airplaneId: airplane.Id, time);
                        await _flightRepository.CreateAsync(flight);

                        var seats = new List<Seat>
                        {
                            new Seat(flight.Id, "1A", SeatType.FirstClass),
                            new Seat(flight.Id, "1B", SeatType.FirstClass),
                            new Seat(flight.Id, "2A", SeatType.Economy),
                            new Seat(flight.Id, "2B", SeatType.Economy),
                            new Seat(flight.Id, "2C", SeatType.Economy),
                            new Seat(flight.Id, "3A", SeatType.Economy),
                            new Seat(flight.Id, "3B", SeatType.Economy)
                        };

                        foreach (var seat in seats)
                        {
                            await _seatRepository.CreateAsync(seat);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception($"Erro ao inserir voos no dia {DateTime.Now.Date}");
            }
        }
    }
}
