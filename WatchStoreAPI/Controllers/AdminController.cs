using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WatchStoreAPI.Models;
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

        public AdminController(UserManager<ApplicationUser> userManger,ICartRepository repoCart)
        {
            _userManger = userManger;
            _repoCart = repoCart;
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
        [HttpGet("GetUserCart")]
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

    }
}
