using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface IPurchaseRepository
    {
        Task CreateAsync(Purchase purchase);
    }
}
