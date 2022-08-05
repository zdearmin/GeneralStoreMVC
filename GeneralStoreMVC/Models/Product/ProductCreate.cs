using System.ComponentModel.DataAnnotations;

namespace GeneralStoreMVC.Models.Product
{
    public class ProductCreate
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        public int QuantityInStock { get; set; }
        public double Price { get; set; }
    }
}
