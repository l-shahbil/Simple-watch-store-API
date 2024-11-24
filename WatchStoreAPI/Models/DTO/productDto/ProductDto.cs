using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchStoreAPI.Models.DTO.productDto
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ModelNumber { get; set; }
        public string? Description { get; set; }
        [Required]
        public IFormFile Img { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
