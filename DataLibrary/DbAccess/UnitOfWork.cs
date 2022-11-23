using System.Data;

namespace DataLibrary.DbAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;
    public IDbConnection Connection { get { return _connection; } }
    public IDbTransaction Transaction { get { return _transaction; } }

    internal UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
        _transaction = _connection.BeginTransaction();
    }
    public void Commit()
    {
        try
        {
            _transaction.Commit();
            _transaction = _connection.BeginTransaction();
        }
        catch (Exception)
        {
            _transaction.Rollback();
        }
    }
    public void Dispose()
    {
        _transaction.Connection?.Close();
        _transaction.Connection?.Dispose();
        _transaction.Dispose();
    }
}