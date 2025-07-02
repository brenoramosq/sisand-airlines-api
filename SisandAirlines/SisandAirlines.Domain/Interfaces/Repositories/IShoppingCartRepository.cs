using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface IShoppingCartRepository
    {
        Task CreateAsync(ShoppingCart cart);
        Task<ShoppingCart?> GetBySessionAsync(string session);
        Task<ShoppingCart?> GetByIdAsync(Guid id);
        Task<ShoppingCart?> GetWithItemsById(Guid id);
        Task AssociateCustomerToCartAsync(Guid cartId, Guid customerId);
        Task UpdateAsync(ShoppingCart cart);
    }
}
