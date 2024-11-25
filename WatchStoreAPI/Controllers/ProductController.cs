using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO.productDto;
using WatchStoreAPI.Repository.Base;
using static System.Net.Mime.MediaTypeNames;


namespace WatchStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repoProduct;

        public ProductController(IRepository<Product> repoProduct)
        {
            _repoProduct = repoProduct;
        }

        [HttpGet("GetProducts")]

        public async Task<IActionResult> GetProducts()
        {
            var products = await _repoProduct.GetAllAsync();

            List<ProductViewDto> listProductViewDto = new List<ProductViewDto>();
            foreach (var product in products)
            {
                ProductViewDto prodView = new ProductViewDto();

                prodView.Id = product.Id;
                prodView.Name = product.Name;
                prodView.ModelNumber = product.ModelNumber;
                prodView.Description = product.Description;
                prodView.ImgPath = product.ImgPath;
                prodView.Price = product.Price;

                listProductViewDto.Add(prodView);
            }
            return Ok(listProductViewDto);
        }
        [HttpGet("getProductById/{id:int}", Name = "ProductDetalsRoute")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id != 0 || id != null)
            {
                ProductViewDto prodView = new ProductViewDto();

                var product = await _repoProduct.GetByIdAsync(id);
                if (product != null)
                {
                    prodView.Id = product.Id;
                    prodView.Name = product.Name;
                    prodView.ModelNumber = product.ModelNumber;
                    prodView.Description = product.Description;
                    prodView.ImgPath = product.ImgPath;
                    prodView.Price = product.Price;


                    return Ok(prodView);
                }
            }
            return NotFound();
        }
       
    }
    
}

