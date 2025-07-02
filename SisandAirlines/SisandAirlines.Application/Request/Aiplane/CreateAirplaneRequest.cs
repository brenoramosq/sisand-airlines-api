using MediatR;

namespace SisandAirlines.Application.Request.Aiplane
{
    public class CreateAirplaneRequest : IRequest
    {
        public string Model { get; set; }   
    }
}
