using System;
using System.Data;
using System.Threading.Tasks;

namespace SisandAirlines.Domain.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction? Transaction { get; }

        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
