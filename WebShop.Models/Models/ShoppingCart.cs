using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.Models
{
    //New Version
    public class ShoppingCart
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        [Column("Ammount")]
        [Required]
        [Precision(18, 2)]
        [Range(0, 9999999999.99)]
        public decimal Ammount { get; set; }

        [Column("CartCountItems")]
        [Required]
        [Range(0, 99999999)]
        public int CartCountItems { get; set; }
    }
}
