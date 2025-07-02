using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces.Repositories
{
    public interface  IShoppingCartItemRepository
    {
        Task CreateManyAsync(List<ShoppingCartItem> items);
        Task CreateOneAsync(ShoppingCartItem item);
    }
}
