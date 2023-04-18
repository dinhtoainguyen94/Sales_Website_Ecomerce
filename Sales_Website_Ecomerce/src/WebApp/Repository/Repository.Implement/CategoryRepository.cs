using Models.RequestModel;
using Repository.Interface;
using Repository.Interfaces.Actions;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class CategoryRepository : Repository, ICategoryRepository
    {
        public CategoryRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }

        public string Create(CategoryRequestModel item)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_InsertCategory");
            command.Parameters.AddWithValue("@Name", item.Name);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                //Console.WriteLine("Add thất bại");
                return "Thêm thất bại";
            }
            else
            {
                _transaction.Commit();
                //Console.WriteLine("Đã Add {0} bản ghi.", rowsAffected);
                return "Thêm thành công";
            }
        }

        public string Get(int id)
        {
            var command = CreateCommand("sp_GetCategoryById");
            command.Parameters.AddWithValue("@categoryId", id);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return reader["Name"].ToString() ?? "";
            };
        }

        public List<string> GetAll()
        {
            var command = CreateCommand("sp_GetAllCategory");
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var lstCate = new List<string>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    lstCate.Add(reader["Name"].ToString() ?? "");
                }
            };
            return lstCate;
        }

        public string Remove(int id)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_DeleteCategoryAndProducts");
            command.Parameters.AddWithValue("@CategoryId", id);

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

        public string Update(CategoryRequestModel item)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_UpdateCategory");
            command.Parameters.AddWithValue("@categoryId", item.CategoryID);
            command.Parameters.AddWithValue("@Name", item.Name);

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

        IEnumerable<string> IReadRepository<string, int>.GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }
    }
}
