using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }

        public string Create(ProductRequestModel item)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_InsertProduct");
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Code", item.Code);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@Description", item.Description);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine("Add thất bại");
                return "Thêm thất bại";
            }
            else
            {
                _transaction.Commit();
                Console.WriteLine("Đã Add {0} bản ghi.", rowsAffected);
                return "Thêm thành công";
            }
        }

        public ProductResponeModel Get(int id)
        {
            var command = CreateCommand("sp_GetProductById");
            command.Parameters.AddWithValue("@productId", id);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var product = new ProductResponeModel();

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                product=  new ProductResponeModel
                {
                    Name = reader["Name"].ToString() ?? "",
                    Code = reader["Code"].ToString() ?? "",
                    Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]),
                    Price = reader["Price"].ToString() ?? "",
                    Description = reader["Description"].ToString() ?? "",
                    CategoryName = reader["CategoryName"].ToString() ?? ""
                };
            };

            command = CreateCommand("sp_GetAllCategory");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var lstCate = new List<string>();

            Dictionary<int, string> cate = new Dictionary<int, string>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cate.Add(Convert.ToInt32(reader["CategoryId"]), reader["Name"].ToString() ?? "");
                }
                product.DictCategory = cate;
            };
            return product;
        }

        public IEnumerable<ProductResponeModel> GetAll(int pageIndex)
        {
            var command = CreateCommand("sp_GetPagedData");
            command.Parameters.AddWithValue("@PageIndex", pageIndex);
            command.Parameters.AddWithValue("@PageSize", 10);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var lstProduct = new List<ProductResponeModel>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pro = new ProductResponeModel
                    {
                        Name = reader["Name"].ToString() ?? "",
                        Code = reader["Code"].ToString() ?? "",
                        Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]),
                        Price = reader["Price"].ToString() ?? "",
                        Description = reader["Description"].ToString() ?? "",
                        CategoryName = reader["CategoryName"].ToString() ??""
                    };
                    lstProduct.Add(pro);
                }
            };
            return lstProduct;
        }

        public List<ProductResponeModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public string Remove(int id)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_DeleteProduct");
            command.Parameters.AddWithValue("@productId", id);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine("Xóa thất bại");
                return "Xóa thất bại";
            }
            else
            {
                _transaction.Commit();
                Console.WriteLine("Đã Xóa {0} bản ghi.", rowsAffected);
                return "Xóa thành công";
            }
        }

        public string Update(ProductRequestModel item, int productID)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_UpdateProduct");
            command.Parameters.AddWithValue("@productId", productID);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Code", item.Code);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@Description", item.Description);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine("Update thất bại");
                return "Sửa thất bại";
            }
            else
            {
                _transaction.Commit();
                Console.WriteLine("Đã Uodate {0} bản ghi.", rowsAffected);
                return "Sửa thành công";
            }
        }
    }
}
