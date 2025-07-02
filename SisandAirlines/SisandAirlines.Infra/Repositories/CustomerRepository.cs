using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Repositories;
using SisandAirlines.Domain.Utils;

namespace SisandAirlines.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateAsync(Customer customer)
        {
            var query = 
            @"
                INSERT INTO customer (id, full_name, email, document, birth_date, password_hash, secondary_password_hash, create_date, active)
                VALUES (@id, @fullName, @email, @document, @birthDate, @passwordhash, @secondaryPasswordHash, @createDate, @active)
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new
            {
                id = customer.Id,
                fullname =  customer.FullName,
                email = customer.Email,
                document =  customer.Document,
                birthDate = customer.BirthDate,
                passwordhash = customer.PasswordHash,
                secondaryPasswordHash = customer.SecondaryPasswordHash,
                createDate = customer.CreateDate,
                active = customer.Active
            });
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            var query =
             @"
                SELECT 
                    id, 
                    full_name as fullName, 
                    email, 
                    document, 
                    birth_date as birthDate,
                    password_hash as passwordHash,
                    secondary_password_hash as secondaryPasswordHash,
                    create_date as createDate,
                    active
                FROM 
                    customer
                WHERE 
                    email = @email 
                    AND active = true
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { email });
        }

        public async Task<Customer?> GetByEmailAndPasswordAsync(string email, string password)
        {
            var query =
           @"
                SELECT 
                    id, 
                    full_name as fullName, 
                    email, 
                    document, 
                    birth_date as birthDate,
                    password_hash as passwordHash,
                    secondary_password_hash as secondaryPasswordHash,
                    create_date as createDate,
                    active
                FROM 
                    customer
                WHERE 
                    email = @email 
                    AND password_hash = @password
                    AND active = true
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { email = email, password = EncryptPassword.ToSHA256(password) });
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            var query =
             @"
                SELECT 
                    id, 
                    full_name as fullName, 
                    email, 
                    document, 
                    birth_date as birthDate,
                    password_hash as passwordHash,
                    secondary_password_hash as secondaryPasswordHash,
                    create_date as createDate,
                    active
                FROM 
                    customer
                WHERE 
                    id = @id      
                    AND active = true
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { id });
        }

        public async Task<Customer?> GetByDocumentAsync(string document)
        {
            var query =
             @"
                SELECT 
                    id, 
                    full_name as fullName, 
                    email, 
                    document, 
                    birth_date as birthDate,
                    password_hash as passwordHash,
                    secondary_password_hash as secondaryPasswordHash,
                    create_date as createDate,
                    active
                FROM 
                    customer
                WHERE 
                    document = @document     
                    AND active = true
              ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { document });
        }
    }
}
