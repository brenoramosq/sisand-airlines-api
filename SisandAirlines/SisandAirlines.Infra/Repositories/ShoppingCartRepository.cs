using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Interfaces.UoW;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Transactions;

namespace SisandAirlines.Infra.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {        
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartRepository(string connectionString, IUnitOfWork unitOfWork)
        {
            _connectionString = connectionString;
            _unitOfWork = unitOfWork;
        }
        
        public async Task CreateAsync(ShoppingCart cart)
        {
            var query = @"
                INSERT INTO shopping_cart 
                (
                    id,
                    customer_id,
                    created_date,
                    is_finalized,
                    session
                ) 
                VALUES 
                (
                    @id,
                    @customerId,
                    @createdDate,
                    @isFinalized,
                    @session
                );";

            await _unitOfWork.Connection.ExecuteAsync(query, new
            {
                id = cart.Id,
                customerId = cart.CustomerId,
                createdDate = cart.CreatedDate,
                isFinalized = cart.IsFinalized,
                session = cart.Session
            }, _unitOfWork.Transaction);            
        }

        public async Task<ShoppingCart?> GetBySessionAsync(string session)
        {
            var query =
              @"
                SELECT 
                    id, 
                    customer_id, 
                    created_date, 
                    is_finalized, 
                    session
                FROM 
                    shopping_cart
                WHERE 
                    session = @session 
                    AND is_finalized = false 
                    AND customer_id IS NULL                
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<ShoppingCart>(query, new { session });
        }

        public async Task<ShoppingCart?> GetByIdAsync(Guid id)
        {
            var query =
              @"
                SELECT 
                    id, 
                    customer_id, 
                    created_date, 
                    is_finalized, 
                    session
                FROM 
                    shopping_cart
                WHERE 
                    id = @id        
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<ShoppingCart>(query, new { id });
        }

        public async Task<ShoppingCart?> GetWithItemsById(Guid id)
        {
            var query =
              @"
                SELECT 
                    sc.id, 
                    sc.customer_id, 
                    sc.created_date, 
                    sc.is_finalized, 
                    sc.session,
                    sci.id,
                    sci.shopping_cart_id,
                    sci.flight_id,
                    sci.quantity,
                    sci.unit_price
                FROM 
                    shopping_cart sc
                INNER JOIN shopping_cart_item sci ON sci.shopping_cart_id = sc.id
                WHERE 
                    sc.id = @id        
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            var dictionary = new Dictionary<Guid, ShoppingCart>();

            await connection.QueryAsync<ShoppingCart, ShoppingCartItem, ShoppingCart>(
                query,
                (cart, item) =>
                {
                    if (!dictionary.TryGetValue(cart.Id, out var shoppingCart))
                    {
                        shoppingCart = cart;    
                        dictionary.Add(shoppingCart.Id, shoppingCart);
                    }

                    shoppingCart.AddItem(item);
                    return shoppingCart;
                },
                new { id },
                splitOn: "id"
            );

            return dictionary?.Values?.FirstOrDefault();
        }

        public async Task AssociateCustomerToCartAsync(Guid cartId, Guid customerId)
        {
            var query = 
            @"
                UPDATE 
                    shopping_cart
                SET 
                    customer_id = @customerId
                WHERE 
                    id = @id AND customer_id IS NULL;
             "
            ;

            using var connection = new NpgsqlConnection(_connectionString);

            var affectedRows = await connection.ExecuteAsync(query, new
            {
                cartId = cartId,
                customerId = customerId
            });
        }

        public async Task UpdateAsync(ShoppingCart cart)
        {
            var query = @"
                UPDATE 
                    shopping_cart 
                SET 
                    is_finalized = @isFinalized 
                WHERE 
                    id = @id";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new { id = cart.Id, isFinalized = cart.IsFinalized });
        }
    }
}
