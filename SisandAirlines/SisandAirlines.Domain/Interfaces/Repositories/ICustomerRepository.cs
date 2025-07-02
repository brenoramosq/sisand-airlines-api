using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer?> GetByDocumentAsync(string document);
        Task<Customer?> GetByEmailAndPasswordAsync(string email, string password);
        Task CreateAsync(Customer customer);
    }
}
