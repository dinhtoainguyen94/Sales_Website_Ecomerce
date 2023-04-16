namespace Models.RequestModel
{
    public class ProductRequestModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageProduct { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public int StatusID { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
