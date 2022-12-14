using System.ComponentModel.DataAnnotations;

namespace GeneralStoreMVC.Models.Customer
{
    public class CustomerCreate
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
