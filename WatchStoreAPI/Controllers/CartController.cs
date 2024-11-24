using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO;
using WatchStoreAPI.Repository.Base;

namespace WatchStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="User")]
    public class CartController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly ICartRepository _repoCart;
        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<CartItem> _repoCartItem;

        public CartController(UserManager<ApplicationUser> userManger,ICartRepository repoCart,IRepository<Product>repoProduct,IRepository<CartItem> repoCartItem) 
        {
            _userManger = userManger;
            _repoCart = repoCart;
            _repoProduct = repoProduct;
            _repoCartItem = repoCartItem;
        }

        [HttpGet("getCartItems")]
        public async Task<IActionResult> getCartItems()
        {
            var user = await _userManger.GetUserAsync(User);
            var cartWithCartItem = _repoCart.ShowCartItem(user.Id);
            if (! cartWithCartItem.Any()) 
            {
                return Ok("The Cart Is Empty");
            }

             return Ok(cartWithCartItem);
           
        }

        [HttpPost("AddProductToCart/{id:int}")]
        public async Task<IActionResult> AddProductToCart([FromRoute]int id)
        {
               if (id != 0 || id != null)
                {
                    Product p = await _repoProduct.GetByIdAsync(id);
                    if (p != null)
                    {
                        var user = await _userManger.GetUserAsync(User);
                        var cart =(await _repoCart.GetAllAsync()).FirstOrDefault(c => c.User == user);
                        _repoCart.AddProductToCart(cart.Id, p.Id);
                        return NoContent();
                    }
                }
            return BadRequest();
          
        }
        [HttpDelete("RemoveCartItem/{id:int}")]
        public IActionResult RemoveCartItem([FromRoute]int id)
        {
            if (id != 0)
            {

                var cartItem = _repoCartItem.SelecteOne(c => c.ProductId == id);
                if (cartItem != null)
                {
                    _repoCartItem.DeleteEntity(cartItem);
                    return NoContent();
                }
            }
            return BadRequest();

        }
    }
}
