using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;

namespace SisandAirlines.Infra.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly string _connectionString;

        public PurchaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateAsync(Purchase purchase)
        {
            var query =
             @"
                INSERT INTO purchase (id, customer_id, shopping_cart_id, purchase_date, total_amount, payment_method, confirmation_code)
                VALUES (@id, @customerId, @shoppingCartId, @purchaseDate, @totalAmount, @paymentMethod, @confirmationCode);
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new
            {
                id = purchase.Id,
                customerId = purchase.CustomerId,
                shoppingCartId = purchase.ShoppingCartId,
                purchaseDate = purchase.PurchaseDate,
                totalAmount = purchase.TotalAmount,
                paymentMethod = purchase.PaymentMethod,
                confirmationCode = purchase.ConfirmationCode,
            });
        }
    }
}
