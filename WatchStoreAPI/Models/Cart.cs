using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WatchStoreAPI.Models.DTO;

namespace WatchStoreAPI.Models
{
    [Table("carts")]
    [Index("UserId", Name = "IX_carts_UserID", IsUnique = true)]
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        [Key]
        public int Id { get; set; }
        [Column("UserID")]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; }

        [InverseProperty("Cart")]
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
