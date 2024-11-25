using System.ComponentModel.DataAnnotations;

namespace WatchStoreAPI.Models.DTO.productDto
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? ModelNumber { get; set; }
        public string? Description { get; set; }
        public IFormFile? Img { get; set; }
        public double? Price { get; set; }
    }
}
