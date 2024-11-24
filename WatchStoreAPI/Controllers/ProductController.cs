using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO.productDto;
using WatchStoreAPI.Repository.Base;
using static System.Net.Mime.MediaTypeNames;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace WatchStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repoProduct;
        private readonly IHostingEnvironment _host;

        public ProductController(IRepository<Product> repoProduct, IHostingEnvironment host)
        {
            _repoProduct = repoProduct;
            _host = host;
        }

        [HttpGet ("GetProducts")]
        
        public async Task<IActionResult> GetProducts()
        {
            var products =await _repoProduct.GetAllAsync();

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
        [HttpGet("getProductById/{id:int}")]
        public async Task<IActionResult> GetProductById(int id) 
        {
            if (id != 0 || id != null) 
            {
                ProductViewDto prodView = new ProductViewDto();

                var product =await _repoProduct.GetByIdAsync(id);
                if(product != null)
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
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddProduct([FromForm]ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                string fileName = string.Empty;
                if (productDto.Img != null)
                {
                    string myUpload = Path.Combine(_host.WebRootPath, "images");
                    fileName = Guid.NewGuid().ToString() + productDto.Img.FileName;
                    string fullPath = Path.Combine(myUpload, fileName);
                    productDto.Img.CopyTo(new FileStream(fullPath, FileMode.Create));
                    product.ImgPath = $"/images/{fileName}";

                    //Add properties
                    product.Name = productDto.Name;
                    product.ModelNumber = productDto.ModelNumber;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                }
                await _repoProduct.AddAsyncEntity(product);
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest();

        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> EditProduct([FromRoute]int id,[FromBody]Product product)
        {
            if (ModelState.IsValid)
            {
                if(id != 0 || id != null)
                {
                    var prod = await _repoProduct.GetByIdAsync(id);
                    if (prod != null) 
                    {                     
                        _repoProduct.UpdateEntity(product);
                        return StatusCode(StatusCodes.Status204NoContent);
                    }

                }
            }
            return NotFound();
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem(int id)
        {

            if (id != 0 || id != null)
            {
                var product = await _repoProduct.GetByIdAsync(id);
                if (product != null)
                {
                    _repoProduct.DeleteEntity(product);
                    return NoContent();
                }
            }
            return NotFound();
        }
    }
}

