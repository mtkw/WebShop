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
        public int ShoppingCartId { get; set; }
        [ForeignKey(nameof(ShoppingCartId))]
        [ValidateNever]
        public ShoppingCart ShoppingCart { get; set; }
       
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; }
        [Required]
        [Precision(18, 2)]
        [Range(0, 9999999999.99)]
        public decimal TotalAmmount { get; set; }
        public int TotalQuantity { get; set; }
    }
}
