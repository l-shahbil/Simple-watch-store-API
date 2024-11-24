namespace WatchStoreAPI.Models.DTO
{
    public class CartDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductModelNumber { get; set; } 
        public string? ProductDescription { get; set; }
        public string? ProductImgPath { get; set; }
        public double price { get; set; }
        public int Quantity { get; set; }
    }
}
