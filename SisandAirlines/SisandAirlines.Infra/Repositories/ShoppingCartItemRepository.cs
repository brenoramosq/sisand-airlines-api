using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;

namespace SisandAirlines.Infra.Repositories
{
    public class ShoppingCartItemRepository : IShoppingCartItemRepository
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartItemRepository(string connectionString, IUnitOfWork unitOfWork)
        {
            _connectionString = connectionString;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateOneAsync(ShoppingCartItem item)
        {
            var query =
            @"  
                INSERT INTO shopping_cart_item (id, shopping_cart_id, flight_id, seat_type, quantity, unit_price)
                VALUES (@id, @cartId, @flightId, @seatType, @quantity, @unitPrice);
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new
            {
                id = item.Id,
                cartId = item.ShoppingCartId,
                flightId = item.FlightId,
                seatType = item.SeatType,
                quantity = item.Quantity,
                unitPrice = item.UnitPrice
            });
        }

        public async Task CreateManyAsync(List<ShoppingCartItem> items)
        {
            var query = 
            @"
                INSERT INTO shopping_cart_item (id, shopping_cart_id, flight_id, seat_type, quantity, unit_price)
                VALUES (@id, @shoppingCartId, @flightId, @seatType, @quantity, @unitPrice);
            ";

            await _unitOfWork.Connection.ExecuteAsync(query, items, _unitOfWork.Transaction);            
        }
    }
}
