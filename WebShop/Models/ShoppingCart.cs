using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
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

        public List<CartItem> CartItems { get; set; }

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



    //Old Version
 /*   public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }*/
}
