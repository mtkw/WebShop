using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public double Price { get; set; }
        public string Currency {  get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public int ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        [ValidateNever]
        public ProductCategory Category { get; set; }
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        [ValidateNever]
        public Supplier Supplier { get; set; }

        public int Quantity { get; set; }
    }
}
