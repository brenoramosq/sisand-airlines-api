using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface IAirplaneRepository
    {
        Task<IEnumerable<Airplane>> GetAllAsync();
        Task CreateAsync(Airplane airplane);
    }
}
