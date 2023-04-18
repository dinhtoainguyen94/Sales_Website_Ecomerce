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
        //SqlConnection: Represents a connect to a SQL Server Database
        //SqlTransaction: Represents a transaction SQL to be made in SQL Server Database        
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
            //ExecuteReader():  thực thi các truy vấn trả lại TẬP HỢP giá trị, như các truy vấn SELECT thông thường.
            //ExecuteScalar(): thực thi các truy vấn trả lại MỘT giá trị duy nhất, thường là kết quả của truy vấn Aggregate (SELECT COUNT|MIN|MAX|AVG).
            //ExecuteNonQuery(): chuyên để thực thi các truy vấn không trả về dữ liệu (INSERT, UPDATE, DELETE).
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
