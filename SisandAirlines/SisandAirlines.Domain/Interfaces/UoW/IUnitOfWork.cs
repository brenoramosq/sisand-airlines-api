using System.Data;

namespace SisandAirlines.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        Task RollbackAsync();
    }
}
