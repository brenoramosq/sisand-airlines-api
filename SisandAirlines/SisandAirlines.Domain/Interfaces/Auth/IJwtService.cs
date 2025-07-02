using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateToken(Customer customer);
    }
}
