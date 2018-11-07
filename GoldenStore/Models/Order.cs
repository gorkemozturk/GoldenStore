using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Models
{
    public class Order
    {
        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }

        [Required]
        public decimal Total { get; set; }

        public string Status { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
