using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Repository.Implement
{
    public class CartRepository : Repository, ICartRepository
    {
        public CartRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }

        public int Create(CartRequestModel item)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_GetCartByIDCustomer");
            command.Parameters.AddWithValue("@CustomerID", item.CustomerID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var reader = command.ExecuteReader();

            if (reader.HasRows) //Customer đã có cart
            {
                reader.Read();
                int CartID = string.IsNullOrEmpty(reader["CartID"].ToString()) ? 0 : Convert.ToInt32(reader["CartID"]);
                reader.Close();

                //1.Check Product đã có trong Cart_Product (thữ hiện update)
                command = CreateCommand("sp_GetCartProduct");
                command.Parameters.AddWithValue("@CartID", CartID);
                command.Parameters.AddWithValue("@ProductID", item.ProdutID);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                reader = command.ExecuteReader();

                reader.Read();
                int oldQuantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);
                bool hasRows = reader.HasRows;
                reader.Close();


                if (hasRows) //Product đã có cart
                {
                    item.Quantity = oldQuantity + item.Quantity;
                    //1.1 update lại so luong, status
                    return UpdateCart(command, item, CartID);
                }
                else
                {
                    //2.Add Cart_Product
                    return InsertCartProduct(command, item, CartID);
                }
            }
            else //Customer chưa có cart
            {
                reader.Close();
                command = CreateCommand("sp_InsertCart");
                command.Parameters.AddWithValue("@CustomerID", item.CustomerID);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (command.ExecuteNonQuery() != 0) //add cart success
                {
                    #region Process cart_product
                    //1. Get CartID
                    command = CreateCommand("sp_GetCartByIDCustomer");
                    command.Parameters.AddWithValue("@CustomerID", item.CustomerID);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    reader = command.ExecuteReader();

                    reader.Read();
                    int CartID = string.IsNullOrEmpty(reader["CartID"].ToString()) ? 0 : Convert.ToInt32(reader["CartID"]);
                    reader.Close();
                    
                    //2. Add Cart_Product
                    return InsertCartProduct(command, item, CartID);

                    #endregion
                }
            }
            return 0;
        }

        public CartResponeModel Get(int id)
        {
            var command = CreateCommand("sp_GetProductById");
            command.Parameters.AddWithValue("@productId", id);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var product = new ProductResponeModel();

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                product = new ProductResponeModel
                {
                    ProductID = reader["Name"].ToString() ?? "",
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
            return new CartResponeModel();
        }

        public List<CartResponeModel> GetAll(int pageIndex)
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
                        CategoryName = reader["CategoryName"].ToString() ?? ""
                    };
                    lstProduct.Add(pro);
                }
            };
            return new List<CartResponeModel>();
        }

        public List<CartResponeModel> GetAll()
        {
            throw new NotImplementedException();
        }

        private int UpdateCart(SqlCommand command, CartRequestModel item, int CartID)
        {
            command = CreateCommand("sp_UpdateCartProduct");
            command.Parameters.AddWithValue("@CartID", CartID);
            command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@StatusID", item.StatusID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command.ExecuteNonQuery();
        }

        private int InsertCartProduct(SqlCommand command, CartRequestModel item, int CartID)
        {
            command = CreateCommand("sp_InsertCartProduct");
            command.Parameters.AddWithValue("@CartID", CartID);
            command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@StatusID", item.StatusID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command.ExecuteNonQuery();
        }

        //public int Remove(int id)
        //{
        //    //throw new NotImplementedException();
        //    var command = CreateCommand("sp_DeleteProduct");
        //    command.Parameters.AddWithValue("@productId", id);

        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    return command.ExecuteNonQuery();
        //}

        public int Update(ProductRequestModel item, int productID)
        {
            //throw new NotImplementedException();
            var command = CreateCommand("sp_UpdateProduct");
            command.Parameters.AddWithValue("@productId", productID);
            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Code", item.Code);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@Description", item.Description);
            command.Parameters.AddWithValue("@CategoryId", item.CategoryId);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }
    }
}
