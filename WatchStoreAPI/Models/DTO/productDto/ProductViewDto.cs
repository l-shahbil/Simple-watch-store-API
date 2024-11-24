namespace WatchStoreAPI.Models.DTO.productDto
{
    public class ProductViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ModelNumber { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImgPath { get; set; }
        public double Price { get; set; }
    }
}
