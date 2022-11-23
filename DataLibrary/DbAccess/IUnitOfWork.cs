using System.Data;

namespace DataLibrary.DbAccess
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Commit();
        void Dispose();
    }
}