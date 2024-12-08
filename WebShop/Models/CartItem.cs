using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string? ImgPath { get; set; }
        public int ProductCategoryId { get; set; }
        [ForeignKey(nameof(ProductCategoryId))]
        [ValidateNever]
        public ProductCategory Category { get; set; }
        public int SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        [ValidateNever]
        public Supplier Supplier { get; set; }
        public int Quantity { get; set; }
        [Column("Ammount")]
        [Required]
        [Precision(18, 2)]
        [Range(0, 9999999999.99)]
        public decimal ItemAmmount { get; set; }
    }
}
