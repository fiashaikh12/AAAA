namespace Entities
{
    public class ProductDetails
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string  Name { get; set; }
        public int Quantity { get; set; }
        public string  SKUNumber { get; set; }
        public string  Specification { get; set; }
        public string  ImagePath { get; set; }
        public bool IsAvailable { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public bool  IsPackaging { get; set; }
        public int  CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
