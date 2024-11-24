using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO;

namespace WatchStoreAPI.Repository.Base
{
    public interface ICartRepository : IRepository<Cart>
    {
        void AddProductToCart(int cartId, int productId);
        List<CartDto> ShowCartItem(string UserId);


    } 
}
