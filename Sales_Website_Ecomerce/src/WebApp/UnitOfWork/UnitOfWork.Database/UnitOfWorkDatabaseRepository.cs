using Repository.Implement;
using Repository.Interface;
using System.Data.SqlClient;
using UnitOfWork.Interface;

namespace UnitOfWork.Database
{
    public class UnitOfWorkDatabaseRepository : IUnitOfWorkRepository
    {
        public IProductRepository ProductRepository { get; }

        public UnitOfWorkDatabaseRepository(SqlConnection context, SqlTransaction transaction)
        {          
            ProductRepository = new ProductRepository(context, transaction);
        }
    }
}
