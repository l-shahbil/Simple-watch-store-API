using Microsoft.EntityFrameworkCore;
using WatchStoreAPI.Data;
using WatchStoreAPI.Models;
using WatchStoreAPI.Models.DTO;
using WatchStoreAPI.Repository.Base;

namespace WatchStoreAPI.Repository
{
    public class CartRepoImplement : ImplementRepo<Cart>, ICartRepository
    {
        private readonly appDbContext _context;

        public CartRepoImplement(appDbContext context) : base(context)
        {
            _context = context;
        }
        public void AddProductToCart(int cartId, int productId)
        {
            var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.Id == cartId);
            if (cart == null)
                throw new Exception("Cart not found");

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new Exception("Product not found");

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                existingCartItem.Price = product.Price * existingCartItem.Quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = 1,
                    Price = product.Price
                };
                _context.CartItems.Add(newCartItem);
            }
            _context.SaveChanges();
        }
        public List<CartDto> ShowCartItem(string UserId)
        {
            var cart = _context.Carts.Include(cart => cart.CartItems).ThenInclude(cartItems => cartItems.Product).FirstOrDefault(cart => cart.UserId == UserId);
            List<CartDto> cartDtoList = new List<CartDto>();
            foreach (var cartItem in cart.CartItems)
            {
                CartDto cDto = new CartDto();
                cDto.ProductId = cartItem.Product.Id;
                cDto.ProductName = cartItem.Product.Name;
                cDto.ProductDescription = cartItem.Product.Description;
                cDto.ProductModelNumber = cartItem.Product.ModelNumber;
                cDto.ProductImgPath = cartItem.Product.ImgPath;
                cDto.price = cartItem.Price;
                cDto.Quantity = cartItem.Quantity;

                cartDtoList.Add(cDto);
            }
            return cartDtoList;
        }
    }
}
