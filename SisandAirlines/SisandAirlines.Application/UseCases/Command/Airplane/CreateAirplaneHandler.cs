using MediatR;
using SisandAirlines.Application.Request.Aiplane;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;
using System.Net;

namespace SisandAirlines.Application.UseCases.Command.Airplane
{
    public class CreateAirplaneHandler : IRequestHandler<CreateAirplaneRequest>
    {
        private readonly INotificator _notificator;
        private readonly IAirplaneRepository _airplaneRepository;

        public CreateAirplaneHandler(INotificator notificator, IAirplaneRepository airplaneRepository)
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
            _airplaneRepository = airplaneRepository ?? throw new ArgumentNullException(nameof(airplaneRepository));
        }

        public async Task Handle(CreateAirplaneRequest request, CancellationToken cancellationToken)
        {
            var airplanes = await _airplaneRepository.GetAllAsync();

            if(airplanes.Any())
            {
                _notificator.Add(new Notification("Nenhum avião pode ser mais cadastrado, pois atingiu o limite", HttpStatusCode.NotFound));
                return;
            }

            var newAirplanes = new List<Domain.Entities.Airplane>()
            {
                new Domain.Entities.Airplane(request.Model),
                new Domain.Entities.Airplane(request.Model),
                new Domain.Entities.Airplane(request.Model)
            };

            foreach(var airplane in newAirplanes)
            {
                await _airplaneRepository.CreateAsync(airplane);
            }                       
        }
    }
}
