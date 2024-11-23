using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WatchStoreAPI.Models
{
    [Table("cartItems")]
    [Index("ProductId", Name = "IX_cartItems_ProductID")]
    public partial class CartItem
    {
        [Key]
        [Column("CartID")]
        public int CartId { get; set; }
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        [ForeignKey("CartId")]
        [InverseProperty("CartItems")]
        public virtual Cart Cart { get; set; } = null!;
        [ForeignKey("ProductId")]
        [InverseProperty("CartItems")]
        public virtual Product Product { get; set; } = null!;
    }
}
