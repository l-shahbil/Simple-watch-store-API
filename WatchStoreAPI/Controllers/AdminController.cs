using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO.productDto;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using WatchStoreAPI.Repository.Base;

namespace WatchStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly ICartRepository _repoCart;
        private readonly IRepository<Product> _repoProduct;
        private readonly IHostingEnvironment _host;

        public AdminController(UserManager<ApplicationUser> userManger,ICartRepository repoCart,IRepository<Product> repoProduct, IHostingEnvironment host)
        {
            _userManger = userManger;
            _repoCart = repoCart;
            _repoProduct = repoProduct;
            _host = host;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> ViewUsers()
        {
            var users = _userManger.Users.ToList();

            var usersDto = users.Where(user => user.UserName !="Admin").Select(u=> new
            {
                u.name,
                u.UserName,
                u.Email
            });
            return Ok(usersDto);
        }
        [HttpGet("GetUserCar/{userName:alpha}")]
        public async Task<IActionResult> GetUserCart(string userName)
        {
            var user =await _userManger.FindByNameAsync(userName);
       
            if (user != null) 
            { 
                var cartWithCartItem = _repoCart.ShowCartItem(user.Id);
                if (!cartWithCartItem.Any())
                {
                    return Ok("The Cart Is Empty");
                }

                return Ok(cartWithCartItem);
            }
            return NotFound();
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto productDto)
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
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        productDto.Img.CopyTo(stream);
                    }
                    product.ImgPath = $"images/{fileName}";

                    //Add properties
                    product.Name = productDto.Name;
                    product.ModelNumber = productDto.ModelNumber;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                }
                await _repoProduct.AddAsyncEntity(product);

                string url = Url.Link("ProductDetalsRoute", new {id = product.Id});
                return Created(url,product);
            }
            return BadRequest();

        }
        [HttpPut("EditProduct/{id:int}")]
        public async Task<IActionResult> EditProduct([FromRoute] int id, [FromForm] ProductUpdateDto productUpdate)
        {
            if (id != 0 || id != null)
            {
                var prod = await _repoProduct.GetByIdAsync(id);
                if (prod != null)
                {
                    prod.Name = string.IsNullOrEmpty(productUpdate.Name) ? prod.Name : productUpdate.Name;
                    prod.ModelNumber = string.IsNullOrEmpty(productUpdate.ModelNumber) ? prod.ModelNumber : productUpdate.ModelNumber;
                    prod.Description = string.IsNullOrEmpty(productUpdate.Description) ? prod.Description : productUpdate.Description;
                    prod.Price = productUpdate.Price ?? prod.Price;

                    if (productUpdate.Img != null)
                    {
                        try
                        {
                            //for delete old img
                            string imagePathOldImg = Path.Combine(_host.WebRootPath, prod.ImgPath);
                            if (System.IO.File.Exists(imagePathOldImg))
                            {
                                using (var stream = new FileStream(imagePathOldImg, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete))
                                {
                                    System.IO.File.Delete(imagePathOldImg);
                                }
                            }

                            //for add New img
                            string myUpload = Path.Combine(_host.WebRootPath, "images");
                            string fileName = Guid.NewGuid().ToString() + productUpdate.Img.FileName;
                            string fullPath = Path.Combine(myUpload, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                productUpdate.Img.CopyTo(stream);
                            }
                            prod.ImgPath = $"images/{fileName}";

                            _repoProduct.UpdateEntity(prod);
                            return NoContent();
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500, $"فشل في معالجة الصورة: {ex.Message}");
                        }

                    }
                    _repoProduct.UpdateEntity(prod);
                    return NoContent();

                }


            }
            return BadRequest();

        }

        [HttpDelete("DeleteProduct/{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
        {

            if (id != 0)
            {
                var product = await _repoProduct.GetByIdAsync(id);
                if (product != null)
                {
                    try
                    {

                        string imagePath = Path.Combine(_host.WebRootPath, product.ImgPath);

                        if (System.IO.File.Exists(imagePath))
                        {
                            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                            _repoProduct.DeleteEntity(product);
                            return NoContent();
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"فشل في حذف الصورة: {ex.Message}");
                    }
                }
            }
            return NotFound();

        }

    }
}
