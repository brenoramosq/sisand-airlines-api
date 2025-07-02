using Npgsql;
using SisandAirlines.Domain.Interfaces.UoW;
using System.Data;

namespace SisandAirlines.Infra.UoW
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly string _connectionString;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;

        public IDbConnection Connection => _connection!;
        public IDbTransaction Transaction => _transaction!;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(_connectionString);
                await ((NpgsqlConnection)_connection).OpenAsync();
            }

            _transaction = _connection.BeginTransaction();
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public Task RollbackAsync()
        {
            _transaction?.Rollback();
            Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _transaction?.Dispose();
            _connection?.Dispose();

            _transaction = null;
            _connection = null;
            _disposed = true;
        }
    }
}
