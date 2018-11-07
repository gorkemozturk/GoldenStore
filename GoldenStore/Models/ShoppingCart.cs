using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Models
{
    public class ShoppingCart
    {
        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0.")]
        public int Count { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
