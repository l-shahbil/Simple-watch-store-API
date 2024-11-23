using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

        [ForeignKey("UserId")]
        [InverseProperty("Cart")]
        public virtual AspNetUser User { get; set; } = null!;
        [InverseProperty("Cart")]
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
