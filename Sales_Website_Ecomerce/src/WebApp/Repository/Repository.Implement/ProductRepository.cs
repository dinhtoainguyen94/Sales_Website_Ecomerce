using Models.ResponseModels;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }
        public ProductResponeModel Get(int id)
        {
            var command = CreateCommand("sp_GetProductById");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@productId", id);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new ProductResponeModel
                {
                    Name = reader["Name"].ToString(),
                    Code = reader["Code"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    Price = reader["Price"].ToString(),
                    Description = reader["Description"].ToString()
                };
            };
        }

        public IEnumerable<ProductResponeModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
