namespace Models.ResponseModels
{
    public class CartResponeModel
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public int QuantityMax { get; set; }
        public decimal Price { get; set; }
    }
}
