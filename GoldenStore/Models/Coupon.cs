using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Models
{
    public class Coupon
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Coupon Type")]
        public string CouponType { get; set; }
        public enum ECouponType { Percent = 0, Cash = 1 }

        [Required]
        public double Discount { get; set; }

        [Required]
        [Display(Name = "Minimum Amount")]
        public double MinimumAmount { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}
