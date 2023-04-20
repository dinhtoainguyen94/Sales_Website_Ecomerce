using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using Repository.Interfaces.Actions;
using System.Data.SqlClient;

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
            int CartID = GetCartIDByIDCustomer(item.CustomerID);
            SqlDataReader reader;
            if (CartID != 0) //Customer đã có cart
            {
                //1.Check Product đã có trong Cart_Product
                reader = GetCartProduct(item.ProdutID, CartID);

                reader.Read();
                int oldQuantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);
                bool hasRows = reader.HasRows;
                reader.Close();


                if (hasRows) //Product đã có cart
                {
                    item.Quantity = oldQuantity + item.Quantity;
                    //1.1 update lại so luong, status
                    return UpdateCart(item, CartID);
                }
                else
                {
                    //2.Add Cart_Product
                    return InsertCartProduct(item, CartID);
                }
            }
            else //Customer chưa có cart
            {
                SqlCommand command = CreateCommand("sp_InsertCart");
                command.Parameters.AddWithValue("@CustomerID", item.CustomerID);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (command.ExecuteNonQuery() != 0) //add cart success
                {
                    #region Process cart_product
                    //1. Get CartID
                    CartID = GetCartIDByIDCustomer(item.CustomerID);

                    //2. Add Cart_Product
                    return InsertCartProduct(item, CartID);

                    #endregion
                }
            }
            return 0;
        }

        public List<CartResponeModel> GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public List<CartResponeModel> GetAll()
        {
            throw new NotImplementedException();
        }

        private int UpdateCart(CartRequestModel item, int CartID)
        {
            SqlCommand command = CreateCommand("sp_UpdateCartProduct");
            command.Parameters.AddWithValue("@CartID", CartID);
            command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@StatusID", item.StatusID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command.ExecuteNonQuery();
        }

        private int InsertCartProduct(CartRequestModel item, int CartID)
        {
            SqlCommand command = CreateCommand("sp_InsertCartProduct");
            command.Parameters.AddWithValue("@CartID", CartID);
            command.Parameters.AddWithValue("@ProdutID", item.ProdutID);
            command.Parameters.AddWithValue("@Quantity", item.Quantity);
            command.Parameters.AddWithValue("@StatusID", item.StatusID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command.ExecuteNonQuery();
        }

        private SqlDataReader GetCartProduct(int produtID, int CartID)
        {
            SqlCommand command = CreateCommand("sp_GetCartProduct");
            command.Parameters.AddWithValue("@CartID", CartID);
            command.Parameters.AddWithValue("@ProductID", produtID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command.ExecuteReader();
        }

        private int GetCartIDByIDCustomer(int customerID)
        {
            SqlCommand command = CreateCommand("sp_GetCartByIDCustomer");
            command.Parameters.AddWithValue("@CustomerID", customerID);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var reader = command.ExecuteReader();

            reader.Read();
            int CartID = string.IsNullOrEmpty(reader["CartID"].ToString()) ? 0 : Convert.ToInt32(reader["CartID"]);
            reader.Close();

            return CartID;
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

        CartResponeModel IReadRepository<CartResponeModel, int>.Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<CartResponeModel> Get(int customerID = 0, int pageIndex = 1)
        {
            //1 get cartID
            int CartID = GetCartIDByIDCustomer(customerID);

            //2 get CartProduct
            var reader = GetCartProduct(0, CartID);

            List<CartResponeModel> cart = new List<CartResponeModel>();
            while (reader.Read())
            {
                var cartProduct = new CartResponeModel();
                cartProduct.ProductName = reader["Name"].ToString() ?? "";
                cartProduct.Quantity = string.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"]);
                cartProduct.QuantityMax = string.IsNullOrEmpty(reader["QuantityMax"].ToString()) ? 0 : Convert.ToInt32(reader["QuantityMax"]);
                cartProduct.Price = string.IsNullOrEmpty(reader["Price"].ToString()) ? 0 : Convert.ToDecimal(reader["Price"]);
                cart.Add(cartProduct);
            }
            reader.Close();

            return cart;
        }
    }
}
