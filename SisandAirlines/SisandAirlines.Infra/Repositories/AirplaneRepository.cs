using Dapper;
using Npgsql;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Enums;
using SisandAirlines.Domain.Interfaces.Repositories;

namespace SisandAirlines.Infra.Repositories
{
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly string _connectionString;

        public AirplaneRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
       
        public async Task<IEnumerable<Airplane>> GetAllAsync()
        {
            var query =
            @"
                SELECT 
                    id,
                    model,
                    code
                FROM 
                    airplane
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QueryAsync<Airplane>(query);
        }

        public async Task CreateAsync(Airplane airplane)
        {
            var query =
            @"
                INSERT INTO airplane (id, model, code)
                VALUES (@id, @model, @code)
            ";

            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(query, new
            {
                id = airplane.Id,
                model = airplane.Model,
                code = airplane.Code
            });
        }
    }
}
