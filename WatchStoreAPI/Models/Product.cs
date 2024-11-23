using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WatchStoreAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ModelNumber { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImgPath { get; set; }
        public double Price { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
