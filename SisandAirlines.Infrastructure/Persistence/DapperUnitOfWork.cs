using System.Data;
using System.Threading.Tasks;
using SisandAirlines.Domain.Interfaces;

namespace SisandAirlines.Infrastructure.Persistence
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;

        public IDbConnection Connection => _connection;
        public IDbTransaction? Transaction => _transaction;

        public DapperUnitOfWork(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task BeginAsync()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            _transaction = _connection.BeginTransaction();
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            if (_transaction is null)
                return Task.CompletedTask;

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;

            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            if (_transaction is null)
                return Task.CompletedTask;

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            if (_transaction is not null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            _connection.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
